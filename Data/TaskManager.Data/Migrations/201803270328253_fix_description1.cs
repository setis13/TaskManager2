namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_description1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Task", "Description", c => c.String());
            AlterColumn("dbo.Comment", "Description", c => c.String());
            AlterColumn("dbo.SubTask", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubTask", "Description", c => c.String(maxLength: 1024));
            AlterColumn("dbo.Comment", "Description", c => c.String(maxLength: 2048));
            AlterColumn("dbo.Task", "Description", c => c.String(maxLength: 1024));
        }
    }
}
