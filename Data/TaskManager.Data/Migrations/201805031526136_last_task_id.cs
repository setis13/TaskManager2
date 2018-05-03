namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class last_task_id : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LastModifiedTaskId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LastModifiedTaskId");
        }
    }
}
