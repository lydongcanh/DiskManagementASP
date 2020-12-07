namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EZHR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EProjectNews", "LeadTimeRange", c => c.Int(nullable: false));
            AddColumn("dbo.EProjectNews", "LeadTime", c => c.Int(nullable: false));
            AddColumn("dbo.EProjectReplaces", "LeadTimeRange", c => c.Int(nullable: false));
            AddColumn("dbo.EProjectReplaces", "LeadTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EProjectReplaces", "LeadTime");
            DropColumn("dbo.EProjectReplaces", "LeadTimeRange");
            DropColumn("dbo.EProjectNews", "LeadTime");
            DropColumn("dbo.EProjectNews", "LeadTimeRange");
        }
    }
}
