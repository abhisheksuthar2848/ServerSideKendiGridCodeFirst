using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServerSideKindoGrid.Models
{
    public class Employee
    {
        [Key]

        public int EmpId { get; set; }
       
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address { get; set; }
    }
}