namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductInfors", "Volume_AVG_Unit", c => c.String());
            AddColumn("dbo.ProductInfors", "QuantityUnit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductInfors", "QuantityUnit");
            DropColumn("dbo.ProductInfors", "Volume_AVG_Unit");
        }
    }
}
