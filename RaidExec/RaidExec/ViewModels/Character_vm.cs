using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    public class CharacterAddForm
    {
        [Required]
        [Display(Name = "Available Games")]
        public SelectList GameList { get; set; }

        [Range(1, Int32.MaxValue)]
        [Display(Name = "Level")]
        public int Level { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Class")]
        public string Class { get; set; }

        [Required]
        [Display(Name = "Account")]
        public UserAccountBase UserAccount { get; set; }

        [Display(Name = "Image")]
        public string UrlImage { get; set; }
    }

    public class CharacterAdd : CharacterAddForm
    {
        [Range(1, Int32.MaxValue)]
        public int GameId { get; set; }
    }

    public class CharacterBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Level")]
        public int Level { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Class")]
        public string Class { get; set; }

        [Display(Name = "Game")]
        public GameBase Game { get; set; }

        [Display(Name = "Account")]
        public UserAccountBase UserAccount { get; set; }

        [Display(Name = "Image")]
        public string UrlImage { get; set; }
    }

    public class CharacterDetails : CharacterBase
    {
        [Display(Name = "Group")]
        public GroupBase Group { get; set; }

        [Display(Name = "Guild")]
        public GuildBase Guild { get; set; }

        [Display(Name = "Raids")]
        public IEnumerable<RaidBase> Raids { get; set; }

        [Display(Name = "Raid Invites")]
        public IEnumerable<RaidInviteBase> RaidInvites { get; set; }

        [Display(Name = "Group Invites")]
        public IEnumerable<GroupInviteBase> GroupInvites { get; set; }

        [Display(Name = "Guild Invites")]
        public IEnumerable<GuildInviteBase> GuildInvites { get; set; }


        [Display(Name = "Raid Applications")]
        public IEnumerable<RaidInviteBase> RaidApplications { get; set; }

        [Display(Name = "Group Applications")]
        public IEnumerable<GroupInviteBase> GroupApplications { get; set; }

        [Display(Name = "Guild Applications")]
        public IEnumerable<GuildInviteBase> GuildApplications { get; set; }
    }

    public class CharacterGuildFind
    {
        public CharacterGuild Character { get; set; }

        [Display(Name = "Guilds")]
        public IEnumerable<GuildFind> Guilds { get; set; }
    }

    public class GuildFind
        {
            public int Id { get; set; }

        [Display(Name = "Name")]
       public string  Name { get; set; }

        [Display(Name = "Leader")]
        public string LeaderName { get; set; }

        public string Username { get; set; }
    }

    public class CharacterGuild
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Guild")]
        public GuildBase Guild { get; set; }
    }

    //public class CharacterEditForm
    //{
    //    [Key]
    //    [Range(1, Int32.MaxValue)]
    //    public int Id { get; set; }

    //    [Range(1, Int32.MaxValue)]
    //    [Display(Name = "Level")]
    //    public int Level { get; set; }

    //    [Required]
    //    [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [Display(Name = "Name")]
    //    public string Name { get; set; }

    //    [Required]
    //    [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [Display(Name = "Class")]
    //    public string Class { get; set; }

    //    [Required]
    //    [Display(Name = "Account")]
    //    public UserAccountBase UserAccount { get; set; }
    //}

    public class CharacterEditForm
    {

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public int Level { get; set; }

        public string Class { get; set; }

        public UserAccountBase UserAccount { get; set; }

        [Display(Name = "Image")]
        public string UrlImage { get; set; }
    }

    public class CharacterEditInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Name { get; set; }

        public int Level { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Class { get; set; }

        public string UrlImage { get; set; }
    }
}