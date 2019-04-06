namespace RaidExec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Somethingbroke : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupInvites", "Declined", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupInvites", "Declined");
        }
    }
}
