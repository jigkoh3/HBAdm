using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PL.Web.Startup))]

namespace PL.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //var idProvider = new CustomUserIdProvider();

            //GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);          

            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
