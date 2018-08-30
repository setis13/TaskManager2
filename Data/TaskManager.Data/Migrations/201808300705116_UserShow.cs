namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserShow : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserShow",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CommentId = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Guid(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Comment", t => t.CommentId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.LastModifiedById)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => new { t.UserId, t.CommentId }, unique: true, name: "IX_UserShow")
                .Index(t => t.CreatedById)
                .Index(t => t.LastModifiedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserShow", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserShow", "LastModifiedById", "dbo.Users");
            DropForeignKey("dbo.UserShow", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.UserShow", "CommentId", "dbo.Comment");
            DropIndex("dbo.UserShow", new[] { "LastModifiedById" });
            DropIndex("dbo.UserShow", new[] { "CreatedById" });
            DropIndex("dbo.UserShow", "IX_UserShow");
            DropTable("dbo.UserShow");
        }
    }
}
