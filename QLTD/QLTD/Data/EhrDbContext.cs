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
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Disk> Disks { get; set; }
        public DbSet<DiskHold> DiskHolds { get; set; }
        public DbSet<DiskTitle> DiskTitles { get; set; }
        public DbSet<DiskType> DiskTypes { get; set; }
        public DbSet<LateCharge> LateCharges { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<RentDetail> RentDetails { get; set; }
        public DbSet<RentReceipt> RentReceipts { get; set; }
    }
}