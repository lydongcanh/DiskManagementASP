namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class e3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActionAuditTrails", "DateTimeStamp", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Candidates", "DOB", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Candidates", "StartedTime", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Candidates", "DateFinalComment", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Candidates", "ContractDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Candidates", "SubmissionDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Forms", "ExpiryDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Forms", "CreationDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EProjects", "StartingDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EProjects", "EndingDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Interviews", "StartDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Interviews", "EndDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Interviews", "SendDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EProjectNews", "RequestedDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EProjectNews", "CompletedDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EProjectNews", "StartingDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EProjectNews", "EndingDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EProjectReplaces", "SubmitedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EProjectReplaces", "LeavingDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EProjectReplaces", "InformedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EProjectReplaces", "RequestedDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EProjectReplaces", "CompletedDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EProjectReplaces", "StartingDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EProjectReplaces", "EndingDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.AuditTrails", "DateTimeStamp", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.EFormAuditTrails", "DateTimeStamp", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.LoginAuditTrails", "DateTimeStamp", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Notifications", "TimeExecute", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notifications", "TimeExecute", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LoginAuditTrails", "DateTimeStamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EFormAuditTrails", "DateTimeStamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AuditTrails", "DateTimeStamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EProjectReplaces", "EndingDate", c => c.DateTime());
            AlterColumn("dbo.EProjectReplaces", "StartingDate", c => c.DateTime());
            AlterColumn("dbo.EProjectReplaces", "CompletedDate", c => c.DateTime());
            AlterColumn("dbo.EProjectReplaces", "RequestedDate", c => c.DateTime());
            AlterColumn("dbo.EProjectReplaces", "InformedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EProjectReplaces", "LeavingDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EProjectReplaces", "SubmitedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EProjectNews", "EndingDate", c => c.DateTime());
            AlterColumn("dbo.EProjectNews", "StartingDate", c => c.DateTime());
            AlterColumn("dbo.EProjectNews", "CompletedDate", c => c.DateTime());
            AlterColumn("dbo.EProjectNews", "RequestedDate", c => c.DateTime());
            AlterColumn("dbo.Interviews", "SendDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Interviews", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Interviews", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EProjects", "EndingDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EProjects", "StartingDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Forms", "CreationDate", c => c.DateTime());
            AlterColumn("dbo.Forms", "ExpiryDate", c => c.DateTime());
            AlterColumn("dbo.Candidates", "SubmissionDate", c => c.DateTime());
            AlterColumn("dbo.Candidates", "ContractDate", c => c.DateTime());
            AlterColumn("dbo.Candidates", "DateFinalComment", c => c.DateTime());
            AlterColumn("dbo.Candidates", "StartedTime", c => c.DateTime());
            AlterColumn("dbo.Candidates", "DOB", c => c.DateTime());
            AlterColumn("dbo.ActionAuditTrails", "DateTimeStamp", c => c.DateTime(nullable: false));
        }
    }
}
