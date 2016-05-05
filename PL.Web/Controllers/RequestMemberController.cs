using BLL.Service;
using DAL.Entity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PL.Web.Helpers;
using PL.Web.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PL.Web.Hubs;
using Microsoft.AspNet.SignalR;

namespace PL.Web.Controllers
{
    public class RequestMemberController : ApiController
    {
        private ApplicationUserManager userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        private readonly IRequestMemberService _requestMemberService;
        private readonly IBaseService _baseService;
        public RequestMemberController(IRequestMemberService requestMember, IBaseService baseService)
        {
            _requestMemberService = requestMember;
            _baseService = baseService;
        }

        public HttpResponseMessage Get()
        {
            try
            {
                var results = _requestMemberService.Get();
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Get(string email)
        {
            try
            {
                var results = _requestMemberService.Get(email);
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Post([FromBody]RequestMember value)
        {
            try
            {
                if (value == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, _baseService.GetDataInvalidError().ResponseMsg);

                if (ModelState.IsValid)
                {
                    _requestMemberService.Register(value);
                    var context = GlobalHost.ConnectionManager.GetHubContext<HealthBookHub>();
                    context.Clients.All.hasNewMember(1);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, _baseService.GetModelStateError(ModelState).ResponseMsg);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, _baseService.GetExceptionError(ex).ResponseMsg);
            }
        }

        private string GeneratePassword(string origin)
        {
            if (!string.IsNullOrEmpty(origin))
            {
                var maxLength = origin.Length;
                var starter = maxLength - 6;
                var password = origin.Substring(starter);
                return password;
            }
            return "342301";
        }

        public HttpResponseMessage Put([FromBody]RequestMember value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userPassword = string.Empty;

                    //Register to AspNetUser only Approved status
                    if (value.RegisterStatus.ToUpper() == "APPROVED")
                    {
                        userPassword = GeneratePassword(value.IPPassport);
                        UserProfile user = new UserProfile
                        {
                            UserName = value.Email,
                            Email = value.Email,
                            EmailConfirmed = false
                        };

                        IdentityResult identityResult = UserManager.Create(user, userPassword);

                        IHttpActionResult createResult = GetErrorResult(identityResult);

                        if (createResult != null)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, createResult);
                        }

                        UserProfile justCreatedUser = UserManager.FindByName(value.Email);

                        IdentityResult roleResult = UserManager.AddToRole(justCreatedUser.Id, "User");

                        IHttpActionResult addRoleResult = GetErrorResult(roleResult);

                        if (addRoleResult != null)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, addRoleResult);
                        }

                        string code = UserManager.GenerateEmailConfirmationToken(user.Id);
                        var callbackUrl = Url.Link("ConfirmEmail", new { userId = user.Id, code = code });

                        var notification = new AccountNotificationModel
                        {
                            Code = code,
                            Url = callbackUrl,
                            UserId = justCreatedUser.Id,
                            Email = justCreatedUser.Email,
                            DisplayName = justCreatedUser.UserName
                        };
                    }

                    value.UpdatedBy = _baseService.GetCurrentUser();
                    _requestMemberService.SetMemberStatus(value, userPassword);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, _baseService.GetDataInvalidError());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, _baseService.GetExceptionError(ex).ResponseMsg);
            }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        public HttpResponseMessage Delete(string email)
        {
            try
            {
                UserProfile user = UserManager.FindByEmail(email);

                if (user == null)
                {
                    _requestMemberService.Delete(email);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                IdentityResult result = UserManager.Delete(user);

                if (result.Succeeded)
                {
                    _requestMemberService.Delete(email);
                }

                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResult);
                }

                //_requestMemberService.Delete(email);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, _baseService.GetExceptionError(ex).ResponseMsg);
            }
        }
    }
}