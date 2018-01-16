namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comment_timespan_nullable_ticks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comment", "ActualWorkTicks", c => c.Long());
            DropColumn("dbo.Comment", "ActualWork");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comment", "ActualWork", c => c.Time(precision: 7));
            DropColumn("dbo.Comment", "ActualWorkTicks");
        }
    }
}
