using Ehr.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ehr.Data
{
    public class QLTDDBContext : DbContext
    {
        public QLTDDBContext() : base ("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<QLTDDBContext>(null);
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
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
        public DbSet<OrderLateCharge> Orders { get; set; }
        public DbSet<OrderRent> Rents { get; set; }
        public DbSet<OrderDetail> RentDetails { get; set; }
        public DbSet<OrderReceipt> RentReceipts { get; set; }
    }
}