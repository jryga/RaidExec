using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaidExec.Models
{
    public class GroupInvite
    {
        public GroupInvite()
        {
            DateReceived = DateTime.Now;
            Declined = false;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateReceived { get; set; }

        public Group Group { get; set; }

        public Character Character { get; set; }
        public bool Declined { get; set; }

    }
}