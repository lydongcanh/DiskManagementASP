using Ehr.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ehr.Data
{
    public class EhrDbContext : DbContext
    {
        public EhrDbContext() : base ("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<EhrDbContext>(null);
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("TIMESTAMP"));
            //modelBuilder.Entity<EProject>()
            //               .HasMany<User>(s => s.ProjectMembers)
            //               .WithMany(p => p.Projects)
            //               .Map(pu =>
            //               {
            //                   pu.MapLeftKey("Project_Id");
            //                   pu.MapRightKey("User_Id");
            //                   pu.ToTable("EProjectUser");
            //               });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
    }
}