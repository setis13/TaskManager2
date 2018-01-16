namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cooment_nullable_percent : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comment", "ActualWork", c => c.Time(precision: 7));
            AlterColumn("dbo.Comment", "Progress", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comment", "Progress", c => c.Single(nullable: false));
            AlterColumn("dbo.Comment", "ActualWork", c => c.Time(nullable: false, precision: 7));
        }
    }
}
