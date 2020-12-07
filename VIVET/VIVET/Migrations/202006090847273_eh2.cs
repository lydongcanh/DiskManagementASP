namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eh2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Vacancies", name: "Group_Id", newName: "GroupVacancy_Id");
            RenameIndex(table: "dbo.Vacancies", name: "IX_Group_Id", newName: "IX_GroupVacancy_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Vacancies", name: "IX_GroupVacancy_Id", newName: "IX_Group_Id");
            RenameColumn(table: "dbo.Vacancies", name: "GroupVacancy_Id", newName: "Group_Id");
        }
    }
}
