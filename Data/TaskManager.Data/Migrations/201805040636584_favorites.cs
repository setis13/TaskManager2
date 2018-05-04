namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class favorites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserFavorite",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        TaskId = c.Guid(),
                        SubTaskId = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Guid(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.LastModifiedById)
                .ForeignKey("dbo.SubTask", t => t.SubTaskId)
                .ForeignKey("dbo.Task", t => t.TaskId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => new { t.UserId, t.TaskId, t.SubTaskId }, unique: true, name: "IX_UserFavorite")
                .Index(t => t.CreatedById)
                .Index(t => t.LastModifiedById);
            
            AddColumn("dbo.Users", "FavoriteFilter", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserFavorite", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserFavorite", "TaskId", "dbo.Task");
            DropForeignKey("dbo.UserFavorite", "SubTaskId", "dbo.SubTask");
            DropForeignKey("dbo.UserFavorite", "LastModifiedById", "dbo.Users");
            DropForeignKey("dbo.UserFavorite", "CreatedById", "dbo.Users");
            DropIndex("dbo.UserFavorite", new[] { "LastModifiedById" });
            DropIndex("dbo.UserFavorite", new[] { "CreatedById" });
            DropIndex("dbo.UserFavorite", "IX_UserFavorite");
            DropColumn("dbo.Users", "FavoriteFilter");
            DropTable("dbo.UserFavorite");
        }
    }
}
