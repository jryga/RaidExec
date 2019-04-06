using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaidExec.Models
{
    public class Guild
    {
        public Guild()
        {
            Members = new List<Character>();
            Invites = new List<GuildInvite>();
            Applications = new List<GuildInvite>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(400)]
        public string Description { get; set; }

        public Game Game { get; set; }

        public Character Leader { get; set; }

        public ICollection<Character> Members { get; set; }

        [Required]
        public ICollection<GuildInvite> Invites { get; set; }

        [Required]
        public ICollection<GuildInvite> Applications { get; set; }

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

        public int Size
        {
            get
            {
                return Members.Count + 1;
            }
        }
    }
}