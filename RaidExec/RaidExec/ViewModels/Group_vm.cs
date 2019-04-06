using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    public class GroupBase
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        [Display(Name = "Game")]
        public GameBase Game { get; set; }

        [Display(Name = "Leader")]
        public CharacterBase Leader { get; set; }

        public bool invFlag { get; set; }
        public int CurrChar { get; set; }
    }

    public class GroupDetails : GroupBase
    {
        [Display(Name = "Members")]
        public IEnumerable<CharacterBase> Members { get; set; }

        [Display(Name = "Invites")]
        public IEnumerable<GroupInviteBase> Invites { get; set; }
        public IEnumerable<GroupInviteBase> Applications { get; set; }
    }

    public class GroupEditForm
    {
        public GroupEditForm() { }
        public int Id { get; set; }
        public CharacterBase Leader { get; set; }
        public MultiSelectList CharList { get; set; }
        public IEnumerable<CharacterBase> Characters { get; set; }
    }
    public class GroupEditInfo
    {
        public GroupEditInfo() { }
        public int Id { get; set; }
        public IEnumerable<int> Characters { get; set; }
    }
}