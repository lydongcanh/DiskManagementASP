namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionAuditTrails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Username = c.String(),
                        DateTimeStamp = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
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
                        CandidateSource = c.Int(nullable: false),
                        SourceDetail = c.String(),
                        FullName = c.String(),
                        DOB = c.DateTime(precision: 7, storeType: "datetime2"),
                        Sex = c.Int(nullable: false),
                        Height = c.Single(nullable: false),
                        Weight = c.Single(nullable: false),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        EducationLevel = c.Int(nullable: false),
                        Facebook = c.String(),
                        Zalo = c.String(),
                        District = c.String(),
                        ExpectationPlaces = c.String(),
                        YearExperiences = c.Int(nullable: false),
                        Experiences = c.String(),
                        StartedTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        InternalSourceName = c.String(),
                        InternalSourceCode = c.String(),
                        Photos = c.String(),
                        CV = c.String(),
                        Agreement = c.Int(nullable: false),
                        Q1 = c.String(),
                        Q2 = c.String(),
                        Q3 = c.String(),
                        Q4 = c.String(),
                        Q5 = c.String(),
                        Q6 = c.String(),
                        Q7 = c.String(),
                        OtherSource = c.Int(nullable: false),
                        Approval = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        Comment = c.String(),
                        FinalComment = c.String(),
                        DateFinalComment = c.DateTime(precision: 7, storeType: "datetime2"),
                        RoundPassed = c.Int(nullable: false),
                        ScreeningScore = c.Int(nullable: false),
                        ProfileScore = c.Int(nullable: false),
                        ContractDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        SubmissionDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IP = c.String(),
                        ExpFailed = c.Int(nullable: false),
                        FormFailed = c.Int(nullable: false),
                        MailFailed = c.Int(nullable: false),
                        Measure_R1 = c.Int(nullable: false),
                        Comment_R1 = c.Int(nullable: false),
                        FailedReason_R1 = c.Int(nullable: false),
                        Result_R1 = c.Int(nullable: false),
                        Measure_R2 = c.Int(nullable: false),
                        Comment_R2 = c.Int(nullable: false),
                        FailedReason_R2 = c.Int(nullable: false),
                        Result_R2 = c.Int(nullable: false),
                        Measure_R3 = c.Int(nullable: false),
                        Comment_R3 = c.Int(nullable: false),
                        FailedReason_R3 = c.Int(nullable: false),
                        Result_R3 = c.Int(nullable: false),
                        FirstUsed = c.DateTime(precision: 7, storeType: "datetime2"),
                        R1_Eval1 = c.Int(nullable: false),
                        R1_Eval2 = c.Int(nullable: false),
                        R1_Eval3 = c.Int(nullable: false),
                        R1_Eval4 = c.Int(nullable: false),
                        R1_Eval5 = c.Int(nullable: false),
                        R1_Eval6 = c.Int(nullable: false),
                        R1_Eval7 = c.Int(nullable: false),
                        R1_Eval8 = c.Int(nullable: false),
                        R1_Eval9 = c.Int(nullable: false),
                        R1_Eval10 = c.Int(nullable: false),
                        R2_Eval1 = c.Int(nullable: false),
                        R2_Eval2 = c.Int(nullable: false),
                        R2_Eval3 = c.Int(nullable: false),
                        R2_Eval4 = c.Int(nullable: false),
                        R2_Eval5 = c.Int(nullable: false),
                        R2_Eval6 = c.Int(nullable: false),
                        R2_Eval7 = c.Int(nullable: false),
                        R2_Eval8 = c.Int(nullable: false),
                        R2_Eval9 = c.Int(nullable: false),
                        R2_Eval10 = c.Int(nullable: false),
                        R3_Eval1 = c.Int(nullable: false),
                        R3_Eval2 = c.Int(nullable: false),
                        R3_Eval3 = c.Int(nullable: false),
                        R3_Eval4 = c.Int(nullable: false),
                        R3_Eval5 = c.Int(nullable: false),
                        R3_Eval6 = c.Int(nullable: false),
                        R3_Eval7 = c.Int(nullable: false),
                        R3_Eval8 = c.Int(nullable: false),
                        R3_Eval9 = c.Int(nullable: false),
                        R3_Eval10 = c.Int(nullable: false),
                        ResignedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        RejectDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        RejectReason = c.Int(nullable: false),
                        ResignReason = c.Int(nullable: false),
                        Round1_Date = c.DateTime(precision: 7, storeType: "datetime2"),
                        Round2_Date = c.DateTime(precision: 7, storeType: "datetime2"),
                        Round3_Date = c.DateTime(precision: 7, storeType: "datetime2"),
                        CityRegion_Id = c.Int(),
                        Position_Id = c.Int(),
                        Form_Id = c.Int(),
                        ProjectNew_Id = c.Int(),
                        Store_Id = c.Long(),
                        ProjectReplace_Id = c.Int(),
                        CommentBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CityRegions", t => t.CityRegion_Id)
                .ForeignKey("dbo.Vacancies", t => t.Position_Id)
                .ForeignKey("dbo.Forms", t => t.Form_Id)
                .ForeignKey("dbo.EProjectNews", t => t.ProjectNew_Id)
                .ForeignKey("dbo.Stores", t => t.Store_Id)
                .ForeignKey("dbo.EProjectReplaces", t => t.ProjectReplace_Id)
                .ForeignKey("dbo.Users", t => t.CommentBy_Id)
                .Index(t => t.CityRegion_Id)
                .Index(t => t.Position_Id)
                .Index(t => t.Form_Id)
                .Index(t => t.ProjectNew_Id)
                .Index(t => t.Store_Id)
                .Index(t => t.ProjectReplace_Id)
                .Index(t => t.CommentBy_Id);
            
            CreateTable(
                "dbo.CityRegions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        City_Id = c.Int(),
                        Region_Id = c.Int(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.City_Id)
                .ForeignKey("dbo.Regions", t => t.Region_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.City_Id)
                .Index(t => t.Region_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityName = c.String(),
                        IsProvince = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        ProjectManager = c.String(),
                        BussinessDescription = c.String(),
                        Logo = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Information = c.String(),
                        RequirementStatus = c.Int(nullable: false),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupVacancies", t => t.Group_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.GroupVacancies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vacancies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        Category = c.String(),
                        Quantity = c.Int(nullable: false),
                        JobDescription = c.String(),
                        JobRequirement = c.String(),
                        SalaryMin = c.Int(nullable: false),
                        SalaryMax = c.Int(nullable: false),
                        Eval1 = c.String(),
                        Eval2 = c.String(),
                        Eval3 = c.String(),
                        Eval4 = c.String(),
                        Eval5 = c.String(),
                        Eval6 = c.String(),
                        Eval7 = c.String(),
                        Eval8 = c.String(),
                        Eval9 = c.String(),
                        Eval10 = c.String(),
                        Level = c.Int(nullable: false),
                        Customer_Id = c.Int(),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.GroupVacancies", t => t.Group_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.Forms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormName = c.String(),
                        ExpiryDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Banner = c.String(),
                        Vacancy = c.Int(nullable: false),
                        CandidateSource = c.Int(nullable: false),
                        SourceDetail = c.Int(nullable: false),
                        FullName = c.Int(nullable: false),
                        DOB = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        PhoneNumber = c.Int(nullable: false),
                        Email = c.Int(nullable: false),
                        Address = c.Int(nullable: false),
                        EducationLevel = c.Int(nullable: false),
                        Facebook = c.Int(nullable: false),
                        Zalo = c.Int(nullable: false),
                        City = c.Int(nullable: false),
                        District = c.Int(nullable: false),
                        ExpectationPlaces = c.Int(nullable: false),
                        YearExperiences = c.Int(nullable: false),
                        Experiences = c.Int(nullable: false),
                        StartedTime = c.Int(nullable: false),
                        InternalSourceName = c.Int(nullable: false),
                        InternalSourceCode = c.Int(nullable: false),
                        Photos = c.Int(nullable: false),
                        CV = c.Int(nullable: false),
                        Agreement = c.Int(nullable: false),
                        Q1 = c.Int(nullable: false),
                        Q2 = c.Int(nullable: false),
                        Q3 = c.Int(nullable: false),
                        Q4 = c.Int(nullable: false),
                        Q5 = c.Int(nullable: false),
                        Q6 = c.Int(nullable: false),
                        Q7 = c.Int(nullable: false),
                        Q1_TEXT = c.String(),
                        Q2_TEXT = c.String(),
                        Q3_TEXT = c.String(),
                        Q4_TEXT = c.String(),
                        Q5_TEXT = c.String(),
                        Q6_TEXT = c.String(),
                        Q7_TEXT = c.String(),
                        Ads = c.Int(nullable: false),
                        FormLink = c.String(),
                        CreationDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Store_Id = c.Long(),
                        Project_Id = c.Int(),
                        Vacancy_Id = c.Int(),
                        CityRegion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stores", t => t.Store_Id)
                .ForeignKey("dbo.EProjects", t => t.Project_Id)
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_Id)
                .ForeignKey("dbo.CityRegions", t => t.CityRegion_Id)
                .Index(t => t.Store_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.Vacancy_Id)
                .Index(t => t.CityRegion_Id);
            
            CreateTable(
                "dbo.EProjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatusActive = c.Int(nullable: false),
                        ProjectName = c.String(),
                        StartingDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndingDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Hotline = c.String(),
                        TypeProject = c.Int(nullable: false),
                        Store_Id = c.Long(),
                        CreatedBy_Id = c.Int(),
                        Customer_Id = c.Int(),
                        LastEditedBy_Id = c.Int(),
                        MailConfig_Id = c.Int(),
                        Region_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stores", t => t.Store_Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Users", t => t.LastEditedBy_Id)
                .ForeignKey("dbo.MailConfigs", t => t.MailConfig_Id)
                .ForeignKey("dbo.Regions", t => t.Region_Id)
                .Index(t => t.Store_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.LastEditedBy_Id)
                .Index(t => t.MailConfig_Id)
                .Index(t => t.Region_Id);
            
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
                        StartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        RangeTime = c.String(),
                        PriorityInterview = c.Int(nullable: false),
                        EachTime = c.Int(nullable: false),
                        StartTime = c.String(),
                        StopTime = c.String(),
                        StartBreakTime = c.String(),
                        EndBreakTime = c.String(),
                        Round = c.Int(nullable: false),
                        Location = c.String(),
                        Interviewer = c.String(),
                        InterviewerPhone = c.String(),
                        IsSendMail = c.Boolean(nullable: false),
                        SendDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy_Id = c.Int(),
                        LastEditedBy_Id = c.Int(),
                        Project_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Users", t => t.LastEditedBy_Id)
                .ForeignKey("dbo.EProjects", t => t.Project_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.LastEditedBy_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.EProjectNews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PG_SUB_NAME = c.String(),
                        Manager = c.String(),
                        RegionalManager = c.String(),
                        NumberOfRequested = c.Int(nullable: false),
                        WeekRequested = c.Int(nullable: false),
                        RequestedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        CompletedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        CompletedWeek = c.Int(),
                        YearRequested = c.Int(nullable: false),
                        StartingDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        EndingDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        LeadTimeRange = c.Int(nullable: false),
                        LeadTime = c.Int(nullable: false),
                        HRBP_Id = c.Int(),
                        Project_Id = c.Int(),
                        Store_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.HRBP_Id)
                .ForeignKey("dbo.EProjects", t => t.Project_Id)
                .ForeignKey("dbo.Stores", t => t.Store_Id)
                .Index(t => t.HRBP_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.Store_Id);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        District = c.String(),
                        Channel = c.String(),
                        StatusActive = c.Int(nullable: false),
                        CityRegion_Id = c.Int(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CityRegions", t => t.CityRegion_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.CityRegion_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.EProjectReplaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReplacementCode = c.String(),
                        Manager = c.String(),
                        RegionalManager = c.String(),
                        TypeReplacement = c.Int(nullable: false),
                        SubmitedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LeavingDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        InformedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        NumberOfRequested = c.Int(nullable: false),
                        WeekRequested = c.Int(nullable: false),
                        RequestedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        CompletedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        CompletedWeek = c.Int(),
                        YearRequested = c.Int(nullable: false),
                        TargetOfWeek = c.Int(nullable: false),
                        StartingDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        EndingDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        LeadTimeRange = c.Int(nullable: false),
                        LeadTime = c.Int(nullable: false),
                        HRBP_Id = c.Int(),
                        Project_Id = c.Int(),
                        Store_Id = c.Long(),
                        Vacancy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.HRBP_Id)
                .ForeignKey("dbo.EProjects", t => t.Project_Id)
                .ForeignKey("dbo.Stores", t => t.Store_Id)
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_Id)
                .Index(t => t.HRBP_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.Store_Id)
                .Index(t => t.Vacancy_Id);
            
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
                        Default = c.Boolean(nullable: false),
                        InterviewInvited_Id = c.Int(),
                        JobOffer_Id = c.Int(),
                        ReceivedProfile_Id = c.Int(),
                        Reject_Id = c.Int(),
                        WaitRecruited_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Mails", t => t.InterviewInvited_Id)
                .ForeignKey("dbo.Mails", t => t.JobOffer_Id)
                .ForeignKey("dbo.Mails", t => t.ReceivedProfile_Id)
                .ForeignKey("dbo.Mails", t => t.Reject_Id)
                .ForeignKey("dbo.Mails", t => t.WaitRecruited_Id)
                .Index(t => t.InterviewInvited_Id)
                .Index(t => t.JobOffer_Id)
                .Index(t => t.ReceivedProfile_Id)
                .Index(t => t.Reject_Id)
                .Index(t => t.WaitRecruited_Id);
            
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
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegionName = c.String(),
                        RegionCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuditTrails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        KeyFieldID = c.Long(nullable: false),
                        AuditActionType = c.Int(nullable: false),
                        DateTimeStamp = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
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
                        DateTimeStamp = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
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
                        DateTimeStamp = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IpAddress = c.String(),
                        Status = c.Int(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Brief = c.String(),
                        Description = c.String(),
                        TimeExecute = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
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
                "dbo.VacancyCityRegions",
                c => new
                    {
                        Vacancy_Id = c.Int(nullable: false),
                        CityRegion_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vacancy_Id, t.CityRegion_Id })
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_Id, cascadeDelete: true)
                .ForeignKey("dbo.CityRegions", t => t.CityRegion_Id, cascadeDelete: true)
                .Index(t => t.Vacancy_Id)
                .Index(t => t.CityRegion_Id);
            
            CreateTable(
                "dbo.EProjectCityRegions",
                c => new
                    {
                        EProject_Id = c.Int(nullable: false),
                        CityRegion_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EProject_Id, t.CityRegion_Id })
                .ForeignKey("dbo.EProjects", t => t.EProject_Id, cascadeDelete: true)
                .ForeignKey("dbo.CityRegions", t => t.CityRegion_Id, cascadeDelete: true)
                .Index(t => t.EProject_Id)
                .Index(t => t.CityRegion_Id);
            
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
                "dbo.EProjectUser",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.User_Id })
                .ForeignKey("dbo.EProjects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.EProjectVacancies",
                c => new
                    {
                        EProject_Id = c.Int(nullable: false),
                        Vacancy_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EProject_Id, t.Vacancy_Id })
                .ForeignKey("dbo.EProjects", t => t.EProject_Id, cascadeDelete: true)
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_Id, cascadeDelete: true)
                .Index(t => t.EProject_Id)
                .Index(t => t.Vacancy_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterviewComments", "Interviewer_Id", "dbo.Users");
            DropForeignKey("dbo.InterviewComments", "Interview_Id", "dbo.Interviews");
            DropForeignKey("dbo.InterviewComments", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.EFormAuditTrails", "Forms_Id", "dbo.Forms");
            DropForeignKey("dbo.DateTimeInterviews", "Interview_Id", "dbo.Interviews");
            DropForeignKey("dbo.DateTimeInterviews", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.CandidateCodes", "Candidates_Id", "dbo.Candidates");
            DropForeignKey("dbo.ActionAuditTrails", "Candidates_Id", "dbo.Candidates");
            DropForeignKey("dbo.Candidates", "CommentBy_Id", "dbo.Users");
            DropForeignKey("dbo.Forms", "CityRegion_Id", "dbo.CityRegions");
            DropForeignKey("dbo.CityRegions", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Customers", "Group_Id", "dbo.GroupVacancies");
            DropForeignKey("dbo.Vacancies", "Group_Id", "dbo.GroupVacancies");
            DropForeignKey("dbo.Forms", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.Forms", "Project_Id", "dbo.EProjects");
            DropForeignKey("dbo.EProjectVacancies", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.EProjectVacancies", "EProject_Id", "dbo.EProjects");
            DropForeignKey("dbo.EProjects", "Region_Id", "dbo.Regions");
            DropForeignKey("dbo.CityRegions", "Region_Id", "dbo.Regions");
            DropForeignKey("dbo.EProjectUser", "User_Id", "dbo.Users");
            DropForeignKey("dbo.EProjectUser", "Project_Id", "dbo.EProjects");
            DropForeignKey("dbo.EProjects", "MailConfig_Id", "dbo.MailConfigs");
            DropForeignKey("dbo.MailConfigs", "WaitRecruited_Id", "dbo.Mails");
            DropForeignKey("dbo.MailConfigs", "Reject_Id", "dbo.Mails");
            DropForeignKey("dbo.MailConfigs", "ReceivedProfile_Id", "dbo.Mails");
            DropForeignKey("dbo.MailConfigs", "JobOffer_Id", "dbo.Mails");
            DropForeignKey("dbo.MailConfigs", "InterviewInvited_Id", "dbo.Mails");
            DropForeignKey("dbo.EProjects", "LastEditedBy_Id", "dbo.Users");
            DropForeignKey("dbo.EProjects", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.EProjects", "CreatedBy_Id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.PermissionRoles", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.PermissionRoles", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.EProjectReplaces", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.EProjectReplaces", "Store_Id", "dbo.Stores");
            DropForeignKey("dbo.EProjectReplaces", "Project_Id", "dbo.EProjects");
            DropForeignKey("dbo.EProjectReplaces", "HRBP_Id", "dbo.Users");
            DropForeignKey("dbo.Candidates", "ProjectReplace_Id", "dbo.EProjectReplaces");
            DropForeignKey("dbo.EProjectNews", "Store_Id", "dbo.Stores");
            DropForeignKey("dbo.EProjects", "Store_Id", "dbo.Stores");
            DropForeignKey("dbo.Forms", "Store_Id", "dbo.Stores");
            DropForeignKey("dbo.Stores", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Stores", "CityRegion_Id", "dbo.CityRegions");
            DropForeignKey("dbo.Candidates", "Store_Id", "dbo.Stores");
            DropForeignKey("dbo.EProjectNews", "Project_Id", "dbo.EProjects");
            DropForeignKey("dbo.EProjectNews", "HRBP_Id", "dbo.Users");
            DropForeignKey("dbo.Candidates", "ProjectNew_Id", "dbo.EProjectNews");
            DropForeignKey("dbo.Interviews", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Interviews", "Project_Id", "dbo.EProjects");
            DropForeignKey("dbo.Interviews", "LastEditedBy_Id", "dbo.Users");
            DropForeignKey("dbo.Interviews", "CreatedBy_Id", "dbo.Users");
            DropForeignKey("dbo.InterviewCandidates", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.InterviewCandidates", "Interview_Id", "dbo.Interviews");
            DropForeignKey("dbo.EProjectCityRegions", "CityRegion_Id", "dbo.CityRegions");
            DropForeignKey("dbo.EProjectCityRegions", "EProject_Id", "dbo.EProjects");
            DropForeignKey("dbo.Candidates", "Form_Id", "dbo.Forms");
            DropForeignKey("dbo.Vacancies", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.VacancyCityRegions", "CityRegion_Id", "dbo.CityRegions");
            DropForeignKey("dbo.VacancyCityRegions", "Vacancy_Id", "dbo.Vacancies");
            DropForeignKey("dbo.Candidates", "Position_Id", "dbo.Vacancies");
            DropForeignKey("dbo.CityRegions", "City_Id", "dbo.Cities");
            DropForeignKey("dbo.Candidates", "CityRegion_Id", "dbo.CityRegions");
            DropIndex("dbo.EProjectVacancies", new[] { "Vacancy_Id" });
            DropIndex("dbo.EProjectVacancies", new[] { "EProject_Id" });
            DropIndex("dbo.EProjectUser", new[] { "User_Id" });
            DropIndex("dbo.EProjectUser", new[] { "Project_Id" });
            DropIndex("dbo.RoleUsers", new[] { "User_Id" });
            DropIndex("dbo.RoleUsers", new[] { "Role_Id" });
            DropIndex("dbo.PermissionRoles", new[] { "Role_Id" });
            DropIndex("dbo.PermissionRoles", new[] { "Permission_Id" });
            DropIndex("dbo.InterviewCandidates", new[] { "Candidate_Id" });
            DropIndex("dbo.InterviewCandidates", new[] { "Interview_Id" });
            DropIndex("dbo.EProjectCityRegions", new[] { "CityRegion_Id" });
            DropIndex("dbo.EProjectCityRegions", new[] { "EProject_Id" });
            DropIndex("dbo.VacancyCityRegions", new[] { "CityRegion_Id" });
            DropIndex("dbo.VacancyCityRegions", new[] { "Vacancy_Id" });
            DropIndex("dbo.InterviewComments", new[] { "Interviewer_Id" });
            DropIndex("dbo.InterviewComments", new[] { "Interview_Id" });
            DropIndex("dbo.InterviewComments", new[] { "Candidate_Id" });
            DropIndex("dbo.EFormAuditTrails", new[] { "Forms_Id" });
            DropIndex("dbo.DateTimeInterviews", new[] { "Interview_Id" });
            DropIndex("dbo.DateTimeInterviews", new[] { "Candidate_Id" });
            DropIndex("dbo.CandidateCodes", new[] { "Candidates_Id" });
            DropIndex("dbo.MailConfigs", new[] { "WaitRecruited_Id" });
            DropIndex("dbo.MailConfigs", new[] { "Reject_Id" });
            DropIndex("dbo.MailConfigs", new[] { "ReceivedProfile_Id" });
            DropIndex("dbo.MailConfigs", new[] { "JobOffer_Id" });
            DropIndex("dbo.MailConfigs", new[] { "InterviewInvited_Id" });
            DropIndex("dbo.Permissions", new[] { "PermissionCode" });
            DropIndex("dbo.EProjectReplaces", new[] { "Vacancy_Id" });
            DropIndex("dbo.EProjectReplaces", new[] { "Store_Id" });
            DropIndex("dbo.EProjectReplaces", new[] { "Project_Id" });
            DropIndex("dbo.EProjectReplaces", new[] { "HRBP_Id" });
            DropIndex("dbo.Stores", new[] { "Customer_Id" });
            DropIndex("dbo.Stores", new[] { "CityRegion_Id" });
            DropIndex("dbo.EProjectNews", new[] { "Store_Id" });
            DropIndex("dbo.EProjectNews", new[] { "Project_Id" });
            DropIndex("dbo.EProjectNews", new[] { "HRBP_Id" });
            DropIndex("dbo.Interviews", new[] { "User_Id" });
            DropIndex("dbo.Interviews", new[] { "Project_Id" });
            DropIndex("dbo.Interviews", new[] { "LastEditedBy_Id" });
            DropIndex("dbo.Interviews", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.EProjects", new[] { "Region_Id" });
            DropIndex("dbo.EProjects", new[] { "MailConfig_Id" });
            DropIndex("dbo.EProjects", new[] { "LastEditedBy_Id" });
            DropIndex("dbo.EProjects", new[] { "Customer_Id" });
            DropIndex("dbo.EProjects", new[] { "CreatedBy_Id" });
            DropIndex("dbo.EProjects", new[] { "Store_Id" });
            DropIndex("dbo.Forms", new[] { "CityRegion_Id" });
            DropIndex("dbo.Forms", new[] { "Vacancy_Id" });
            DropIndex("dbo.Forms", new[] { "Project_Id" });
            DropIndex("dbo.Forms", new[] { "Store_Id" });
            DropIndex("dbo.Vacancies", new[] { "Group_Id" });
            DropIndex("dbo.Vacancies", new[] { "Customer_Id" });
            DropIndex("dbo.Customers", new[] { "Group_Id" });
            DropIndex("dbo.CityRegions", new[] { "Customer_Id" });
            DropIndex("dbo.CityRegions", new[] { "Region_Id" });
            DropIndex("dbo.CityRegions", new[] { "City_Id" });
            DropIndex("dbo.Candidates", new[] { "CommentBy_Id" });
            DropIndex("dbo.Candidates", new[] { "ProjectReplace_Id" });
            DropIndex("dbo.Candidates", new[] { "Store_Id" });
            DropIndex("dbo.Candidates", new[] { "ProjectNew_Id" });
            DropIndex("dbo.Candidates", new[] { "Form_Id" });
            DropIndex("dbo.Candidates", new[] { "Position_Id" });
            DropIndex("dbo.Candidates", new[] { "CityRegion_Id" });
            DropIndex("dbo.ActionAuditTrails", new[] { "Candidates_Id" });
            DropTable("dbo.EProjectVacancies");
            DropTable("dbo.EProjectUser");
            DropTable("dbo.RoleUsers");
            DropTable("dbo.PermissionRoles");
            DropTable("dbo.InterviewCandidates");
            DropTable("dbo.EProjectCityRegions");
            DropTable("dbo.VacancyCityRegions");
            DropTable("dbo.Notifications");
            DropTable("dbo.LoginAuditTrails");
            DropTable("dbo.InterviewComments");
            DropTable("dbo.EFormAuditTrails");
            DropTable("dbo.DateTimeInterviews");
            DropTable("dbo.CandidateCodes");
            DropTable("dbo.AuditTrails");
            DropTable("dbo.Regions");
            DropTable("dbo.Mails");
            DropTable("dbo.MailConfigs");
            DropTable("dbo.Permissions");
            DropTable("dbo.Roles");
            DropTable("dbo.EProjectReplaces");
            DropTable("dbo.Stores");
            DropTable("dbo.EProjectNews");
            DropTable("dbo.Interviews");
            DropTable("dbo.Users");
            DropTable("dbo.EProjects");
            DropTable("dbo.Forms");
            DropTable("dbo.Vacancies");
            DropTable("dbo.GroupVacancies");
            DropTable("dbo.Customers");
            DropTable("dbo.Cities");
            DropTable("dbo.CityRegions");
            DropTable("dbo.Candidates");
            DropTable("dbo.ActionAuditTrails");
        }
    }
}
