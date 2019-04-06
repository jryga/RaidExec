using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaidExec.Models
{
    public class Game
    {
        public Game()
        {
            Raids = new List<Raid>();
            Events = new List<Event>();
            Characters = new List<Character>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int MaxLevel { get; set; }

        [Required]
        [StringLength(400)]
        public string Description { get; set; }

        public ICollection<Raid> Raids { get; set; }

        public ICollection<Event> Events { get; set; }

        public ICollection<Character> Characters { get; set; }
    }
}