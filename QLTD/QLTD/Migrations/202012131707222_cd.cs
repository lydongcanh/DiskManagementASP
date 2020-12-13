namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DiskHolds", "DiskTitle_Id", "dbo.DiskTitles");
            DropIndex("dbo.DiskHolds", new[] { "DiskTitle_Id" });
            AddColumn("dbo.DiskHolds", "Disk_Id", c => c.Int());
            CreateIndex("dbo.DiskHolds", "Disk_Id");
            AddForeignKey("dbo.DiskHolds", "Disk_Id", "dbo.Disks", "Id");
            DropColumn("dbo.DiskHolds", "DiskTitle_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DiskHolds", "DiskTitle_Id", c => c.Int());
            DropForeignKey("dbo.DiskHolds", "Disk_Id", "dbo.Disks");
            DropIndex("dbo.DiskHolds", new[] { "Disk_Id" });
            DropColumn("dbo.DiskHolds", "Disk_Id");
            CreateIndex("dbo.DiskHolds", "DiskTitle_Id");
            AddForeignKey("dbo.DiskHolds", "DiskTitle_Id", "dbo.DiskTitles", "Id");
        }
    }
}
