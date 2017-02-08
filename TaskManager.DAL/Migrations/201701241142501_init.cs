namespace TaskManager.DAL.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId);
            
            CreateTable(
                "dbo.Subproject",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Name = c.String(),
                        ProjectId = c.Guid(nullable: false),
                        Hours = c.Time(nullable: false, precision: 7),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Task",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Status = c.Byte(nullable: false),
                        Important = c.Byte(nullable: false),
                        Progress = c.Byte(nullable: false),
                        Date = c.DateTime(nullable: false),
                        SubprojectId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Subproject", t => t.SubprojectId)
                .Index(t => t.SubprojectId);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Text = c.String(),
                        Hours = c.Time(precision: 7),
                        TaskId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Task", t => t.TaskId)
                .Index(t => t.TaskId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "TaskId", "dbo.Task");
            DropForeignKey("dbo.Task", "SubprojectId", "dbo.Subproject");
            DropForeignKey("dbo.Subproject", "ProjectId", "dbo.Project");
            DropIndex("dbo.Comment", new[] { "TaskId" });
            DropIndex("dbo.Task", new[] { "SubprojectId" });
            DropIndex("dbo.Subproject", new[] { "ProjectId" });
            DropTable("dbo.Comment");
            DropTable("dbo.Task");
            DropTable("dbo.Subproject");
            DropTable("dbo.Project");
        }
    }
}
