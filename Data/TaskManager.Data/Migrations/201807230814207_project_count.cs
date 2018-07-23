namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class project_count : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "Count", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "Count");
        }
    }
}
