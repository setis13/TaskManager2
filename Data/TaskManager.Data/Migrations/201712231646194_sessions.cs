namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sessions : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Session", "LastModifiedById", "dbo.Users");
            DropForeignKey("dbo.Session", "CreatedById", "dbo.Users");
            DropIndex("dbo.Session", new[] { "LastModifiedById" });
            DropIndex("dbo.Session", new[] { "CreatedById" });
            DropTable("dbo.Session");
        }
    }
}
