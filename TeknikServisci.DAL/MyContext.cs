using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using TeknikServisci.Models.Entities;
using TeknikServisci.Models.IdentityModels;

namespace TeknikServisci.DAL
{
    public class MyContext : IdentityDbContext<User>
    {
        public MyContext() : base("name=MyCon")
        {
            this.InstanceDate = DateTime.Now;
        }

        public DateTime InstanceDate { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Failure>()
                .Property(x => x.Price)
                .HasPrecision(6, 2);
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Failure> Failures { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }
    }
}
