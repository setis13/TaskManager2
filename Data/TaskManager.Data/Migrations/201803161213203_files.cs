namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class files : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.File",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        ParentId = c.Guid(nullable: false),
                        FileName = c.String(),
                        Size = c.Long(nullable: false),
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
            DropForeignKey("dbo.File", "LastModifiedById", "dbo.Users");
            DropForeignKey("dbo.File", "CreatedById", "dbo.Users");
            DropIndex("dbo.File", new[] { "LastModifiedById" });
            DropIndex("dbo.File", new[] { "CreatedById" });
            DropTable("dbo.File");
        }
    }
}
