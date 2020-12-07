namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidates", "R1_Eval1", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R1_Eval2", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R1_Eval3", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R1_Eval4", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R1_Eval5", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R1_Eval6", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R1_Eval7", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R1_Eval8", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R1_Eval9", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R1_Eval10", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R2_Eval1", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R2_Eval2", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R2_Eval3", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R2_Eval4", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R2_Eval5", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R2_Eval6", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R2_Eval7", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R2_Eval8", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R2_Eval9", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R2_Eval10", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R3_Eval1", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R3_Eval2", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R3_Eval3", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R3_Eval4", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R3_Eval5", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R3_Eval6", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R3_Eval7", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R3_Eval8", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R3_Eval9", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "R3_Eval10", c => c.Int(nullable: false));
            AddColumn("dbo.EProjects", "Hotline", c => c.String());
            AddColumn("dbo.Interviews", "InterviewerPhone", c => c.String());
            AddColumn("dbo.Vacancies", "Eval1", c => c.String());
            AddColumn("dbo.Vacancies", "Eval2", c => c.String());
            AddColumn("dbo.Vacancies", "Eval3", c => c.String());
            AddColumn("dbo.Vacancies", "Eval4", c => c.String());
            AddColumn("dbo.Vacancies", "Eval5", c => c.String());
            AddColumn("dbo.Vacancies", "Eval6", c => c.String());
            AddColumn("dbo.Vacancies", "Eval7", c => c.String());
            AddColumn("dbo.Vacancies", "Eval8", c => c.String());
            AddColumn("dbo.Vacancies", "Eval9", c => c.String());
            AddColumn("dbo.Vacancies", "Eval10", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vacancies", "Eval10");
            DropColumn("dbo.Vacancies", "Eval9");
            DropColumn("dbo.Vacancies", "Eval8");
            DropColumn("dbo.Vacancies", "Eval7");
            DropColumn("dbo.Vacancies", "Eval6");
            DropColumn("dbo.Vacancies", "Eval5");
            DropColumn("dbo.Vacancies", "Eval4");
            DropColumn("dbo.Vacancies", "Eval3");
            DropColumn("dbo.Vacancies", "Eval2");
            DropColumn("dbo.Vacancies", "Eval1");
            DropColumn("dbo.Interviews", "InterviewerPhone");
            DropColumn("dbo.EProjects", "Hotline");
            DropColumn("dbo.Candidates", "R3_Eval10");
            DropColumn("dbo.Candidates", "R3_Eval9");
            DropColumn("dbo.Candidates", "R3_Eval8");
            DropColumn("dbo.Candidates", "R3_Eval7");
            DropColumn("dbo.Candidates", "R3_Eval6");
            DropColumn("dbo.Candidates", "R3_Eval5");
            DropColumn("dbo.Candidates", "R3_Eval4");
            DropColumn("dbo.Candidates", "R3_Eval3");
            DropColumn("dbo.Candidates", "R3_Eval2");
            DropColumn("dbo.Candidates", "R3_Eval1");
            DropColumn("dbo.Candidates", "R2_Eval10");
            DropColumn("dbo.Candidates", "R2_Eval9");
            DropColumn("dbo.Candidates", "R2_Eval8");
            DropColumn("dbo.Candidates", "R2_Eval7");
            DropColumn("dbo.Candidates", "R2_Eval6");
            DropColumn("dbo.Candidates", "R2_Eval5");
            DropColumn("dbo.Candidates", "R2_Eval4");
            DropColumn("dbo.Candidates", "R2_Eval3");
            DropColumn("dbo.Candidates", "R2_Eval2");
            DropColumn("dbo.Candidates", "R2_Eval1");
            DropColumn("dbo.Candidates", "R1_Eval10");
            DropColumn("dbo.Candidates", "R1_Eval9");
            DropColumn("dbo.Candidates", "R1_Eval8");
            DropColumn("dbo.Candidates", "R1_Eval7");
            DropColumn("dbo.Candidates", "R1_Eval6");
            DropColumn("dbo.Candidates", "R1_Eval5");
            DropColumn("dbo.Candidates", "R1_Eval4");
            DropColumn("dbo.Candidates", "R1_Eval3");
            DropColumn("dbo.Candidates", "R1_Eval2");
            DropColumn("dbo.Candidates", "R1_Eval1");
        }
    }
}
