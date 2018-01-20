namespace TaskManager.Data.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invitation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "InvitationCompanyId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "InvitationCompanyId");
        }
    }
}
