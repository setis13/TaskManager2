namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alarms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alarm",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 64),
                        Description = c.String(maxLength: 256),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        RepeatType = c.Byte(),
                        RepeatValue = c.Byte(),
                        Birthday = c.Boolean(nullable: false),
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
            DropForeignKey("dbo.Alarm", "LastModifiedById", "dbo.Users");
            DropForeignKey("dbo.Alarm", "CreatedById", "dbo.Users");
            DropIndex("dbo.Alarm", new[] { "LastModifiedById" });
            DropIndex("dbo.Alarm", new[] { "CreatedById" });
            DropTable("dbo.Alarm");
        }
    }
}
