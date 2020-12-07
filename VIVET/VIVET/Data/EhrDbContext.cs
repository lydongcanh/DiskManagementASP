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
        public DbSet<AuditTrail> AuditTrails { get; set; }
		public DbSet<MailConfig> MailConfigs { get; set; }
        public DbSet<LoginAuditTrail> LoginAuditTrails { get; set; }
        public DbSet<ActionAuditTrail> ActionAuditTrails { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<ProductRequired> ProductRequireds { get; set; }
        public DbSet<AntimicrobialRequired> AntimicrobialRequireds { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<ProductInfor> ProductInfors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AntimicroBial> AntimicroBials { get; set; }
        public DbSet<OrtherAB> OrtherABs { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalInfor> AnimalInfors { get; set; }
        public DbSet<Antimi> Antimis { get; set; }
    }
}