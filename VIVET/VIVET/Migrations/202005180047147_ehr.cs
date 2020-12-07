namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ehr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidates", "ProjectNew_Id", c => c.Int());
            AddColumn("dbo.Candidates", "ProjectReplace_Id", c => c.Int());
            CreateIndex("dbo.Candidates", "ProjectNew_Id");
            CreateIndex("dbo.Candidates", "ProjectReplace_Id");
            AddForeignKey("dbo.Candidates", "ProjectNew_Id", "dbo.EProjectNews", "Id");
            AddForeignKey("dbo.Candidates", "ProjectReplace_Id", "dbo.EProjectReplaces", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Candidates", "ProjectReplace_Id", "dbo.EProjectReplaces");
            DropForeignKey("dbo.Candidates", "ProjectNew_Id", "dbo.EProjectNews");
            DropIndex("dbo.Candidates", new[] { "ProjectReplace_Id" });
            DropIndex("dbo.Candidates", new[] { "ProjectNew_Id" });
            DropColumn("dbo.Candidates", "ProjectReplace_Id");
            DropColumn("dbo.Candidates", "ProjectNew_Id");
        }
    }
}
