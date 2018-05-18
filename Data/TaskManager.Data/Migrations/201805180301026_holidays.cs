namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class holidays : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Alarm", "Holiday", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Alarm", "Holiday");
        }
    }
}
