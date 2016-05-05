using DAL.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Runtime.Serialization;

namespace DAL.Entity.Models
{
    /// <summary>
    ///  User Profile entity
    /// </summary>
    [DataContract(IsReference = true)]
    public class UserProfile : IdentityUser, IUser
    {

    }

    public partial class AppUserProfile : EntityBase, IUser
    {
        //[Id] [nvarchar](128) NOT NULL,
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string TwoFactorEnabled { get; set; }
        public string LockoutEndDateUtc { get; set; }
        public string LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
    }
}
