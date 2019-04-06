using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaidExec.Models
{
    public class Group
    {
        public Group()
        {
            Members = new List<Character>();
            Invites = new List<GroupInvite>();
            Applications = new List<GroupInvite>();
        }

        [Key]
        public int Id { get; set; }

        public Game Game { get; set; }

        public Character Leader { get; set; }

        public ICollection<Character> Members { get; set; }

        [Required]
        public ICollection<GroupInvite> Invites { get; set; }

        [Required]
        public ICollection<GroupInvite> Applications { get; set; }

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