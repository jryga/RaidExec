using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    public class EventAddForm
    {
        public EventAddForm()
        {

        }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Range(1, Int32.MaxValue)]
        [Display(Name = "Size limit")]
        public int SizeLimit { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Game")]
        public SelectList GameList { get; set; }
    }

    public class EventAdd
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Range(1, Int32.MaxValue)]
        [Display(Name = "Size limit")]
        public int SizeLimit { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Range(1, Int32.MaxValue)]
        public int GameId { get; set; }
    }

    public class EventBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Size limit")]
        public int SizeLimit { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public int GameId { get; set; }

        [Display(Name = "Game")]
        public string GameName { get; set; }

        [Display(Name = "Creator")]
        public UserAccountBase Creator { get; set; }
    }

    public class EventIndex
    {
        public IEnumerable<EventBase> Events { get; set; }

        public IEnumerable<int> GameIds { get; set; }
    }

    public class EventDetails : EventBase
    {
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Raids")]
        public IEnumerable<RaidBase> Raids { get; set; }
    }

    public class EventEditForm
    {
        [Key]
        [Range(1, Int32.MaxValue)]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Range(1, Int32.MaxValue)]
        [Display(Name = "Size Limit")]
        public int SizeLimit { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public class EventEdit
    {
        [Key]
        [Range(1, Int32.MaxValue)]
        public int Id { get; set; }

        [Range(1, Int32.MaxValue)]
        [Display(Name = "Size Limit")]
        public int SizeLimit { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public class EventRaidAddForm
    {
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Scheduled time")]
        public DateTime ScheduledTime { get; set; }

        [Display(Name = "Event")]
        public string Name { get; set; }

        [Range(1, Int32.MaxValue)]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Available characters")]
        public SelectList LeaderList { get; set; }
    }

    public class EventRaidAdd : EventRaidAddForm
    {
        [Range(1, Int32.MaxValue)]
        public int LeaderId { get; set; }
    }
}