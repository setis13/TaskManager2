namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class task_user : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskUser",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        TaskId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Guid(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.LastModifiedById)
                .ForeignKey("dbo.Task", t => t.TaskId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => new { t.TaskId, t.UserId }, unique: true, name: "IX_TaskUser")
                .Index(t => t.CreatedById)
                .Index(t => t.LastModifiedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskUser", "UserId", "dbo.Users");
            DropForeignKey("dbo.TaskUser", "TaskId", "dbo.Task");
            DropForeignKey("dbo.TaskUser", "LastModifiedById", "dbo.Users");
            DropForeignKey("dbo.TaskUser", "CreatedById", "dbo.Users");
            DropIndex("dbo.TaskUser", new[] { "LastModifiedById" });
            DropIndex("dbo.TaskUser", new[] { "CreatedById" });
            DropIndex("dbo.TaskUser", "IX_TaskUser");
            DropTable("dbo.TaskUser");
        }
    }
}
