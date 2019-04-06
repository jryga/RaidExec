using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaidExec.Models
{
    public class Event
    {
        public Event()
        {
            Active = true;
            Raids = new List<Raid>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int SizeLimit { get; set; }

        [Required]
        [StringLength(400)]
        public string Description { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public Game Game { get; set; }

        public UserAccount Creator { get; set; }

        public ICollection<Raid> Raids { get; set; }
    }
}