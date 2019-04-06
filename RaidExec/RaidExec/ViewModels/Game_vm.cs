using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    public class GameAddForm
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [AllowHtml]
        [StringLength(400, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    public class GameAdd : GameAddForm
    {

    }

    public class GameBase : IEquatable<GameBase>
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        public override int GetHashCode()
        {
            return Id;
        }

        public bool Equals(GameBase other)
        {
            return Id == other.Id;
        }
    }

    public class GameDetails : GameBase
    {

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Raids")]
        public IEnumerable<RaidBase> Raids { get; set; }

        [Display(Name = "Events")]
        public IEnumerable<EventBase> Events { get; set; }

        [Display(Name = "Characters")]
        public IEnumerable<CharacterBase> Characters { get; set; }
    }
    public class GameEditForm
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [AllowHtml]
        [StringLength(400, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
    public class GameEditInfo
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [StringLength(400, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}