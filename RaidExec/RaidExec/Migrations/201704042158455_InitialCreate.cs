namespace RaidExec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Level = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Class = c.String(nullable: false, maxLength: 50),
                        Game_Id = c.Int(),
                        UserAccount_Id = c.Int(),
                        Group_Id = c.Int(),
                        Group_Id1 = c.Int(),
                        Guild_Id = c.Int(),
                        Guild_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .ForeignKey("dbo.UserAccounts", t => t.UserAccount_Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id1)
                .ForeignKey("dbo.Guilds", t => t.Guild_Id)
                .ForeignKey("dbo.Guilds", t => t.Guild_Id1)
                .Index(t => t.Game_Id)
                .Index(t => t.UserAccount_Id)
                .Index(t => t.Group_Id)
                .Index(t => t.Group_Id1)
                .Index(t => t.Guild_Id)
                .Index(t => t.Guild_Id1);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 400),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        SizeLimit = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 400),
                        DateCreated = c.DateTime(nullable: false),
                        Creator_Id = c.Int(),
                        Game_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAccounts", t => t.Creator_Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Creator_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        Role = c.String(),
                        MostPlayed = c.String(),
                        RaidsCompleted = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        UserAccount_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAccounts", t => t.UserAccount_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.UserAccount_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Raids",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        ScheduledTime = c.DateTime(nullable: false),
                        Event_Id = c.Int(),
                        Leader_Id = c.Int(),
                        Game_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .ForeignKey("dbo.Characters", t => t.Leader_Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Event_Id)
                .Index(t => t.Leader_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.RaidInvites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateReceived = c.DateTime(nullable: false),
                        Character_Id = c.Int(),
                        Raid_Id = c.Int(),
                        Raid_Id1 = c.Int(),
                        Raid_Id2 = c.Int(),
                        Character_Id1 = c.Int(),
                        Character_Id2 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .ForeignKey("dbo.Raids", t => t.Raid_Id)
                .ForeignKey("dbo.Raids", t => t.Raid_Id1)
                .ForeignKey("dbo.Raids", t => t.Raid_Id2)
                .ForeignKey("dbo.Characters", t => t.Character_Id1)
                .ForeignKey("dbo.Characters", t => t.Character_Id2)
                .Index(t => t.Character_Id)
                .Index(t => t.Raid_Id)
                .Index(t => t.Raid_Id1)
                .Index(t => t.Raid_Id2)
                .Index(t => t.Character_Id1)
                .Index(t => t.Character_Id2);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Game_Id = c.Int(),
                        Leader_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .ForeignKey("dbo.Characters", t => t.Leader_Id)
                .Index(t => t.Game_Id)
                .Index(t => t.Leader_Id);
            
            CreateTable(
                "dbo.GroupInvites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateReceived = c.DateTime(nullable: false),
                        Character_Id = c.Int(),
                        Group_Id = c.Int(),
                        Group_Id1 = c.Int(),
                        Group_Id2 = c.Int(),
                        Character_Id1 = c.Int(),
                        Character_Id2 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id1)
                .ForeignKey("dbo.Groups", t => t.Group_Id2)
                .ForeignKey("dbo.Characters", t => t.Character_Id1)
                .ForeignKey("dbo.Characters", t => t.Character_Id2)
                .Index(t => t.Character_Id)
                .Index(t => t.Group_Id)
                .Index(t => t.Group_Id1)
                .Index(t => t.Group_Id2)
                .Index(t => t.Character_Id1)
                .Index(t => t.Character_Id2);
            
            CreateTable(
                "dbo.Guilds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false, maxLength: 400),
                        Game_Id = c.Int(),
                        Leader_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .ForeignKey("dbo.Characters", t => t.Leader_Id)
                .Index(t => t.Game_Id)
                .Index(t => t.Leader_Id);
            
            CreateTable(
                "dbo.GuildInvites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateReceived = c.DateTime(nullable: false),
                        Declined = c.Boolean(nullable: false),
                        Character_Id = c.Int(),
                        Guild_Id = c.Int(),
                        Guild_Id1 = c.Int(),
                        Guild_Id2 = c.Int(),
                        Character_Id1 = c.Int(),
                        Character_Id2 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .ForeignKey("dbo.Guilds", t => t.Guild_Id)
                .ForeignKey("dbo.Guilds", t => t.Guild_Id1)
                .ForeignKey("dbo.Guilds", t => t.Guild_Id2)
                .ForeignKey("dbo.Characters", t => t.Character_Id1)
                .ForeignKey("dbo.Characters", t => t.Character_Id2)
                .Index(t => t.Character_Id)
                .Index(t => t.Guild_Id)
                .Index(t => t.Guild_Id1)
                .Index(t => t.Guild_Id2)
                .Index(t => t.Character_Id1)
                .Index(t => t.Character_Id2);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.CharacterRaid",
                c => new
                    {
                        CharacterId = c.Int(nullable: false),
                        RaidId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CharacterId, t.RaidId })
                .ForeignKey("dbo.Characters", t => t.CharacterId, cascadeDelete: true)
                .ForeignKey("dbo.Raids", t => t.RaidId, cascadeDelete: true)
                .Index(t => t.CharacterId)
                .Index(t => t.RaidId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CharacterRaid", "RaidId", "dbo.Raids");
            DropForeignKey("dbo.CharacterRaid", "CharacterId", "dbo.Characters");
            DropForeignKey("dbo.RaidInvites", "Character_Id2", "dbo.Characters");
            DropForeignKey("dbo.RaidInvites", "Character_Id1", "dbo.Characters");
            DropForeignKey("dbo.GuildInvites", "Character_Id2", "dbo.Characters");
            DropForeignKey("dbo.GuildInvites", "Character_Id1", "dbo.Characters");
            DropForeignKey("dbo.Characters", "Guild_Id1", "dbo.Guilds");
            DropForeignKey("dbo.Characters", "Guild_Id", "dbo.Guilds");
            DropForeignKey("dbo.Guilds", "Leader_Id", "dbo.Characters");
            DropForeignKey("dbo.GuildInvites", "Guild_Id2", "dbo.Guilds");
            DropForeignKey("dbo.Guilds", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.GuildInvites", "Guild_Id1", "dbo.Guilds");
            DropForeignKey("dbo.GuildInvites", "Guild_Id", "dbo.Guilds");
            DropForeignKey("dbo.GuildInvites", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.GroupInvites", "Character_Id2", "dbo.Characters");
            DropForeignKey("dbo.GroupInvites", "Character_Id1", "dbo.Characters");
            DropForeignKey("dbo.Characters", "Group_Id1", "dbo.Groups");
            DropForeignKey("dbo.Characters", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Groups", "Leader_Id", "dbo.Characters");
            DropForeignKey("dbo.GroupInvites", "Group_Id2", "dbo.Groups");
            DropForeignKey("dbo.Groups", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.GroupInvites", "Group_Id1", "dbo.Groups");
            DropForeignKey("dbo.GroupInvites", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.GroupInvites", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.Raids", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Raids", "Leader_Id", "dbo.Characters");
            DropForeignKey("dbo.RaidInvites", "Raid_Id2", "dbo.Raids");
            DropForeignKey("dbo.Raids", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.RaidInvites", "Raid_Id1", "dbo.Raids");
            DropForeignKey("dbo.RaidInvites", "Raid_Id", "dbo.Raids");
            DropForeignKey("dbo.RaidInvites", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.Events", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Events", "Creator_Id", "dbo.UserAccounts");
            DropForeignKey("dbo.Characters", "UserAccount_Id", "dbo.UserAccounts");
            DropForeignKey("dbo.AspNetUsers", "UserAccount_Id", "dbo.UserAccounts");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Characters", "Game_Id", "dbo.Games");
            DropIndex("dbo.CharacterRaid", new[] { "RaidId" });
            DropIndex("dbo.CharacterRaid", new[] { "CharacterId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.GuildInvites", new[] { "Character_Id2" });
            DropIndex("dbo.GuildInvites", new[] { "Character_Id1" });
            DropIndex("dbo.GuildInvites", new[] { "Guild_Id2" });
            DropIndex("dbo.GuildInvites", new[] { "Guild_Id1" });
            DropIndex("dbo.GuildInvites", new[] { "Guild_Id" });
            DropIndex("dbo.GuildInvites", new[] { "Character_Id" });
            DropIndex("dbo.Guilds", new[] { "Leader_Id" });
            DropIndex("dbo.Guilds", new[] { "Game_Id" });
            DropIndex("dbo.GroupInvites", new[] { "Character_Id2" });
            DropIndex("dbo.GroupInvites", new[] { "Character_Id1" });
            DropIndex("dbo.GroupInvites", new[] { "Group_Id2" });
            DropIndex("dbo.GroupInvites", new[] { "Group_Id1" });
            DropIndex("dbo.GroupInvites", new[] { "Group_Id" });
            DropIndex("dbo.GroupInvites", new[] { "Character_Id" });
            DropIndex("dbo.Groups", new[] { "Leader_Id" });
            DropIndex("dbo.Groups", new[] { "Game_Id" });
            DropIndex("dbo.RaidInvites", new[] { "Character_Id2" });
            DropIndex("dbo.RaidInvites", new[] { "Character_Id1" });
            DropIndex("dbo.RaidInvites", new[] { "Raid_Id2" });
            DropIndex("dbo.RaidInvites", new[] { "Raid_Id1" });
            DropIndex("dbo.RaidInvites", new[] { "Raid_Id" });
            DropIndex("dbo.RaidInvites", new[] { "Character_Id" });
            DropIndex("dbo.Raids", new[] { "Game_Id" });
            DropIndex("dbo.Raids", new[] { "Leader_Id" });
            DropIndex("dbo.Raids", new[] { "Event_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "UserAccount_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Events", new[] { "Game_Id" });
            DropIndex("dbo.Events", new[] { "Creator_Id" });
            DropIndex("dbo.Characters", new[] { "Guild_Id1" });
            DropIndex("dbo.Characters", new[] { "Guild_Id" });
            DropIndex("dbo.Characters", new[] { "Group_Id1" });
            DropIndex("dbo.Characters", new[] { "Group_Id" });
            DropIndex("dbo.Characters", new[] { "UserAccount_Id" });
            DropIndex("dbo.Characters", new[] { "Game_Id" });
            DropTable("dbo.CharacterRaid");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.GuildInvites");
            DropTable("dbo.Guilds");
            DropTable("dbo.GroupInvites");
            DropTable("dbo.Groups");
            DropTable("dbo.RaidInvites");
            DropTable("dbo.Raids");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.Events");
            DropTable("dbo.Games");
            DropTable("dbo.Characters");
        }
    }
}
