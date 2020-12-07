namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiskTypes", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DiskTypes", "Code");
        }
    }
}
