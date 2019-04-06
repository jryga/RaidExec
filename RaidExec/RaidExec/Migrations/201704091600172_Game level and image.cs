namespace RaidExec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gamelevelandimage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "UrlImage", c => c.String(maxLength: 200));
            AddColumn("dbo.Games", "MaxLevel", c => c.Int(nullable: false));
            AddColumn("dbo.UserAccounts", "UrlImage", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserAccounts", "UrlImage");
            DropColumn("dbo.Games", "MaxLevel");
            DropColumn("dbo.Characters", "UrlImage");
        }
    }
}
