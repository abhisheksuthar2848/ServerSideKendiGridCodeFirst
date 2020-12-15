using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ServerSideKindoGrid.Models;

namespace ServerSideKindoGrid.Models
{
    public class Context:DbContext
    {

        public Context() : base("StringDBContext1")
        {

        }

        public DbSet<Employee> Employees { get; set; }



    }

    
}