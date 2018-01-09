namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class root : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyId = c.Guid(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .Index(t => t.CompanyId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Session",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        LastActivity = c.DateTime(nullable: false),
                        IpAddress = c.String(nullable: false, maxLength: 30),
                        UserAgent = c.String(nullable: false, maxLength: 512),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Guid(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.LastModifiedById)
                .Index(t => t.CreatedById)
                .Index(t => t.LastModifiedById);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 64),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Guid(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.LastModifiedById)
                .Index(t => t.CreatedById)
                .Index(t => t.LastModifiedById);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 32),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Guid(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.LastModifiedById)
                .Index(t => t.CompanyId)
                .Index(t => t.CreatedById)
                .Index(t => t.LastModifiedById);
            
            CreateTable(
                "dbo.Task",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                        Index = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 64),
                        Description = c.String(maxLength: 1024),
                        Priority = c.Byte(nullable: false),
                        ActualWork = c.Time(nullable: false, precision: 7),
                        TotalWork = c.Time(nullable: false, precision: 7),
                        Progress = c.Single(nullable: false),
                        Status = c.Byte(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Guid(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.LastModifiedById)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .Index(t => t.CompanyId)
                .Index(t => t.ProjectId)
                .Index(t => t.CreatedById)
                .Index(t => t.LastModifiedById);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        TaskId = c.Guid(),
                        SubTaskId = c.Guid(),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(maxLength: 2048),
                        ActualWork = c.Time(nullable: false, precision: 7),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Guid(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.LastModifiedById)
                .ForeignKey("dbo.SubTask", t => t.SubTaskId)
                .ForeignKey("dbo.Task", t => t.TaskId)
                .Index(t => t.CompanyId)
                .Index(t => t.TaskId)
                .Index(t => t.SubTaskId)
                .Index(t => t.CreatedById)
                .Index(t => t.LastModifiedById);
            
            CreateTable(
                "dbo.SubTask",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        TaskId = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 64),
                        Description = c.String(nullable: false, maxLength: 1024),
                        ActualWork = c.Time(nullable: false, precision: 7),
                        TotalWork = c.Time(nullable: false, precision: 7),
                        Progress = c.Single(nullable: false),
                        Status = c.Byte(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Guid(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.LastModifiedById)
                .ForeignKey("dbo.Task", t => t.TaskId)
                .Index(t => t.CompanyId)
                .Index(t => t.TaskId)
                .Index(t => t.CreatedById)
                .Index(t => t.LastModifiedById);
            
            CreateTable(
                "dbo.Todo",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 64),
                        Description = c.String(maxLength: 1024),
                        Priority = c.Byte(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Guid(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.LastModifiedById)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .Index(t => t.CompanyId)
                .Index(t => t.ProjectId)
                .Index(t => t.CreatedById)
                .Index(t => t.LastModifiedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Todo", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Todo", "LastModifiedById", "dbo.Users");
            DropForeignKey("dbo.Todo", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Todo", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Task", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Task", "LastModifiedById", "dbo.Users");
            DropForeignKey("dbo.Task", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Task", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Comment", "TaskId", "dbo.Task");
            DropForeignKey("dbo.SubTask", "TaskId", "dbo.Task");
            DropForeignKey("dbo.SubTask", "LastModifiedById", "dbo.Users");
            DropForeignKey("dbo.SubTask", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SubTask", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Comment", "SubTaskId", "dbo.SubTask");
            DropForeignKey("dbo.Comment", "LastModifiedById", "dbo.Users");
            DropForeignKey("dbo.Comment", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Comment", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Project", "LastModifiedById", "dbo.Users");
            DropForeignKey("dbo.Project", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Project", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Users", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Company", "LastModifiedById", "dbo.Users");
            DropForeignKey("dbo.Company", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Session", "LastModifiedById", "dbo.Users");
            DropForeignKey("dbo.Session", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropIndex("dbo.Todo", new[] { "LastModifiedById" });
            DropIndex("dbo.Todo", new[] { "CreatedById" });
            DropIndex("dbo.Todo", new[] { "ProjectId" });
            DropIndex("dbo.Todo", new[] { "CompanyId" });
            DropIndex("dbo.SubTask", new[] { "LastModifiedById" });
            DropIndex("dbo.SubTask", new[] { "CreatedById" });
            DropIndex("dbo.SubTask", new[] { "TaskId" });
            DropIndex("dbo.SubTask", new[] { "CompanyId" });
            DropIndex("dbo.Comment", new[] { "LastModifiedById" });
            DropIndex("dbo.Comment", new[] { "CreatedById" });
            DropIndex("dbo.Comment", new[] { "SubTaskId" });
            DropIndex("dbo.Comment", new[] { "TaskId" });
            DropIndex("dbo.Comment", new[] { "CompanyId" });
            DropIndex("dbo.Task", new[] { "LastModifiedById" });
            DropIndex("dbo.Task", new[] { "CreatedById" });
            DropIndex("dbo.Task", new[] { "ProjectId" });
            DropIndex("dbo.Task", new[] { "CompanyId" });
            DropIndex("dbo.Project", new[] { "LastModifiedById" });
            DropIndex("dbo.Project", new[] { "CreatedById" });
            DropIndex("dbo.Project", new[] { "CompanyId" });
            DropIndex("dbo.Company", new[] { "LastModifiedById" });
            DropIndex("dbo.Company", new[] { "CreatedById" });
            DropIndex("dbo.Session", new[] { "LastModifiedById" });
            DropIndex("dbo.Session", new[] { "CreatedById" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Users", new[] { "CompanyId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropTable("dbo.Todo");
            DropTable("dbo.SubTask");
            DropTable("dbo.Comment");
            DropTable("dbo.Task");
            DropTable("dbo.Project");
            DropTable("dbo.Company");
            DropTable("dbo.Session");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
        }
    }
}
