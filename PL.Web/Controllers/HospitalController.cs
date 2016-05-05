using BLL.Service;
using DAL.Entity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PL.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PL.Web.Controllers
{
    public class HospitalController : ApiController
    {
        private readonly IRequestMemberService _requestMember;
        public HospitalController(IRequestMemberService requestMember)
        {
            _requestMember = requestMember;

        }

        public HttpResponseMessage Get()
        {
            try
            {
                var results = _requestMember.GetHospital();
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public string Get(int id)
        {
            return "value";
        }

        public HttpResponseMessage Post([FromBody]Hospital value)
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

        public HttpResponseMessage Put([FromBody]Hospital value)
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
      
        public void Delete(int id)
        {
        }
    }
}