using BLL.Service;
using DAL.Data;
using DAL.Entity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PL.Web.Helpers;
using PL.Web.Providers;
using System;
using System.Configuration;

namespace PL.Web
{
    public partial class Startup
    {
        //Enable OWIN Bearer Token Middleware	
        static Startup()
        {
            PublicClientId = "self";

            UserManagerFactory = () => new ApplicationUserManager(new UserStore<UserProfile>(new DataContext()));

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                //Exposes Token endpoint
                TokenEndpointPath = new PathString("/Token"),
                //Use ApplicationOAuthProvider in order to authenticate
                Provider = new ApplicationOAuthProvider(PublicClientId, UserManagerFactory),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14), //Token expiration => The user will remain authenticated for 14 days
                AllowInsecureHttp = true
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static System.Func<ApplicationUserManager> UserManagerFactory { get; set; }

        public static string PublicClientId { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new DataContext());
            app.CreatePerOwinContext<ApplicationUserManager>(Create);

            //Enable Cors support in the Web API. Should go before the activation of Bearer tokens
            //http://aspnetwebstack.codeplex.com/discussions/467315
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enable the application to use bearer tokens to authenticate users
            // Enabling 3 components:
            // 1. Authorization Server middleware. For creating the bearer tokens
            // 2. Application bearer token middleware. Will atuthenticate every request with Authorization : Bearer header
            // 3. External bearer token middleware. For external providers
            app.UseOAuthBearerTokens(OAuthOptions);

            app.UseMicrosoftAccountAuthentication(ConfigurationManager.AppSettings["MicrosoftKey"], ConfigurationManager.AppSettings["MicrosoftSecret"]);

            app.UseTwitterAuthentication(ConfigurationManager.AppSettings["TwitterKey"], ConfigurationManager.AppSettings["TwitterSecret"]);

            app.UseFacebookAuthentication(ConfigurationManager.AppSettings["FacebookKey"], ConfigurationManager.AppSettings["FacebookSecret"]);

            //app.UseGoogleAuthentication();       
        }

        private static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<UserProfile>(context.Get<DataContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<UserProfile>(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            manager.UserLockoutEnabledByDefault = true;

            manager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<UserProfile>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
        private static bool IsAjaxRequest(IOwinRequest request)
        {
            IReadableStringCollection query = request.Query;
            if ((query != null) && (query["X-Requested-With"] == "XMLHttpRequest"))
            {
                return true;
            }
            IHeaderDictionary headers = request.Headers;
            return ((headers != null) && (headers["X-Requested-With"] == "XMLHttpRequest"));
        }
    }
}