using DAL.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity.Models
{
    public partial class Hospital : EntityBase
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string AuthenServiceURL { get; set; }
        public string HealthbookServiceURL { get; set; }
        public string RecordStatus { get; set; } 
    }
}
