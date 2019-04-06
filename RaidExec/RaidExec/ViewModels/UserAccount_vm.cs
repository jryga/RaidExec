using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    public class UserAccountBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name = "Most Played")]
        public string MostPlayed { get; set; }

        [Display(Name = "Image Url")]
        public string UrlImage { get; set; }

        [Display(Name = "Raids Completed")]
        public int RaidsCompleted { get; set; }
    }

    public class UserAccountHome : UserAccountBase
    {
        public UserAccountHome()
        {
            Games = new List<GameBase>();
            Characters = new List<CharacterDetails>();
        }

        public IEnumerable<GameBase> Games { get; set; }

        public ICollection<CharacterDetails> Characters { get; set; }
    }


    public class UserAccountEditForm : UserAccountBase
    {

    }

    public class UserAccountEditInfo
    {
        public UserAccountEditInfo()
        {

        }
        public int Id { get; set; }
        public string Role { get; set; }
        public string MostPlayed { get; set; }
        public string UrlImage { get; set; }

    }


    public class UserAccountGame
    {
        public int Id { get; set; }

        public int GameId { get; set; }

    }


    public class UserAccountAddGame : UserAccountGame
    {

        // Attention - SelectList for the associated item
        [Display(Name = "Game")]
        public SelectList GameList { get; set; }

        public string GameName { get; set; }
    }

    public class UserAccountAddGameInfo
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public string GameName { get; set; }

    }

    public class UserAccountCharacter
    {
        public UserAccountCharacter()
        {
            Characters = new List<CharacterBase>();
        }

        [Key]
        public int Id { get; set; }

        [Required, StringLength(100, MinimumLength = 2)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public IEnumerable<CharacterBase> Characters { get; set; }

    }



    public class UserAccountAddCharacter : UserAccountAddGame
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public string Class { get; set; }

        public string UrlImage { get; set; }


    }

    public class ChangePassword
    {
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}