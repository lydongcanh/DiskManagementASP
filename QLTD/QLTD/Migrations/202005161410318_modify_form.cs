namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_form : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VacancyForms", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.VacancyForms", "Form_Id", "dbo.Forms");
            DropIndex("dbo.VacancyForms", new[] { "Vacancy_Id" });
            DropIndex("dbo.VacancyForms", new[] { "Form_Id" });
            AddColumn("dbo.Forms", "Vacancy", c => c.Int(nullable: false));
            AddColumn("dbo.Forms", "YearExperiences", c => c.Int(nullable: false));
            AddColumn("dbo.Forms", "Vacancy_Id", c => c.Int());
            CreateIndex("dbo.Forms", "Vacancy_Id");
            AddForeignKey("dbo.Forms", "Vacancy_Id", "dbo.Vacancies", "Id");
            DropColumn("dbo.Forms", "Store");
            DropTable("dbo.VacancyForms");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.VacancyForms",
                c => new
                    {
                        Vacancy_Id = c.Int(nullable: false),
                        Form_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vacancy_Id, t.Form_Id });
            
            AddColumn("dbo.Forms", "Store", c => c.Int(nullable: false));
            DropForeignKey("dbo.Forms", "Vacancy_Id", "dbo.Vacancies");
            DropIndex("dbo.Forms", new[] { "Vacancy_Id" });
            DropColumn("dbo.Forms", "Vacancy_Id");
            DropColumn("dbo.Forms", "YearExperiences");
            DropColumn("dbo.Forms", "Vacancy");
            CreateIndex("dbo.VacancyForms", "Form_Id");
            CreateIndex("dbo.VacancyForms", "Vacancy_Id");
            AddForeignKey("dbo.VacancyForms", "Form_Id", "dbo.Forms", "Id", cascadeDelete: true);
            AddForeignKey("dbo.VacancyForms", "Vacancy_Id", "dbo.Vacancies", "Id", cascadeDelete: true);
        }
    }
}
