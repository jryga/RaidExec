using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RaidExec.Models
{
    public class UserAccount
    {
        public UserAccount()
        {
            Role = "";
            Rating = 0;
            MostPlayed = "";
            RaidsCompleted = 0;

            Events = new List<Event>();
            Characters = new List<Character>();
        }

        [Key]
        public int Id { get; set; }

        public int Rating { get; set; }

        public string Role { get; set; }

        public string MostPlayed { get; set; }

        public int RaidsCompleted { get; set; }

        [StringLength(200)]
        public string UrlImage { get; set; }

        public ICollection<Event> Events { get; set; }

        [Required]
        public ICollection<Character> Characters { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string Name
        {
            get
            {
                return ApplicationUser.UserName;
            }
        }

        public string Email
        {
            get
            {
                return ApplicationUser.Email;
            }
        }

        public IEnumerable<Game> Games
        {
            get
            {
                var games = new List<Game>();

                foreach (var chara in Characters)
                {
                    if (!games.Contains(chara.Game))
                    {
                        games.Add(chara.Game);
                    }
                }
                games.OrderBy(e => e.Name);

                return games;
            }
        }
    }
}