using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaidExec.Controllers
{
    public class GroupInviteBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Date Received")]
        public DateTime DateReceived { get; set; }

        [Display(Name = "From Group")]
        public GroupBase Group { get; set; }

        [Display(Name = "To Character")]
        public CharacterBase Character { get; set; }

        public bool Declined { get; set; }
    }
}