using DAL.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity.Models
{
    public partial class RequestMember : EntityBase
    {
        [Key]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IPPassport { get; set; }
        public string Tel { get; set; }
        public string HN { get; set; }
        public string Photo { get; set; }
        public string RegisterStatus { get; set; }
    }
}
