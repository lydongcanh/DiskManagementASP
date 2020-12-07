namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Gender", c => c.String());
            AddColumn("dbo.Users", "Experience", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Experience");
            DropColumn("dbo.Users", "Gender");
        }
    }
}
