using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaidExec.Models
{
    public class Character
    {
        public Character()
        {
            Class = "None";
            Raids = new List<Raid>();
            RaidInvites = new List<RaidInvite>();
            GroupInvites = new List<GroupInvite>();
            GuildInvites = new List<GuildInvite>();

            RaidApplications = new List<RaidInvite>();
            GroupApplications = new List<GroupInvite>();
            GuildApplications = new List<GuildInvite>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Class { get; set; }

        [StringLength(200)]
        public string UrlImage { get; set; }

        public Game Game { get; set; }

        public Group Group { get; set; }

        public Guild Guild { get; set; }

        public UserAccount UserAccount { get; set; }

        public ICollection<Raid> Raids { get; set; }

        public ICollection<RaidInvite> RaidInvites { get; set; }

        public ICollection<GroupInvite> GroupInvites { get; set; }

        public ICollection<GuildInvite> GuildInvites { get; set; }

        public ICollection<RaidInvite> RaidApplications { get; set; }

        public ICollection<GroupInvite> GroupApplications { get; set; }

        public ICollection<GuildInvite> GuildApplications { get; set; }

    }
}