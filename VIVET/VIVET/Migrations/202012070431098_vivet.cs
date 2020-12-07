namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vivet : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ProductOrigin", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "TypeOfProduct", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "Other_Subtance_In_Product", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "Unit_Of_Volume_Of_Product", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Unit_Of_Volume_Of_Product", c => c.String());
            AlterColumn("dbo.Products", "Other_Subtance_In_Product", c => c.String());
            AlterColumn("dbo.Products", "TypeOfProduct", c => c.String());
            AlterColumn("dbo.Products", "ProductOrigin", c => c.String());
        }
    }
}
