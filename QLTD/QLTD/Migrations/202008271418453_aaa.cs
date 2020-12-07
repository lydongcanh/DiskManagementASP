namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductInfors", "AB_Unit", c => c.String());
            AddColumn("dbo.ProductInfors", "AB_Amount", c => c.Double(nullable: false));
            AddColumn("dbo.ProductInfors", "UnitPackage", c => c.String());
            AddColumn("dbo.ProductInfors", "AB1", c => c.String());
            AddColumn("dbo.ProductInfors", "AB1_Amount", c => c.Double(nullable: false));
            AddColumn("dbo.ProductInfors", "AB1_Unit", c => c.String());
            AddColumn("dbo.ProductInfors", "AB2", c => c.String());
            AddColumn("dbo.ProductInfors", "AB2_Amount", c => c.Double(nullable: false));
            AddColumn("dbo.ProductInfors", "AB2_Unit", c => c.String());
            AddColumn("dbo.ProductInfors", "AB3", c => c.String());
            AddColumn("dbo.ProductInfors", "AB3_Amount", c => c.Double(nullable: false));
            AddColumn("dbo.ProductInfors", "AB3_Unit", c => c.String());
            AddColumn("dbo.ProductInfors", "AB4", c => c.String());
            AddColumn("dbo.ProductInfors", "AB4_Amount", c => c.Double(nullable: false));
            AddColumn("dbo.ProductInfors", "AB4_Unit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductInfors", "AB4_Unit");
            DropColumn("dbo.ProductInfors", "AB4_Amount");
            DropColumn("dbo.ProductInfors", "AB4");
            DropColumn("dbo.ProductInfors", "AB3_Unit");
            DropColumn("dbo.ProductInfors", "AB3_Amount");
            DropColumn("dbo.ProductInfors", "AB3");
            DropColumn("dbo.ProductInfors", "AB2_Unit");
            DropColumn("dbo.ProductInfors", "AB2_Amount");
            DropColumn("dbo.ProductInfors", "AB2");
            DropColumn("dbo.ProductInfors", "AB1_Unit");
            DropColumn("dbo.ProductInfors", "AB1_Amount");
            DropColumn("dbo.ProductInfors", "AB1");
            DropColumn("dbo.ProductInfors", "UnitPackage");
            DropColumn("dbo.ProductInfors", "AB_Amount");
            DropColumn("dbo.ProductInfors", "AB_Unit");
        }
    }
}
