namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rents", "RentLenght", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rents", "RentLenght");
        }
    }
}
