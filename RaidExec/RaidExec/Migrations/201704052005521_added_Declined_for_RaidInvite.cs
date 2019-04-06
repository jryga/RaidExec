namespace RaidExec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_Declined_for_RaidInvite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RaidInvites", "Declined", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RaidInvites", "Declined");
        }
    }
}
