namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiskTitles", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DiskTitles", "Image");
        }
    }
}
