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

namespace PL.Web.Controllers
{
    public class OnlineMemberController : ApiController
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
        public OnlineMemberController(IRequestMemberService requestMember, IBaseService baseService)
        {
            _requestMemberService = requestMember;
            _baseService = baseService;
        }

        public HttpResponseMessage Get()
        {
            try
            {
                var results = _requestMemberService.GetMember();
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, _baseService.GetExceptionError(ex).ResponseMsg);
            }
        }

        public HttpResponseMessage Get(string id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK);
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
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, _baseService.GetExceptionError(ex).ResponseMsg);
            }
        }

        public HttpResponseMessage Put([FromBody]RequestMember value)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, _baseService.GetExceptionError(ex).ResponseMsg);
            }
        }

        public void Delete(int id)
        {
        }
    }
}