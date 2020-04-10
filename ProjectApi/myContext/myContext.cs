using ProjectApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjectApi.MyContext
{
    public class myContext : DbContext
    {
        public myContext() : base("DB_API") { }
        public DbSet<Department> department { get; set; }
        public DbSet<Devisi> devisi { get; set; }
    }
}