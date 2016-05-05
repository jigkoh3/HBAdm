using BLL.Service;
using DAL.Data;
using DAL.Repository;
using DAL.Repository.Providers.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using PL.Web.Controllers;
using PL.Web.Models;
using System.Web.Http;
using Unity.WebApi;

namespace PL.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IBaseService, BaseService>();
            container.RegisterType<IRequestMemberService, RequestMemberService>();
            container.RegisterType<IDbContext, DataContext>("DefaultConnection");
            container.RegisterType<IUnitOfWork, UnitOfWork>("UnitOfWork", new HierarchicalLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<IDbContext>("DefaultConnection")));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}