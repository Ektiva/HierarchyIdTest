using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HierarchyIdTest1.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Domain> Domains { get; set; }
        public DbSet<DomainType> DomainTypes { get; set; }
        public DbSet<NewDomain> NewDomains { get; set; }

        //public DbSet<DomainTest> DomainTests { get; set; }
    }
}
