using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaidExec.Models
{
    public class Raid
    {
        public Raid()
        {
            Active = true;
            Members = new List<Character>();
            Invites = new List<RaidInvite>();
            Applications = new List<RaidInvite>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime ScheduledTime { get; set; }

        public Event Event { get; set; }

        public Character Leader { get; set; }

        public ICollection<Character> Members { get; set; }

        [Required]
        public ICollection<RaidInvite> Invites { get; set; }

        [Required]
        public ICollection<RaidInvite> Applications { get; set; }

        public Game Game
        {
            get
            {
                return Event.Game;
            }
        }

        public int Size
        {
            get
            {
                return Members.Count + 1;
            }
        }

        public int SizeLimit
        {
            get
            {
                return Event.SizeLimit;
            }
        }

        public int SpotsRemaining
        {
            get
            {
                return SizeLimit - Size;
            }
        }

        public IEnumerable<Character> AllCharacters
        {
            get
            {
                var all = new List<Character>();

                all.Add(Leader);
                all.AddRange(Members);

                return all;
            }
        }
    }
}