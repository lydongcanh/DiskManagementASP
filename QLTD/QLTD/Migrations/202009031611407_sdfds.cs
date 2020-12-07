namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdfds : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductInfors", "AmountPet", c => c.Double(nullable: false));
            AlterColumn("dbo.ProductInfors", "AmountProduct", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductInfors", "AmountProduct", c => c.Int(nullable: false));
            AlterColumn("dbo.ProductInfors", "AmountPet", c => c.Int(nullable: false));
        }
    }
}
