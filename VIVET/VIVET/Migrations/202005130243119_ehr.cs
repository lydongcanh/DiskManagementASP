namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ehr : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionAuditTrails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Username = c.String(),
                        DateTimeStamp = c.DateTime(nullable: false),
                        Action = c.String(),
                        Status = c.Int(nullable: false),
                        Actiontype = c.Int(nullable: false),
                        Candidates_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidates", t => t.Candidates_Id)
                .Index(t => t.Candidates_Id);
            
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Approval = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        Comment = c.String(),
                        FinalComment = c.String(),
                        DateFinalComment = c.DateTime(),
                        RoundPassed = c.Int(nullable: false),
                        ScreeningScore = c.Int(nullable: false),
                        ContractDate = c.DateTime(),
                        SubmissionDate = c.DateTime(),
                        IP = c.String(),
                        CandidateSource = c.Int(nullable: false),
                        SourceDetail = c.String(),
                        InternalSourceName = c.String(),
                        InternalSourceCode = c.String(),
                        FullName = c.String(),
                        DOB = c.DateTime(),
                        Sex = c.Int(nullable: false),
                        Height = c.Single(nullable: false),
                        Weight = c.Single(nullable: false),
                        EducationLevel = c.Int(nullable: false),
                        Address = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Experiences = c.String(),
                        Facebook = c.String(),
                        Zalo = c.String(),
                        Photos = c.String(),
                        CV = c.String(),
                        StartedTime = c.DateTime(),
                        Agreement = c.Int(nullable: false),
                        ExpFailed = c.Int(nullable: false),
                        FormFailed = c.Int(nullable: false),
                        MailFailed = c.Int(nullable: false),
                        Form_Id = c.Int(),
                        Position_Id = c.Int(),
                        CommentBy_Id = c.Int(),
                        Region_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Forms", t => t.Form_Id)
                .ForeignKey("dbo.Vacancies", t => t.Position_Id)
                .ForeignKey("dbo.Users", t => t.CommentBy_Id)
                .ForeignKey("dbo.Regions", t => t.Region_Id)
                .Index(t => t.Form_Id)
                .Index(t => t.Position_Id)
                .Index(t => t.CommentBy_Id)
                .Index(t => t.Region_Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityName = c.String(),
                        IsProvince = c.Boolean(nullable: false),
                        Region_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.Region_Id)
                .Index(t => t.Region_Id);
            
            CreateTable(
                "dbo.Forms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormName = c.String(),
                        Banner = c.String(),
                        FormLink = c.String(),
                        CreationDate = c.DateTime(),
                        ExpiryDate = c.DateTime(),
                        CandidateSource = c.Int(nullable: false),
                        SourceDetail = c.Int(nullable: false),
                        InternalSourceName = c.Int(nullable: false),
                        InternalSourceCode = c.Int(nullable: false),
                        FullName = c.Int(nullable: false),
                        DOB = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        EducationLevel = c.Int(nullable: false),
                        Address = c.Int(nullable: false),
                        Email = c.Int(nullable: false),
                        PhoneNumber = c.Int(nullable: false),
                        Experiences = c.Int(nullable: false),
                        Facebook = c.Int(nullable: false),
                        Zalo = c.Int(nullable: false),
                        Photos = c.Int(nullable: false),
                        CV = c.Int(nullable: false),
                        StartedTime = c.Int(nullable: false),
                        Agreement = c.Int(nullable: false),
                        Store = c.Int(nullable: false),
                        Project_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatusActive = c.Int(nullable: false),
                        ProjectName = c.String(),
                        TypeProject = c.Int(nullable: false),
                        NumberOfPSRequest = c.Int(),
                        NumberOfCVPerPS = c.Int(),
                        NumberOfRecordReceived = c.Int(),
                        NumberOfPass = c.Int(),
                        NumberOfPSRemaining = c.Int(),
                        TimeLine = c.DateTime(),
                        WorkFestivalDay = c.DateTime(),
                        KeyCP = c.Int(),
                        Position = c.Int(),
                        RecruitmentType = c.Int(),
                        SentDate = c.DateTime(),
                        ReceivedDate = c.DateTime(),
                        OffDate = c.DateTime(),
                        NumberOfDayOff = c.Double(),
                        NewHC = c.Int(),
                        Replacement = c.Int(),
                        TotalRequest = c.Int(),
                        WeekRequest = c.Int(),
                        DateRequest = c.DateTime(),
                        MonthRequest = c.String(),
                        DueDate = c.DateTime(),
                        CompletionDate = c.DateTime(),
                        CompletionMonth = c.String(),
                        CompletionWeek = c.Int(),
                        Completion = c.Int(),
                        Pending = c.Int(),
                        Remark = c.String(),
                        RankAchieved = c.Int(),
                        Leadtime = c.Int(),
                        NumberOfRecruimentDays = c.Int(),
                        RangeOfLeadtime = c.String(),
                        District = c.String(),
                        YearRequest = c.Int(),
                        TargetOfWeek = c.Int(),
                        Target = c.Int(),
                        CompletionOfWeek = c.Int(),
                        Month = c.String(),
                        ReplacementType = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        City_Id = c.Int(),
                        CreatedBy_Id = c.Int(),
                        Customer_Id = c.Int(),
                        LastEditedBy_Id = c.Int(),
                        Region_Id = c.Int(),
                        TeamLeader_Id = c.Int(),
                        Recruiter_Id = c.Int(),
                        RFM_Id = c.Int(),
                        HRBP_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.City_Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Users", t => t.LastEditedBy_Id)
                .ForeignKey("dbo.Regions", t => t.Region_Id)
                .ForeignKey("dbo.Users", t => t.TeamLeader_Id)
                .ForeignKey("dbo.Users", t => t.Recruiter_Id)
                .ForeignKey("dbo.Users", t => t.RFM_Id)
                .ForeignKey("dbo.Users", t => t.HRBP_Id)
                .Index(t => t.City_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.LastEditedBy_Id)
                .Index(t => t.Region_Id)
                .Index(t => t.TeamLeader_Id)
                .Index(t => t.Recruiter_Id)
                .Index(t => t.RFM_Id)
                .Index(t => t.HRBP_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 50),
                        Password = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        FullName = c.String(),
                        Image = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Address = c.String(),
                        ResetPasswordCode = c.String(),
                        ConfirmPasswordCode = c.String(),
                        EmployeeType = c.Int(nullable: false),
                        Department = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Username, unique: true);
            
            CreateTable(
                "dbo.Interviews",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        InterviewName = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        PriorityInterview = c.Int(nullable: false),
                        EachTime = c.Int(nullable: false),
                        StartTime = c.String(),
                        StopTime = c.String(),
                        StartBreakTime = c.String(),
                        EndBreakTime = c.String(),
                        Round = c.Int(nullable: false),
                        Location = c.String(),
                        IsSendMail = c.Boolean(nullable: false),
                        SendDate = c.DateTime(nullable: false),
                        CreatedBy_Id = c.Int(),
                        LastEditedBy_Id = c.Int(),
                        Project_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Users", t => t.LastEditedBy_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.LastEditedBy_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        RoleStatus = c.Int(nullable: false),
                        IsRoot = c.Int(nullable: false),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PermissionName = c.String(),
                        PermissionCode = c.String(maxLength: 20),
                        PermisstionStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.PermissionCode, unique: true);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Logo = c.String(),
                        Phone = c.String(),
                        Information = c.String(),
                        RequirementStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        Phone = c.String(),
                        Street = c.String(),
                        Ward = c.String(),
                        District = c.String(),
                        StatusActive = c.Int(nullable: false),
                        City_Id = c.Int(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.City_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.City_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegionName = c.String(),
                        RegionCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vacancies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuditTrails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        KeyFieldID = c.Long(nullable: false),
                        AuditActionType = c.Int(nullable: false),
                        DateTimeStamp = c.DateTime(nullable: false),
                        DataModel = c.String(),
                        Changes = c.String(),
                        ValueBefore = c.String(),
                        ValueAfter = c.String(),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CandidateCodes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CodeVerify = c.String(),
                        Candidates_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidates", t => t.Candidates_Id)
                .Index(t => t.Candidates_Id);
            
            CreateTable(
                "dbo.DateTimeInterviews",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        InterviewTime = c.String(),
                        Date = c.String(),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        Candidate_Id = c.Long(),
                        Interview_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id)
                .ForeignKey("dbo.Interviews", t => t.Interview_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.Interview_Id);
            
            CreateTable(
                "dbo.EFormAuditTrails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DateTimeStamp = c.DateTime(nullable: false),
                        IpAddress = c.String(),
                        Forms_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Forms", t => t.Forms_Id)
                .Index(t => t.Forms_Id);
            
            CreateTable(
                "dbo.InterviewComments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Comment = c.String(),
                        Score = c.Double(nullable: false),
                        FinalState = c.Int(nullable: false),
                        Candidate_Id = c.Long(),
                        Interview_Id = c.Long(),
                        Interviewer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id)
                .ForeignKey("dbo.Interviews", t => t.Interview_Id)
                .ForeignKey("dbo.Users", t => t.Interviewer_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.Interview_Id)
                .Index(t => t.Interviewer_Id);
            
            CreateTable(
                "dbo.LoginAuditTrails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Username = c.String(),
                        DateTimeStamp = c.DateTime(nullable: false),
                        IpAddress = c.String(),
                        Status = c.Int(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Mails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TemplateName = c.String(),
                        Subject = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MailConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServerAddress = c.String(),
                        Port = c.Int(nullable: false),
                        Timeout = c.Int(nullable: false),
                        UseSSL = c.Boolean(nullable: false),
                        EmailSend = c.String(),
                        EmailCC = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        InterviewInvited_Id = c.Int(),
                        JobOffer_Id = c.Int(),
                        ReceivedProfile_Id = c.Int(),
                        Reject_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Mails", t => t.InterviewInvited_Id)
                .ForeignKey("dbo.Mails", t => t.JobOffer_Id)
                .ForeignKey("dbo.Mails", t => t.ReceivedProfile_Id)
                .ForeignKey("dbo.Mails", t => t.Reject_Id)
                .Index(t => t.InterviewInvited_Id)
                .Index(t => t.JobOffer_Id)
                .Index(t => t.ReceivedProfile_Id)
                .Index(t => t.Reject_Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Brief = c.String(),
                        Description = c.String(),
                        TimeExecute = c.DateTime(nullable: false),
                        ProjectId = c.Int(),
                        InterviewId = c.Int(),
                        AddressId = c.Int(),
                        CandidateId = c.Int(),
                        CityId = c.Int(),
                        CustomerId = c.Int(),
                        FormId = c.Int(),
                        GroupCustomerId = c.Int(),
                        GroupProjectId = c.Int(),
                        ProjectMemberId = c.Int(),
                        RegionId = c.Int(),
                        RoleId = c.Int(),
                        StaffId = c.Int(),
                        StoreId = c.Int(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CityCandidates",
                c => new
                    {
                        City_Id = c.Int(nullable: false),
                        Candidate_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.City_Id, t.Candidate_Id })
                .ForeignKey("dbo.Cities", t => t.City_Id, cascadeDelete: true)
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id, cascadeDelete: true)
                .Index(t => t.City_Id)
                .Index(t => t.Candidate_Id);
            
            CreateTable(
                "dbo.FormCities",
                c => new
                    {
                        Form_Id = c.Int(nullable: false),
                        City_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Form_Id, t.City_Id })
                .ForeignKey("dbo.Forms", t => t.Form_Id, cascadeDelete: true)
                .ForeignKey("dbo.Cities", t => t.City_Id, cascadeDelete: true)
                .Index(t => t.Form_Id)
                .Index(t => t.City_Id);
            
            CreateTable(
                "dbo.InterviewCandidates",
                c => new
                    {
                        Interview_Id = c.Long(nullable: false),
                        Candidate_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Interview_Id, t.Candidate_Id })
                .ForeignKey("dbo.Interviews", t => t.Interview_Id, cascadeDelete: true)
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id, cascadeDelete: true)
                .Index(t => t.Interview_Id)
                .Index(t => t.Candidate_Id);
            
            CreateTable(
                "dbo.InterviewUser",
                c => new
                    {
                        Interview_Id = c.Long(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Interview_Id, t.User_Id })
                .ForeignKey("dbo.Interviews", t => t.Interview_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Interview_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.PermissionRoles",
                c => new
                    {
                        Permission_Id = c.Int(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Permission_Id, t.Role_Id })
                .ForeignKey("dbo.Permissions", t => t.Permission_Id, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.Permission_Id)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.User_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.StoreCandidates",
                c => new
                    {
                        Store_Id = c.Long(nullable: false),
                        Candidate_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Store_Id, t.Candidate_Id })
                .ForeignKey("dbo.Stores", t => t.Store_Id, cascadeDelete: true)
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id, cascadeDelete: true)
                .Index(t => t.Store_Id)
                .Index(t => t.Candidate_Id);
            
            CreateTable(
                "dbo.StoreForms",
                c => new
                    {
                        Store_Id = c.Long(nullable: false),
                        Form_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Store_Id, t.Form_Id })
                .ForeignKey("dbo.Stores", t => t.Store_Id, cascadeDelete: true)
                .ForeignKey("dbo.Forms", t => t.Form_Id, cascadeDelete: true)
                .Index(t => t.Store_Id)
                .Index(t => t.Form_Id);
            
            CreateTable(
                "dbo.StoreProjects",
                c => new
                    {
                        Store_Id = c.Long(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Store_Id, t.Project_Id })
                .ForeignKey("dbo.Stores", t => t.Store_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Store_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.ProjectUser",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.User_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.VacancyForms",
                c => new
                    {
                        Vacancy_Id = c.Int(nullable: false),
                        Form_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vacancy_Id, t.Form_Id })
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_Id, cascadeDelete: true)
                .ForeignKey("dbo.Forms", t => t.Form_Id, cascadeDelete: true)
                .Index(t => t.Vacancy_Id)
                .Index(t => t.Form_Id);
            
            CreateTable(
                "dbo.VacancyProjects",
                c => new
                    {
                        Vacancy_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vacancy_Id, t.Project_Id })
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Vacancy_Id)
                .Index(t => t.Project_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MailConfigs", "Reject_Id", "dbo.Mails");
            DropForeignKey("dbo.MailConfigs", "ReceivedProfile_Id", "dbo.Mails");
            DropForeignKey("dbo.MailConfigs", "JobOffer_Id", "dbo.Mails");
            DropForeignKey("dbo.MailConfigs", "InterviewInvited_Id", "dbo.Mails");
            DropForeignKey("dbo.InterviewComments", "Interviewer_Id", "dbo.Users");
            DropForeignKey("dbo.InterviewComments", "Interview_Id", "dbo.Interviews");
            DropForeignKey("dbo.InterviewComments", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.EFormAuditTrails", "Forms_Id", "dbo.Forms");
            DropForeignKey("dbo.DateTimeInterviews", "Interview_Id", "dbo.Interviews");
            DropForeignKey("dbo.DateTimeInterviews", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.CandidateCodes", "Candidates_Id", "dbo.Candidates");
            DropForeignKey("dbo.ActionAuditTrails", "Candidates_Id", "dbo.Candidates");
            DropForeignKey("dbo.Candidates", "Region_Id", "dbo.Regions");
            DropForeignKey("dbo.Candidates", "CommentBy_Id", "dbo.Users");
            DropForeignKey("dbo.Forms", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "HRBP_Id", "dbo.Users");
            DropForeignKey("dbo.Projects", "RFM_Id", "dbo.Users");
            DropForeignKey("dbo.Projects", "Recruiter_Id", "dbo.Users");
            DropForeignKey("dbo.VacancyProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.VacancyProjects", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.VacancyForms", "Form_Id", "dbo.Forms");
            DropForeignKey("dbo.VacancyForms", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.Candidates", "Position_Id", "dbo.Vacancies");
            DropForeignKey("dbo.Projects", "TeamLeader_Id", "dbo.Users");
            DropForeignKey("dbo.Projects", "Region_Id", "dbo.Regions");
            DropForeignKey("dbo.Cities", "Region_Id", "dbo.Regions");
            DropForeignKey("dbo.ProjectUser", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ProjectUser", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "LastEditedBy_Id", "dbo.Users");
            DropForeignKey("dbo.Projects", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.StoreProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.StoreProjects", "Store_Id", "dbo.Stores");
            DropForeignKey("dbo.StoreForms", "Form_Id", "dbo.Forms");
            DropForeignKey("dbo.StoreForms", "Store_Id", "dbo.Stores");
            DropForeignKey("dbo.Stores", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Stores", "City_Id", "dbo.Cities");
            DropForeignKey("dbo.StoreCandidates", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.StoreCandidates", "Store_Id", "dbo.Stores");
            DropForeignKey("dbo.Projects", "CreatedBy_Id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.PermissionRoles", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.PermissionRoles", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.Interviews", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Interviews", "LastEditedBy_Id", "dbo.Users");
            DropForeignKey("dbo.InterviewUser", "User_Id", "dbo.Users");
            DropForeignKey("dbo.InterviewUser", "Interview_Id", "dbo.Interviews");
            DropForeignKey("dbo.Interviews", "CreatedBy_Id", "dbo.Users");
            DropForeignKey("dbo.InterviewCandidates", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.InterviewCandidates", "Interview_Id", "dbo.Interviews");
            DropForeignKey("dbo.Projects", "City_Id", "dbo.Cities");
            DropForeignKey("dbo.FormCities", "City_Id", "dbo.Cities");
            DropForeignKey("dbo.FormCities", "Form_Id", "dbo.Forms");
            DropForeignKey("dbo.Candidates", "Form_Id", "dbo.Forms");
            DropForeignKey("dbo.CityCandidates", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.CityCandidates", "City_Id", "dbo.Cities");
            DropIndex("dbo.VacancyProjects", new[] { "Project_Id" });
            DropIndex("dbo.VacancyProjects", new[] { "Vacancy_Id" });
            DropIndex("dbo.VacancyForms", new[] { "Form_Id" });
            DropIndex("dbo.VacancyForms", new[] { "Vacancy_Id" });
            DropIndex("dbo.ProjectUser", new[] { "User_Id" });
            DropIndex("dbo.ProjectUser", new[] { "Project_Id" });
            DropIndex("dbo.StoreProjects", new[] { "Project_Id" });
            DropIndex("dbo.StoreProjects", new[] { "Store_Id" });
            DropIndex("dbo.StoreForms", new[] { "Form_Id" });
            DropIndex("dbo.StoreForms", new[] { "Store_Id" });
            DropIndex("dbo.StoreCandidates", new[] { "Candidate_Id" });
            DropIndex("dbo.StoreCandidates", new[] { "Store_Id" });
            DropIndex("dbo.RoleUsers", new[] { "User_Id" });
            DropIndex("dbo.RoleUsers", new[] { "Role_Id" });
            DropIndex("dbo.PermissionRoles", new[] { "Role_Id" });
            DropIndex("dbo.PermissionRoles", new[] { "Permission_Id" });
            DropIndex("dbo.InterviewUser", new[] { "User_Id" });
            DropIndex("dbo.InterviewUser", new[] { "Interview_Id" });
            DropIndex("dbo.InterviewCandidates", new[] { "Candidate_Id" });
            DropIndex("dbo.InterviewCandidates", new[] { "Interview_Id" });
            DropIndex("dbo.FormCities", new[] { "City_Id" });
            DropIndex("dbo.FormCities", new[] { "Form_Id" });
            DropIndex("dbo.CityCandidates", new[] { "Candidate_Id" });
            DropIndex("dbo.CityCandidates", new[] { "City_Id" });
            DropIndex("dbo.MailConfigs", new[] { "Reject_Id" });
            DropIndex("dbo.MailConfigs", new[] { "ReceivedProfile_Id" });
            DropIndex("dbo.MailConfigs", new[] { "JobOffer_Id" });
            DropIndex("dbo.MailConfigs", new[] { "InterviewInvited_Id" });
            DropIndex("dbo.InterviewComments", new[] { "Interviewer_Id" });
            DropIndex("dbo.InterviewComments", new[] { "Interview_Id" });
            DropIndex("dbo.InterviewComments", new[] { "Candidate_Id" });
            DropIndex("dbo.EFormAuditTrails", new[] { "Forms_Id" });
            DropIndex("dbo.DateTimeInterviews", new[] { "Interview_Id" });
            DropIndex("dbo.DateTimeInterviews", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidateCodes", new[] { "Candidates_Id" });
            DropIndex("dbo.Stores", new[] { "Customer_Id" });
            DropIndex("dbo.Stores", new[] { "City_Id" });
            DropIndex("dbo.Permissions", new[] { "PermissionCode" });
            DropIndex("dbo.Interviews", new[] { "Project_Id" });
            DropIndex("dbo.Interviews", new[] { "LastEditedBy_Id" });
            DropIndex("dbo.Interviews", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.Projects", new[] { "HRBP_Id" });
            DropIndex("dbo.Projects", new[] { "RFM_Id" });
            DropIndex("dbo.Projects", new[] { "Recruiter_Id" });
            DropIndex("dbo.Projects", new[] { "TeamLeader_Id" });
            DropIndex("dbo.Projects", new[] { "Region_Id" });
            DropIndex("dbo.Projects", new[] { "LastEditedBy_Id" });
            DropIndex("dbo.Projects", new[] { "Customer_Id" });
            DropIndex("dbo.Projects", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Projects", new[] { "City_Id" });
            DropIndex("dbo.Forms", new[] { "Project_Id" });
            DropIndex("dbo.Cities", new[] { "Region_Id" });
            DropIndex("dbo.Candidates", new[] { "Region_Id" });
            DropIndex("dbo.Candidates", new[] { "CommentBy_Id" });
            DropIndex("dbo.Candidates", new[] { "Position_Id" });
            DropIndex("dbo.Candidates", new[] { "Form_Id" });
            DropIndex("dbo.ActionAuditTrails", new[] { "Candidates_Id" });
            DropTable("dbo.VacancyProjects");
            DropTable("dbo.VacancyForms");
            DropTable("dbo.ProjectUser");
            DropTable("dbo.StoreProjects");
            DropTable("dbo.StoreForms");
            DropTable("dbo.StoreCandidates");
            DropTable("dbo.RoleUsers");
            DropTable("dbo.PermissionRoles");
            DropTable("dbo.InterviewUser");
            DropTable("dbo.InterviewCandidates");
            DropTable("dbo.FormCities");
            DropTable("dbo.CityCandidates");
            DropTable("dbo.Notifications");
            DropTable("dbo.MailConfigs");
            DropTable("dbo.Mails");
            DropTable("dbo.LoginAuditTrails");
            DropTable("dbo.InterviewComments");
            DropTable("dbo.EFormAuditTrails");
            DropTable("dbo.DateTimeInterviews");
            DropTable("dbo.CandidateCodes");
            DropTable("dbo.AuditTrails");
            DropTable("dbo.Vacancies");
            DropTable("dbo.Regions");
            DropTable("dbo.Stores");
            DropTable("dbo.Customers");
            DropTable("dbo.Permissions");
            DropTable("dbo.Roles");
            DropTable("dbo.Interviews");
            DropTable("dbo.Users");
            DropTable("dbo.Projects");
            DropTable("dbo.Forms");
            DropTable("dbo.Cities");
            DropTable("dbo.Candidates");
            DropTable("dbo.ActionAuditTrails");
        }
    }
}
