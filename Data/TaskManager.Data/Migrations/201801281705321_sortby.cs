namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sortby : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "SortBy", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "SortBy");
        }
    }
}
