namespace Ehr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        PhoneNumber = c.String(),
                        Address = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiskHolds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HoldDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Customer_Id = c.Int(),
                        Disk_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Disks", t => t.Disk_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Disk_Id);
            
            CreateTable(
                "dbo.Disks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Status = c.Int(nullable: false),
                        DiskTitle_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiskTitles", t => t.DiskTitle_Id)
                .Index(t => t.DiskTitle_Id);
            
            CreateTable(
                "dbo.DiskTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                        Status = c.Int(nullable: false),
                        DiskType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiskTypes", t => t.DiskType_Id)
                .Index(t => t.DiskType_Id);
            
            CreateTable(
                "dbo.DiskTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        Price = c.Double(nullable: false),
                        LateCharge = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LateCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        ChargeOwed = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                        RentReceipt_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderReceipts", t => t.RentReceipt_Id)
                .Index(t => t.RentReceipt_Id);
            
            CreateTable(
                "dbo.OrderReceipts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        ReceiptDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Status = c.Int(nullable: false),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReceiptNumber = c.Int(nullable: false),
                        Prices = c.Double(nullable: false),
                        LateCharge = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                        Disk_Id = c.Int(),
                        Rent_Id = c.Int(),
                        RentReceipt_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Disks", t => t.Disk_Id)
                .ForeignKey("dbo.OrderRents", t => t.Rent_Id)
                .ForeignKey("dbo.OrderReceipts", t => t.RentReceipt_Id)
                .Index(t => t.Disk_Id)
                .Index(t => t.Rent_Id)
                .Index(t => t.RentReceipt_Id);
            
            CreateTable(
                "dbo.OrderRents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        RentLenght = c.Int(nullable: false),
                        RentDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ReceiptDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Status = c.Int(nullable: false),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.OrderLateCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        PayDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Detail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LateCharges", t => t.Detail_Id)
                .Index(t => t.Detail_Id);
            
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
                        Gender = c.String(),
                        Experience = c.String(),
                        ResetPasswordCode = c.String(),
                        ConfirmPasswordCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Username, unique: true);
            
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        Permission_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Permission_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.Permission_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Permission_Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Role_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Role_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RolePermissions", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.RolePermissions", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.OrderLateCharges", "Detail_Id", "dbo.LateCharges");
            DropForeignKey("dbo.LateCharges", "RentReceipt_Id", "dbo.OrderReceipts");
            DropForeignKey("dbo.OrderDetails", "RentReceipt_Id", "dbo.OrderReceipts");
            DropForeignKey("dbo.OrderDetails", "Rent_Id", "dbo.OrderRents");
            DropForeignKey("dbo.OrderRents", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.OrderDetails", "Disk_Id", "dbo.Disks");
            DropForeignKey("dbo.OrderReceipts", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.DiskHolds", "Disk_Id", "dbo.Disks");
            DropForeignKey("dbo.DiskTitles", "DiskType_Id", "dbo.DiskTypes");
            DropForeignKey("dbo.Disks", "DiskTitle_Id", "dbo.DiskTitles");
            DropForeignKey("dbo.DiskHolds", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.UserRoles", new[] { "Role_Id" });
            DropIndex("dbo.UserRoles", new[] { "User_Id" });
            DropIndex("dbo.RolePermissions", new[] { "Permission_Id" });
            DropIndex("dbo.RolePermissions", new[] { "Role_Id" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.Permissions", new[] { "PermissionCode" });
            DropIndex("dbo.OrderLateCharges", new[] { "Detail_Id" });
            DropIndex("dbo.OrderRents", new[] { "Customer_Id" });
            DropIndex("dbo.OrderDetails", new[] { "RentReceipt_Id" });
            DropIndex("dbo.OrderDetails", new[] { "Rent_Id" });
            DropIndex("dbo.OrderDetails", new[] { "Disk_Id" });
            DropIndex("dbo.OrderReceipts", new[] { "Customer_Id" });
            DropIndex("dbo.LateCharges", new[] { "RentReceipt_Id" });
            DropIndex("dbo.DiskTitles", new[] { "DiskType_Id" });
            DropIndex("dbo.Disks", new[] { "DiskTitle_Id" });
            DropIndex("dbo.DiskHolds", new[] { "Disk_Id" });
            DropIndex("dbo.DiskHolds", new[] { "Customer_Id" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.RolePermissions");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Permissions");
            DropTable("dbo.OrderLateCharges");
            DropTable("dbo.OrderRents");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.OrderReceipts");
            DropTable("dbo.LateCharges");
            DropTable("dbo.DiskTypes");
            DropTable("dbo.DiskTitles");
            DropTable("dbo.Disks");
            DropTable("dbo.DiskHolds");
            DropTable("dbo.Customers");
        }
    }
}
