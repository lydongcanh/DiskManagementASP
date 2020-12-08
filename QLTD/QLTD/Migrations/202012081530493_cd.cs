namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.RentReceipts", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Rents", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rents", "Status");
            DropColumn("dbo.RentReceipts", "Status");
            DropColumn("dbo.Customers", "Status");
        }
    }
}
