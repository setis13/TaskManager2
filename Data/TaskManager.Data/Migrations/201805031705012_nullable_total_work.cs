namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullable_total_work : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Task", "TotalWorkTicks", c => c.Long());
            AlterColumn("dbo.SubTask", "TotalWorkTicks", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubTask", "TotalWorkTicks", c => c.Long(nullable: false));
            AlterColumn("dbo.Task", "TotalWorkTicks", c => c.Long(nullable: false));
        }
    }
}
