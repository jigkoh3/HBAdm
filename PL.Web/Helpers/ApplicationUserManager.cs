using DAL.Entity.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PL.Web.Helpers
{
    public class ApplicationUserManager : UserManager<UserProfile>
    {
        public ApplicationUserManager(IUserStore<UserProfile> store)
            : base(store)
        {

        }
    }
}