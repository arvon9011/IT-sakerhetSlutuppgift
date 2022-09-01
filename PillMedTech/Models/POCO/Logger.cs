using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PillMedTech.Models
{
    

    public class Logger
    {
        [Key]
        public int Lognmr { get; set; }
        public string EmployeeId { get; set; }
        public string Time { get; set; }
        public string Ip { get; set; }
        public string Action { get; set; }

    }
}
