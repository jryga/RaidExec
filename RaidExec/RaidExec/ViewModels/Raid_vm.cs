using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    public class RaidSimpleAddForm
    {
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Scheduled Time")]
        public DateTime ScheduledTime { get; set; }

        [Display(Name = "Game")]
        public string GameName { get; set; }

        [Range(1, Int32.MaxValue)]
        [HiddenInput(DisplayValue = false)]
        public int GameId { get; set; }

        [Display(Name = "Event")]
        public string EventName { get; set; }

        [Range(1, Int32.MaxValue)]
        [HiddenInput(DisplayValue = false)]
        public int EventId { get; set; }

        [Required]
        [Display(Name = "Available Characters")]
        public SelectList LeaderList { get; set; }
    }

    public class RaidSimpleAdd : RaidSimpleAddForm
    {
        [Range(1, Int32.MaxValue)]
        public int LeaderId { get; set; }
    }

    public class RaidBase
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Scheduled Time")]
        public DateTime ScheduledTime { get; set; }

        [Display(Name = "Game")]
        public GameBase Game { get; set; }

        [Display(Name = "Event")]
        public EventBase Event { get; set; }

        [Display(Name = "Leader")]
        public CharacterBase Leader { get; set; }

        [Display(Name = "Size")]
        public int Size { get; set; }

        [Display(Name = "Size Limit")]
        public int SizeLimit { get; set; }

        [Display(Name = "Spots Remaining")]
        public int SpotsRemaining { get; set; }

        [Display(Name = "Spots")]
        public string Spots
        {
            get
            {
                return Size + "/" + SizeLimit;
            }
        }
    }

    public class RaidDetails : RaidBase
    {
        [Display(Name = "Members")]
        public IEnumerable<CharacterBase> Members { get; set; }
        public IEnumerable<RaidInviteBase> Invites { get; set; }
        public IEnumerable<RaidInviteBase> Applications { get; set; }
    }
    public class RaidInviteForm
    {
        public int Id { get; set; }
        public DateTime ScheduledTime { get; set; }
        public String EventName { get; set; }
        public MultiSelectList CharList { get; set; }
    }
    public class RaidInviteInfo
    {
        public int Id { get; set; }
        public IEnumerable<int> Characters { get; set; }
    }
    public class RaidEditForm
    {
        [Key]
        [Range(1, Int32.MaxValue)]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Scheduled Time")]
        public DateTime ScheduledTime { get; set; }

        [Display(Name = "Game")]
        public string GameName { get; set; }

        [Display(Name = "Event")]
        public string EventName { get; set; }

        [Display(Name = "Leader")]
        public string LeaderName { get; set; }
    }

    public class RaidEdit
    {
        [Key]
        [Range(1, Int32.MaxValue)]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Scheduled Time")]
        public DateTime ScheduledTime { get; set; }
    }
    public class RaidApplicationForm
    {
        public int Id { get; set; }
        public String EventName { get; set; }
        public CharacterBase Leader { get; set; }
        public SelectList CharList { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Scheduled Time")]
        public DateTime ScheduledTime { get; set; }
    }
    public class RaidApplicationInfo
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
    }
}