namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sss : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductInfors", "CollectedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Questionnaires", "CollectedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questionnaires", "CollectedDate");
            DropColumn("dbo.ProductInfors", "CollectedDate");
        }
    }
}
