namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductInfors", "AntiAmount", c => c.Double(nullable: false));
            AddColumn("dbo.ProductInfors", "IsPrescription", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductInfors", "IsPrescription");
            DropColumn("dbo.ProductInfors", "AntiAmount");
        }
    }
}
