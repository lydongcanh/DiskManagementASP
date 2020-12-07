namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EProjectNews", "StartingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.EProjectNews", "EndingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.EProjectReplaces", "StartingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.EProjectReplaces", "EndingDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EProjectReplaces", "EndingDate");
            DropColumn("dbo.EProjectReplaces", "StartingDate");
            DropColumn("dbo.EProjectNews", "EndingDate");
            DropColumn("dbo.EProjectNews", "StartingDate");
        }
    }
}
