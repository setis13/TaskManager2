namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comment", "Status", c => c.Byte(nullable: false));
            AddColumn("dbo.Comment", "Progress", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comment", "Progress");
            DropColumn("dbo.Comment", "Status");
        }
    }
}
