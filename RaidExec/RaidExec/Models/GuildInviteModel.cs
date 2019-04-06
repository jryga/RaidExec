using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaidExec.Models
{
    public class GuildInvite
    {
        public GuildInvite()
        {
            DateReceived = DateTime.Now;
            Declined = false;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateReceived { get; set; }

        public Guild Guild { get; set; }

        public Character Character { get; set; }
        public bool Declined { get; set; }
    }
}