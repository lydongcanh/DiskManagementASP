namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ehr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EProjectReplaces", "TargetOfWeek", c => c.Int(nullable: false));
            DropColumn("dbo.EProjectReplaces", "PG_SUB_NAME");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EProjectReplaces", "PG_SUB_NAME", c => c.String());
            DropColumn("dbo.EProjectReplaces", "TargetOfWeek");
        }
    }
}
