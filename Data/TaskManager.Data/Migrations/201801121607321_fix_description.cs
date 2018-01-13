namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_description : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubTask", "Description", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubTask", "Description", c => c.String(nullable: false, maxLength: 1024));
        }
    }
}
