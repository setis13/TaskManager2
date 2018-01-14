namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_timespan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Task", "ActualWorkTicks", c => c.Long(nullable: false));
            AddColumn("dbo.Task", "TotalWorkTicks", c => c.Long(nullable: false));
            AddColumn("dbo.SubTask", "ActualWorkTicks", c => c.Long(nullable: false));
            AddColumn("dbo.SubTask", "TotalWorkTicks", c => c.Long(nullable: false));
            DropColumn("dbo.Task", "ActualWork");
            DropColumn("dbo.Task", "TotalWork");
            DropColumn("dbo.SubTask", "ActualWork");
            DropColumn("dbo.SubTask", "TotalWork");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubTask", "TotalWork", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.SubTask", "ActualWork", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Task", "TotalWork", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Task", "ActualWork", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.SubTask", "TotalWorkTicks");
            DropColumn("dbo.SubTask", "ActualWorkTicks");
            DropColumn("dbo.Task", "TotalWorkTicks");
            DropColumn("dbo.Task", "ActualWorkTicks");
        }
    }
}
