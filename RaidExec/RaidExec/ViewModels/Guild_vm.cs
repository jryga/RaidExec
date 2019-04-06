using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    public class GuildSimpleAddForm
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(400, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Leader")]
        public CharacterBase Leader { get; set; }

        [Display(Name = "Game")]
        public GameBase Game
        {
            get
            {
                return Leader.Game;
            }
        }
    }

    public class GuildSimpleAdd : GuildSimpleAddForm
    {

    }

    //public class GuildEditForm
    //{
    //    [Key]
    //    [Range(1, Int32.MaxValue)]
    //    public int Id { get; set; }

    //    [Display(Name = "Name")]
    //    public string Name { get; set; }

    //    [Required]
    //    [StringLength(400, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [Display(Name = "Description")]
    //    public string Description { get; set; }
    //}

    public class GuildBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Game")]
        public GameBase Game { get; set; }

        [Display(Name = "Leader")]
        public CharacterBase Leader { get; set; }
    }

    public class GuildDetails : GuildBase
    {
        [Display(Name = "Members")]
        public IEnumerable<CharacterBase> Members { get; set; }

        [Display(Name = "Invites")]
        public IEnumerable<GuildInviteBase> Invites { get; set; }

        [Display(Name = "Applications")]
        public IEnumerable<GuildInviteBase> Applications { get; set; }
    }

    public class GuildAdd
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public int GameId { get; set; }
        public int LeaderId { get; set; }
        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }


    public class GuildAddForm
    {
        public GuildAddForm() { }
        public string Name { get; set; }
        public string errorName { get; set; }
        public string LeaderName { get; set;}
        public int LeaderId { get; set; }
        public string GameName { get; set; }
        public int GameId { get; set; }
        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Description")]
        public string Description { get; set; }

    }
    public class GuildEditForm
    {
        public GuildEditForm() { }
        public int Id { get; set; }
        public int GameId { get; set; }
        public String Name { get; set; }
        public IEnumerable<CharacterBase> Characters { get; set; }
        public MultiSelectList CharList { get; set; }
        public CharacterBase Leader { get; set; }
    }

    public class GuildEditInfo
    {
        public GuildEditInfo() { }
        public int Id { get; set; }
        public String Name { get; set; }
        public IEnumerable<int> Characters { get; set; }
        public CharacterBase Leader { get; set; }

    }
    public class GuildEditLeaderForm
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public SelectList LeaderId{ get; set; }
        public string errorName { get; set; }
        public int GameId { get; set; }
        public CharacterBase Leader { get; set; }
        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
    public class GuildEditLeaderInfo
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int LeaderId { get; set; }
        public int GameId { get; set; }
        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}