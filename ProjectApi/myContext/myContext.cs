using ProjectApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjectApi.myContext
{
    public class myContext : DbContext
    {
        public myContext() : base("BelajarAPI") { }
        public DbSet<Department> department { get; set; }
    }
}