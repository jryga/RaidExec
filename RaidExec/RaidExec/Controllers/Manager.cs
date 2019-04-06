using Microsoft.AspNet.Identity.EntityFramework;
using RaidExec.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using AutoMapper;

namespace RaidExec.Controllers
{
    public class Manager
    {
        private ApplicationDbContext ds;

        MapperConfiguration config;
        public IMapper mapper;

        public Manager()
        {
            ds = new ApplicationDbContext();

            // <From, To>
            config = new MapperConfiguration(cfg =>
            {

                // Raid
                cfg.CreateMap<Models.Raid, Controllers.RaidBase>();
                cfg.CreateMap<Models.Raid, Controllers.RaidDetails>();
                cfg.CreateMap<Models.Raid, Controllers.RaidSimpleAddForm>();
                cfg.CreateMap<Controllers.RaidSimpleAdd, Models.Raid>();
                cfg.CreateMap<Controllers.RaidBase, Controllers.RaidEditForm>();
                cfg.CreateMap<Controllers.RaidDetails, Controllers.RaidEditForm>();

                // Event
                cfg.CreateMap<Models.Event, Controllers.EventBase>();
                cfg.CreateMap<Models.Event, Controllers.EventDetails>();
                cfg.CreateMap<Models.Event, Controllers.EventAddForm>();
                cfg.CreateMap<Controllers.EventAdd, Models.Event>();
                cfg.CreateMap<Controllers.EventBase, Controllers.EventEditForm>();
                cfg.CreateMap<Controllers.EventAdd, Controllers.EventAddForm>();
                cfg.CreateMap<Controllers.EventBase, Controllers.EventRaidAddForm>();
                cfg.CreateMap<Controllers.EventRaidAdd, Controllers.EventRaidAddForm>();


                // Game
                cfg.CreateMap<Models.Game, Controllers.GameBase>();
                cfg.CreateMap<Models.Game, Controllers.GameDetails>();
                cfg.CreateMap<Controllers.GameAdd, Models.Game>();
                cfg.CreateMap<Controllers.GameBase, Controllers.GameEditForm>();
                //cfg.CreateMap<Models.Game, Controllers.GameEditForm>();

                // Character
                cfg.CreateMap<Models.Character, Controllers.CharacterBase>();
                cfg.CreateMap<Models.Character, Controllers.CharacterAdd>();
                cfg.CreateMap<Models.Character, Controllers.CharacterDetails>();
                cfg.CreateMap<Models.Character, Controllers.CharacterAddForm>();
                cfg.CreateMap<Models.Character, Controllers.CharacterGuild>();
                cfg.CreateMap<Controllers.CharacterAdd, Models.Character>();
                cfg.CreateMap<Controllers.CharacterBase, Controllers.CharacterEditForm>();
                cfg.CreateMap<Controllers.CharacterEditInfo, Controllers.CharacterEditForm>();

                // UserAccount
                cfg.CreateMap<Models.UserAccount, Controllers.UserAccountBase>();
                cfg.CreateMap<Controllers.UserAccountBase, Controllers.UserAccountAddCharacter>();
                cfg.CreateMap<Controllers.UserAccountBase, Controllers.UserAccountEditForm>();
                cfg.CreateMap<Models.UserAccount, Controllers.UserAccountHome>();

                // Group
                cfg.CreateMap<Models.Group, Controllers.GroupBase>();
                cfg.CreateMap<Models.Group, Controllers.GroupDetails>();
                

                // Guild
                cfg.CreateMap<Models.Guild, Controllers.GuildBase>();
                cfg.CreateMap<Models.Guild, Controllers.GuildAdd>();
                cfg.CreateMap<Controllers.GuildAdd, Controllers.GuildAddForm>();
                cfg.CreateMap<Models.Guild, Controllers.GuildFind>().ForMember(dest => dest.Username,
                     opts => opts.MapFrom(src => src.Leader.UserAccount.Name));
                cfg.CreateMap<Models.Guild, Controllers.GuildDetails>();
                cfg.CreateMap<Controllers.GuildDetails, Controllers.GuildEditForm>();
                cfg.CreateMap<Controllers.GuildAdd, Models.Guild>();

                // Invites
                cfg.CreateMap<Models.GuildInvite, Controllers.GuildInviteBase>();
                cfg.CreateMap<Models.GroupInvite, Controllers.GroupInviteBase>();
                cfg.CreateMap<Models.RaidInvite, Controllers.RaidInviteBase>();


            });

            mapper = config.CreateMapper();
        }


        /// <summary>
        /// 
        /// Game functions
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        public string CancelGuildInv(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How did you do this?";
            }
            var inv = ds.GuildInvitations.Include("Character.GuildInvites").Include("Guild.Invites").Include("Guild.Leader.UserAccount.ApplicationUser").SingleOrDefault(i => i.Id == id);
            if (inv == null)
            {
                return "Can't find the invite";
            }
            if(inv.Guild.Leader.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                return "You are not the leader";
            }
            inv.Character.GuildInvites.Remove(inv);
            inv.Guild.Invites.Remove(inv);
            ds.GuildInvitations.Remove(inv);
            ds.SaveChanges();
            return "Invite cancelled";

        }
        public string CancelRaidInv(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How did you do this?";
            }
            var inv = ds.RaidInvitations.Include("Character.RaidInvites").Include("Raid.Invites").Include("Raid.Leader.UserAccount.ApplicationUser").SingleOrDefault(i => i.Id == id);
            if (inv == null)
            {
                return "Can't find the invite";
            }
            if (inv.Raid.Leader.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                return "You are not the leader";
            }
            inv.Character.RaidInvites.Remove(inv);
            inv.Raid.Invites.Remove(inv);
            ds.RaidInvitations.Remove(inv);
            ds.SaveChanges();
            return "Invite cancelled";

        }
        public string CancelGroupInv(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How did you do this?";
            }
            var inv = ds.GroupInvitations.Include("Character.GroupInvites").Include("Group.Invites").Include("Group.Leader.UserAccount.ApplicationUser").SingleOrDefault(i => i.Id == id);
            if (inv == null)
            {
                return "Can't find the invite";
            }
            if (inv.Group.Leader.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                return "You are not the leader";
            }
            inv.Character.GroupInvites.Remove(inv);
            inv.Group.Invites.Remove(inv);
            ds.GroupInvitations.Remove(inv);
            ds.SaveChanges();
            return "Invite cancelled";
        }
        public string CancelGuildApp(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return  "How did you do this?";
            }
            var app = ds.GuildInvitations.Include("Character.UserAccount.ApplicationUser").Include("Guild.Applications").SingleOrDefault(i => i.Id == id);
            if(app == null)
            {
                return "Can't find the application";
            }
            if(app.Character.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                return "This isn't your application";
            }
            app.Guild.Applications.Remove(app);
            app.Character.GuildApplications.Remove(app);
            ds.GuildInvitations.Remove(app);
            ds.SaveChanges();
            return "Application Cancelled";

        }
        public string CancelRaidApp(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How did you do this?";
            }
            var app = ds.RaidInvitations.Include("Character.UserAccount.ApplicationUser").Include("Raid.Applications").SingleOrDefault(i => i.Id == id);
            if (app == null)
            {
                return "Can't find the application";
            }
            if (app.Character.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                return "This isn't your application";
            }
            app.Raid.Applications.Remove(app);
            app.Character.RaidApplications.Remove(app);
            ds.RaidInvitations.Remove(app);
            ds.SaveChanges();
            return "Application Cancelled";

        }
        public string CancelGroupApp(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How did you do this?";
            }
            var app = ds.GroupInvitations.Include("Character.UserAccount.ApplicationUser").Include("Group.Applications").SingleOrDefault(i => i.Id == id);
            if (app == null)
            {
                return "Can't find the application";
            }
            if (app.Character.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                return "This isn't your application";
            }
            app.Group.Applications.Remove(app);
            app.Character.GroupApplications.Remove(app);
            ds.GroupInvitations.Remove(app);
            ds.SaveChanges();
            return "Application Cancelled";

        }

        public string GuildAccept(int? id)
        {
            string message = null;

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                message = "How did you do this?";
            }

            var inv = ds.GuildInvitations.Include("Character.Guild").Include("Character.GuildInvites").Include("Character.UserAccount.ApplicationUser").
                Include("Guild.Invites").SingleOrDefault(e => e.Id == id);

            if (message == null && inv.Character.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                message = "This invite doesn't belong to you";
            }

            if (message == null && inv == null)
            {
                message = "This invite doesn't exist";
            }

            if (message == null && inv.Character.Guild != null)
            {
                message = "This character is already in a guild";
            }
           
            if (message == null)
            {
                message = "Accepted";
                inv.Character.Guild = inv.Guild;
                inv.Guild.Members.Add(inv.Character);
            }
            //Find the character
            var character = ds.Characters.Include("GuildInvites.Guild.Invites").Include("GuildApplications.Guild.Applications").SingleOrDefault(i => i.Id == inv.Character.Id);
            //Remove his invites from all guilds
            foreach (var g in character.GuildInvites)
            {
                g.Guild.Invites.Remove(g);
            }
            //Remove his guild invites
            character.GuildInvites.Clear();
            //Remove his applications to all guilds
            foreach (var g in character.GuildApplications)
            {
                g.Guild.Applications.Remove(g);
            }
            //Remove his applications
            character.GuildApplications.Clear();
            //Delete all guild apps/invites from the character
            ds.GuildInvitations.RemoveRange(character.GuildApplications);
            ds.GuildInvitations.RemoveRange(character.GuildInvites);
            ds.GuildInvitations.Remove(inv);
            ds.SaveChanges();

            return message;

            // SEND MESSAGE TO CHARACTER AND GUILD IN THIS TOO
        }

        public string GroupKick(int? id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How did you do this?";
            }
            var kick = ds.Characters.Include("Group.Leader.UserAccount.ApplicationUser").SingleOrDefault(i => i.Id == id);
            if (kick == null || kick.Group == null)
            {
                return "Character not in the group";
            }
            if (kick.Group.Leader.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                return "You can't do this";
            }
            var group = ds.Groups.Include("Members").SingleOrDefault(i => i.Id == kick.Group.Id);
            kick.Group = null;
            group.Members.Remove(kick);
            ds.SaveChanges();
            return kick.Name + " has been kicked from group";
        }
        public string GuildKick(int? id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How did you do this?";
            }
            var kick = ds.Characters.Include("Guild.Leader.UserAccount.ApplicationUser").SingleOrDefault(i => i.Id == id);
            if(kick == null || kick.Guild == null)
            {
                return "Character not in the guild";
            }
            if(kick.Guild.Leader.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                return "You can't do this";
            }
            var guild = ds.Guilds.Include("Members").SingleOrDefault(i => i.Id == kick.Guild.Id);
            kick.Guild = null;
            guild.Members.Remove(kick);
            ds.SaveChanges();
            return kick.Name + " has been kicked from guild";
        }
        public string RaidKick(int cid, int rid)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How did you do this?";
            }
            var kick = ds.Characters.Include("Raids.Leader.UserAccount.ApplicationUser").SingleOrDefault(i => i.Id == cid);
            if (kick == null)
            {
                return "Character not found";
            }
            if(kick.Raids.Count == 0) {
                return "Character isn't in any raids";
            }
            var r = kick.Raids.SingleOrDefault(i => i.Id == rid);

            if (r.Leader.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                return "You can't do this";
            }
            
            kick.Raids.Remove(r);
            r.Members.Remove(kick);
            ds.SaveChanges();
            return kick.Name + " has been kicked from raid";
        }
        public string GuildDecline(int? id)
        {
            string message = null;

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                message = "How did you do this?";
            }

            var inv = ds.GuildInvitations.Include("Character.Guild").Include("Character.GuildInvites").Include("Character.UserAccount.ApplicationUser").
                Include("Guild.Invites").SingleOrDefault(e => e.Id == id);

            if (message == null && inv.Character.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                message = "This invite doesn't belong to you";
            }

            if (message == null && inv == null)
            {
                message = "This invite doesn't exist";
            }

            inv.Character.GuildInvites.Remove(inv);
            //inv.Guild.Invites.Remove(inv);
            inv.Declined = true;
            //ds.GuildInvitations.Remove(inv);
            ds.SaveChanges();

            return (message == null) ? "Declined" : message;

            // SEND MESSAGE TO CHARACTER AND GUILD IN THIS TOO
        }

        public string GuildApply(int? gid, int? cid)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }

            var g = ds.Guilds.Include("Applications.Character").SingleOrDefault(e => e.Id == gid);
            var c = ds.Characters.Include("Guild").Include("UserAccount.ApplicationUser").SingleOrDefault(e => e.Id == cid);

            if (g == null || c == null || 
                HttpContext.Current.User.Identity.Name != c.UserAccount.Name)
            {
                return "Random useless error";
            }

            if (c.Guild != null)
            {
                return "You are already in a guild";
            }

            if (g.Applications.Select(e => e.Character.Id).Contains(c.Id))
            {
                return "You are already applied to this guild";
            }

            var app = new GuildInvite
            {
                Character = c,
                Guild = g
            };
            ds.GuildInvitations.Add(app);
            c.GuildApplications.Add(app);
            g.Applications.Add(app);
            ds.SaveChanges();

            return "You have applied";
        }
        public string GroupApply(int? gid, int? cid)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }

            var g = ds.Groups.Include("Applications.Character").SingleOrDefault(e => e.Id == gid);
            var c = ds.Characters.Include("Group").Include("UserAccount.ApplicationUser").SingleOrDefault(e => e.Id == cid);

            if (g == null || c == null ||
                HttpContext.Current.User.Identity.Name != c.UserAccount.Name)
            {
                return "Random useless error";
            }

            if (c.Group != null)
            {
                return "You are already in a group";
            }

            if (g.Applications.Select(e => e.Character.Id).Contains(c.Id))
            {
                return "You are already applied to this group";
            }

            var app = new GroupInvite
            {
                Character = c,
                Group = g
            };
            ds.GroupInvitations.Add(app);
            c.GroupApplications.Add(app);
            g.Applications.Add(app);
            ds.SaveChanges();

            return "You have applied";
        }
        public UserAccountHome UserAccountGetHomeByName(string name)
        {
            var s = ds.Characters.Include("Guild");

            var o = ds.UserAccounts.Include("Characters.Game").Include("Characters.GuildInvites.Guild.Leader").Include("Characters.GuildApplications.Guild.Leader").
                Include("Characters.GroupInvites.Group.Leader").Include("Characters.RaidInvites.Raid.Leader").Include("Characters.Raids.Event.Game").Include("Characters.Raids.Leader").
                Include("Characters.GuildApplications.Guild.Leader").Include("Characters.GroupApplications.Group.Leader").Include("Characters.RaidApplications.Raid.Leader").
                Include("Characters.Guild.Leader").Include("Characters.Group.Leader").Include("ApplicationUser").Include("Characters.RaidInvites.Raid.Event.Game").
                Include("Characters.RaidApplications.Raid.Event.Game").SingleOrDefault(e => e.ApplicationUser.UserName == name);

            // Return the result, or null if not found
            return (o == null) ? null : mapper.Map<UserAccountHome>(o);
        }

        public UserAccountHome UserAccountGetHomeById(int? id)
        {
            var s = ds.Characters.Include("Guild");

            var o = ds.UserAccounts.Include("Characters.Game").Include("Characters.GuildInvites.Guild.Leader").Include("Characters.GuildApplications.Guild.Leader").
                Include("Characters.GroupInvites.Group.Leader").Include("Characters.RaidInvites.Raid.Leader").Include("Characters.Raids.Event.Game").Include("Characters.Raids.Leader").
                Include("Characters.GuildApplications.Guild.Leader").Include("Characters.GroupApplications.Group.Leader").Include("Characters.RaidApplications.Raid.Leader").
                Include("Characters.Guild.Leader").Include("Characters.Group.Leader").Include("ApplicationUser").Include("Characters.RaidInvites.Raid.Event.Game").
                Include("Characters.RaidApplications.Raid.Event.Game").SingleOrDefault(e => e.Id == id);

            // Return the result, or null if not found
            return (o == null) ? null : mapper.Map<UserAccountHome>(o);
        }

        public UserAccountBase UserAccountGetById(int? id)
        {
            var o = ds.UserAccounts.Include("Applicationuser").SingleOrDefault(e => e.Id == id);

            return (o == null) ? null : mapper.Map<UserAccountBase>(o);
        }
        public UserAccountBase UserAccountGetByName(string name)
        {
            var o = ds.UserAccounts.Include("Applicationuser").SingleOrDefault(e => e.ApplicationUser.UserName == name);

            return (o == null) ? null : mapper.Map<UserAccountBase>(o);
        }
        public UserAccountBase UserAccountByCharacterId(int? id)
        {
            var o = ds.Characters.Include("UserAccount.Characters.Game").Include("UserAccount.ApplicationUser").SingleOrDefault(i => i.Id == id);
            return (o == null) ? null : mapper.Map<UserAccountBase>(o.UserAccount);
        }
        public UserAccountBase UserAccountGetByIdWithChar(int? id)
        {
            var x = ds.UserAccounts.Include("Characters").SingleOrDefault(a => a.Id == id);
            return (x == null) ? null : mapper.Map<UserAccountBase>(x);
        }


        public UserAccountBase EditUserAccountBase(UserAccountEditInfo newItem)
        {
            var o = ds.UserAccounts.Include("Applicationuser").SingleOrDefault(e => e.Id == newItem.Id);

            if (o == null)
            {
                // Problem - item was not found, so return
                return null;
            }
            else
            {
                // Update the object with the incoming values
                ds.Entry(o).CurrentValues.SetValues(newItem);
                ds.SaveChanges();

                // Prepare and return the object
                return mapper.Map<UserAccountBase>(o);
            }
        }


        public CharacterBase CharacterCreate(UserAccountAddCharacter newItem, string name)
        {
            var o = ds.UserAccounts.Include("Applicationuser").FirstOrDefault(a => a.ApplicationUser.UserName == name);
            //var o = ds.UserAccounts.Include("Applicationuser").SingleOrDefault(e => e.ApplicationUser.UserName == newItem.Name);
            var found = ds.Games.Find(newItem.GameId);

            if (o == null)
            {
                // Problem - item was not found, so return
                return null;
            }
            else
            {

                var NewChar = new Character();

                NewChar.Name = newItem.Name;
                NewChar.Level = newItem.Level;
                NewChar.Class = newItem.Class;
                NewChar.UrlImage = newItem.UrlImage;
                NewChar.Game = found;
                NewChar.UserAccount = o;


                // Update the object with the incoming values
                var addedItem = ds.Characters.Add(NewChar);
                o.Characters.Add(NewChar);
                found.Characters.Add(addedItem);
                ds.SaveChanges();

                // Prepare and return the object
                return mapper.Map<CharacterBase>(addedItem);
            }
        }

        public CharacterBase EditCharacter(CharacterEditInfo newItem)
        {
            var o = ds.Characters.Include("UserAccount").Include("UserAccount.Applicationuser").Include("Game").SingleOrDefault(e => e.Id == newItem.Id);

            if (o == null)
            {
                // Problem - item was not found, so return
                return null;
            }
            else
            {
                // Update the object with the incoming values
                ds.Entry(o).CurrentValues.SetValues(newItem);
                ds.SaveChanges();

                // Prepare and return the object
                return mapper.Map<CharacterBase>(o);
            }
        }


        public bool CharacterDelete(int userid, int charid)
        {
            // Attempt to fetch the object to be deleted
            var account = ds.UserAccounts.Include("Characters").Include("Applicationuser").SingleOrDefault(e => e.Id == userid);

            var itemToDelete = ds.Characters.Include("Game").Include("Group").Include("Guild").Include("RaidInvites").Include("GroupInvites").
                Include("RaidApplications").Include("GroupApplications").Include("GuildApplications").Include("GuildInvites.Guild.Invites").
                Include("GuildApplications.Guild.Applications").Include("Raids.Leader").Include("RaidApplications.Raid.Invites").Include("RaidInvites.Raid.Invites").
                Include("GroupApplications.Group.Invites").Include("GroupInvites.Group.Invites").SingleOrDefault(e => e.Id == charid);
            if (account == null || itemToDelete == null || itemToDelete.UserAccount != account)
            {
                return false;
            }
            else
            {
                if (itemToDelete.Group != null)
                {


                    if (itemToDelete.Group.Leader != itemToDelete)
                    {
                        GroupLeave(charid);
                    }
                    else
                    {
                        return false;
                    }
                }

                if (itemToDelete.Guild != null)
                {

                    if (itemToDelete.Guild.Leader != itemToDelete)
                    {
                        GuildLeave(charid);
                    }
                    else
                    {
                        return false;
                    }
                }

                foreach (Raid item in itemToDelete.Raids)
                {
                    if (item.Leader != itemToDelete)
                    {
                        RaidLeave(charid, item.Id);
                    }
                    else
                    {
                        return false;
                    }
                }

                foreach (var g in itemToDelete.GuildInvites)
                {
                    g.Guild.Invites.Remove(g);
                }
                //Remove his guild invites
                itemToDelete.GuildInvites.Clear();
                //Remove his applications to all guilds
                foreach (var g in itemToDelete.GuildApplications)
                {
                    g.Guild.Applications.Remove(g);
                }
                //Remove his applications
                itemToDelete.GuildApplications.Clear();
                //Delete all guild apps/invites from the character
                ds.GuildInvitations.RemoveRange(itemToDelete.GuildApplications);
                ds.GuildInvitations.RemoveRange(itemToDelete.GuildInvites);

                foreach (var item in itemToDelete.RaidApplications)
                {
                    item.Raid.Invites.Remove(item);
                }

                //clear raid aplications
                itemToDelete.RaidApplications.Clear();

                foreach (var item in itemToDelete.RaidInvites)
                {
                    item.Raid.Invites.Remove(item);
                }
                itemToDelete.RaidInvites.Clear();

                ds.RaidInvitations.RemoveRange(itemToDelete.RaidApplications);
                ds.RaidInvitations.RemoveRange(itemToDelete.RaidInvites);

                //clear group invites
                foreach (var item in itemToDelete.GroupApplications)
                {
                    item.Group.Invites.Remove(item);
                }
                itemToDelete.GroupApplications.Clear();

                foreach (var item in itemToDelete.GroupInvites)
                {
                    item.Group.Invites.Remove(item);
                }
                itemToDelete.GroupInvites.Clear();

                ds.GroupInvitations.RemoveRange(itemToDelete.GroupApplications);
                ds.GroupInvitations.RemoveRange(itemToDelete.GroupInvites);

                account.Characters.Remove(itemToDelete);
                ds.Characters.Remove(itemToDelete);
                ds.SaveChanges();

                return true;
            }
        }

        public IEnumerable<CharacterBase> CharacterGetAllForUser(string name)
        {
            var a = ds.UserAccounts.Include("ApplicationUser").Include("Characters.Game").SingleOrDefault(e => e.ApplicationUser.UserName == name);

            return mapper.Map<IEnumerable<CharacterBase>>(a.Characters);
        }

        public IEnumerable<CharacterBase> CharacterGetAllForUserByGame(string name, int? id)
        {
            var a = ds.UserAccounts.Include("ApplicationUser").Include("Characters.Game").SingleOrDefault(e => e.ApplicationUser.UserName == name);
            IEnumerable<Character> c = new List<Character>();

            if (a != null)
            {
                c = a.Characters.Where(e => e.Game.Id == id.GetValueOrDefault());
            }

            return mapper.Map<IEnumerable<CharacterBase>>(c);
        }

        public GameBase GameGetById(int? id)
        {
            var o = ds.Games.Find(id);
            return (o == null) ? null : mapper.Map<GameBase>(o);
        }

        public GameDetails GameDetailsGetById(int? id)
        {
            var o = ds.Games.Include("Events.Creator.ApplicationUser").Include("Characters.UserAccount.ApplicationUser").Include("Raids.Leader.UserAccount").SingleOrDefault(e => e.Id == id);
            return (o == null) ? null : mapper.Map<GameDetails>(o);
        }

        public IEnumerable<GameBase> GameGetAll()
        {
            return mapper.Map<IEnumerable<GameBase>>(ds.Games.OrderBy(e => e.Name));
        }

        public IEnumerable<GameBase> GameGetAllByAccountName(string name)
        {
            ICollection<Game> o = null;
            var a = ds.UserAccounts.Include("Characters.Game").SingleOrDefault(e => e.ApplicationUser.UserName == name);

            if (a != null)
            {
                o = new List<Game>();

                foreach (var c in a.Characters)
                {
                    if (!o.Contains(c.Game))
                    {
                        o.Add(c.Game);
                    }
                }
            }

            return mapper.Map<IEnumerable<GameBase>>(o.OrderBy(e => e.Name));
        }

        /// <summary>
        /// 
        /// Raid functions
        /// 
        /// </summary>
        /// <returns></returns>

        public RaidBase RaidGetById(int? id)
        {
            var o = ds.Raids.Include("Event.Game").Include("Leader.UserAccount.ApplicationUser").Include("Members").SingleOrDefault(e => e.Id == id);
            return (o == null) ? null : mapper.Map<RaidBase>(o);
        }

        public RaidDetails RaidDetailsGetById(int? id)
        {
            var o = ds.Raids.Include("Event.Game").Include("Applications.Character").Include("Invites.Character").Include("Leader.UserAccount.ApplicationUser").Include("Members").SingleOrDefault(e => e.Id == id);
            return (o == null) ? null : mapper.Map<RaidDetails>(o);
        }

        public IEnumerable<RaidBase> RaidGetAll()
        {
            return mapper.Map<IEnumerable<RaidBase>>(ds.Raids.Include("Event.Game").Include("Leader.UserAccount.ApplicationUser").Include("Members"));
        }


        public RaidBase RaidAdd(EventRaidAdd newItem)
        {
            var o = new Raid
            {
                Leader = ds.Characters.Find(newItem.LeaderId),
                Event = ds.Events.Find(newItem.Id),
                ScheduledTime = newItem.ScheduledTime                
            };

            if (o.Leader != null && o.Event != null)
            {
                o.Leader.Raids.Add(o);
                ds.Raids.Add(o);
                ds.SaveChanges();
            }
            
            return (o == null) ? null : mapper.Map<RaidBase>(o);
        }

        public RaidBase RaidEdit(RaidEdit newItem)
        {
            var o = ds.Raids.Include("Event.Game").Include("Leader.UserAccount.ApplicationUser").SingleOrDefault(e => e.Id == newItem.Id);
            
            if (o != null)
            {
                if (o.Leader.UserAccount.Name != HttpContext.Current.User.Identity.Name && !HttpContext.Current.User.IsInRole("Admin"))
                {
                    throw new Exception("You can not edit this raid.");
                }

                ds.Entry(o).CurrentValues.SetValues(newItem);
                ds.SaveChanges();
            }

            return (o == null) ? null : mapper.Map<RaidBase>(o);
        }

        public bool RaidRemove(int? id)
        {
            var o = ds.Raids.Include("Leader.UserAccount.ApplicationUser").Include("Event.Raids").SingleOrDefault(e => e.Id == id);

            bool found = (o != null) ? true : false;
            if (found)
            {
                if (o.Leader.UserAccount.Name != HttpContext.Current.User.Identity.Name && !HttpContext.Current.User.IsInRole("Admin"))
                {
                    throw new Exception("You can not remove this raid.");
                }

                // Remove event if it is inactive
                if (o.Event.Raids.Count == 1 && !o.Event.Active)
                {
                    ds.Events.Remove(o.Event);
                }

                ds.Raids.Remove(o);
                ds.SaveChanges();
            }

            return found;
        }

        /// <summary>
        /// 
        /// Event functions
        /// 
        /// </summary>
        /// <returns></returns>

        public EventBase EventGetById(int? id)
        {
            var o = ds.Events.Include("Game").Include("Creator.ApplicationUser").SingleOrDefault(e => e.Id == id);
            return (o == null) ? null : mapper.Map<EventBase>(o);
        }

        public EventDetails EventDetailsGetById(int? id)
        {
            var o = ds.Events.Include("Game").Include("Creator.ApplicationUser").Include("Raids.Leader").Include("Raids.Members").SingleOrDefault(e => e.Id == id);
            return (o == null) ? null : mapper.Map<EventDetails>(o);
        }

        public IEnumerable<EventBase> EventGetAll()
        {
            return mapper.Map<IEnumerable<EventBase>>(ds.Events.Include("Game").Include("Creator.ApplicationUser"));
        }

        public IEnumerable<EventBase> EventGetAllActive()
        {
            return mapper.Map<IEnumerable<EventBase>>(ds.Events.Include("Game").Include("Creator.ApplicationUser").Where(e => e.Active));
        }

        public EventIndex EventGetIndex()
        {
            var index = new EventIndex();

            index.Events = mapper.Map<IEnumerable<EventBase>>(ds.Events.Include("Game").Include("Creator.ApplicationUser").Where(e => e.Active));
            index.GameIds = GameGetAllByAccountName(HttpContext.Current.User.Identity.Name).Select(e => e.Id);

            return index;
        }

        public EventBase EventAdd(EventAdd newItem)
        {
            var o = (newItem == null) ? null : mapper.Map<Event>(newItem);
            Game g = null;

            if (o != null)
            {
                g = ds.Games.Find(newItem.GameId);
            }

            if (o != null && g != null)
            {
                o.Name = o.Name.Trim();
                o.Description = o.Description.Trim();
                o.DateCreated = DateTime.Now;
                o.Creator = ds.UserAccounts.Include("ApplicationUser").SingleOrDefault(e => e.ApplicationUser.UserName == HttpContext.Current.User.Identity.Name);
                o.Game = g;

                o.Creator.Events.Add(o);
                ds.Events.Add(o);
                ds.SaveChanges();
            }

            return (o == null) ? null : mapper.Map<EventBase>(o);
        }
        public GameBase GameAdd(GameAdd newItem)
        {
            var o = (newItem == null) ? null : mapper.Map<Game>(newItem);

            if (o != null)
            {
                o.Name = o.Name.Trim();
                o.Description = o.Description.Trim();
                
                ds.Games.Add(o);
                ds.SaveChanges();
            }

            return (o == null) ? null : mapper.Map<GameBase>(o);
        }

        public EventBase EventEdit(EventEdit newItem)
        {
            var o = ds.Events.Include("Creator.ApplicationUser").SingleOrDefault(e => e.Id == newItem.Id);

            if (o != null)
            {
                if (o.Creator.Name != HttpContext.Current.User.Identity.Name && !HttpContext.Current.User.IsInRole("Admin"))
                {
                    throw new Exception("You can not edit this event.");
                }

                ds.Entry(o).CurrentValues.SetValues(newItem);
                ds.SaveChanges();
            }

            return (o == null) ? null : mapper.Map<EventBase>(o);
        }
        public GameBase GameEdit(GameEditInfo newItem)
        {
            var o = ds.Games.SingleOrDefault(e => e.Id == newItem.Id);

            if (o != null)
            {
                ds.Entry(o).CurrentValues.SetValues(newItem);
                ds.SaveChanges();
            }

            return (o == null) ? null : mapper.Map<GameBase>(o);
        }

        public bool EventRemove(int? id)
        {
            var o = ds.Events.Include("Creator.ApplicationUser").Include("Raids").SingleOrDefault(e => e.Id == id);

            bool found = (o != null) ? true : false;
            if (found)
            {
                if (o.Creator.Name != HttpContext.Current.User.Identity.Name && !HttpContext.Current.User.IsInRole("Admin"))
                {
                    throw new Exception("You can not remove this event.");
                }


                // Remove event if there are no raids else mark it as inactive
                if (o.Raids.Count == 0)
                {
                    ds.Events.Remove(o);
                }
                else
                {
                    o.Active = false;
                }

                ds.SaveChanges();
            }

            return found;
        }

        /// <summary> ///////////////////////////////////////////////////////////////////////////////////////
        /// 
        /// Matt's functions
        /// 
        /// </summary>
        /// <returns></returns> /////////////////////////////////////////////////////////////////////////////
        /// 
        public String GroupLeave(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var c = ds.Characters.Include("UserAccount.ApplicationUser").Include("Group").SingleOrDefault(e => e.Id == id);
            if (c.Group == null)
            {
                return "You are not in a group";
            }

            var g = ds.Groups.Include("Members").Include("Invites.Character").Include("Applications.Character").Include("Leader").SingleOrDefault(e => e.Id == c.Group.Id);


            if (g == null || c == null ||
                HttpContext.Current.User.Identity.Name != c.UserAccount.Name)
            {
                return "Random useless error";
            }
            c.Group = null;
            g.Members.Remove(c);
            ds.SaveChanges();

            return "You have left the group";
        }
        public IEnumerable<GroupBase> GroupGetAllByGame(int id)
        {
            var o = ds.Groups.Include("Members").Include("Leader").Where(i => i.Game.Id == id);
            return mapper.Map<IEnumerable<GroupBase>>(o);
        }
        public String AddGroup(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var c = ds.Characters.Include("Game").Include("Group").Include("GroupApplications").Include("UserAccount.ApplicationUser").Include("GroupInvites").SingleOrDefault(i => i.Id == id);
            if (c == null || HttpContext.Current.User.Identity.Name != c.UserAccount.Name)
            {
                return "Random useless error";
            }
            if(c.Group != null)
            {
                return "Already in a group";
            }
            c.GroupApplications.Clear();
            c.GroupInvites.Clear();
            var group = new Group { Leader = c, Game = c.Game };
            c.Group = group;
            ds.Groups.Add(group);
            ds.SaveChanges();
            return "Created Group";
        }
        public IEnumerable<CharacterBase> CharacterGetAll()
        {
            var o = ds.Characters.Include("Game").Include("UserAccount.ApplicationUser");
            return mapper.Map<IEnumerable<CharacterBase>>(o);
        }

        public GroupDetails getGroupById(int id)
        {
            return mapper.Map<GroupDetails>(ds.Groups.Include("Invites.Character").Include("Applications.Character").Include("Members.UserAccount.ApplicationUser").Include("Leader.UserAccount.ApplicationUser").Include("Leader.Game").Include("Game").SingleOrDefault(a => a.Id == id));
        }
        public string RaidAccept(int? id)
        {
            string message = null;

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                message = "How did you do this?";
            }

            var inv = ds.RaidInvitations.Include("Character.RaidInvites").Include("Character.UserAccount.ApplicationUser").
                Include("Raid.Invites").SingleOrDefault(e => e.Id == id);

            if (message == null && inv.Character.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                message = "This invite doesn't belong to you";
            }

            if (message == null && inv == null)
            {
                message = "This invite doesn't exist";
            }

            if (message == null)
            {
                message = "Accepted";
                inv.Character.Raids.Add(inv.Raid);
                inv.Raid.Members.Add(inv.Character);
            }
            //Delete the raid invite
            inv.Raid.Invites.Remove(inv);
            inv.Character.RaidInvites.Remove(inv);
            ds.RaidInvitations.Remove(inv);
            ds.SaveChanges();

            return message;

            // SEND MESSAGE TO CHARACTER AND GUILD IN THIS TOO
        }
        public string ApplyGroupToRaid(RaidApplicationInfo item)
        {
            var raid = ds.Raids.Include("Event.Game").Include("Applications.Character").Include("Invites.Character").SingleOrDefault(i => i.Id == item.Id);
            var character = ds.Characters.Include("UserAccount.ApplicationUser").Include("Game").Include("RaidApplications.Raid").Include("RaidInvites.Raid").Include("Group.Members").SingleOrDefault(i => i.Id == item.CharacterId);
            if (raid == null)
                return "Raid not found";
            if (character == null)
                return "Character not found";
            if (character.UserAccount.Name != HttpContext.Current.User.Identity.Name)
                return "This character doesn't belong to you";
            var group = ds.Groups.Include("Members.RaidApplications").SingleOrDefault(i => i.Id == character.Group.Id);

            if (group != null)
            {
                foreach (var m in group.Members)
                {
                    var hasInv = false;
                    var hasApp = false;
                    //Check if user has application or invite
                    var checkInvite = raid.Invites.SingleOrDefault(i => i.Character.Id == m.Id);
                    hasInv = checkInvite != null;

                    var checkApplication = raid.Applications.SingleOrDefault(i => i.Character.Id == m.Id);
                    hasApp = checkApplication != null;
                    if (!hasApp && !hasInv)
                    {
                        var application = new RaidInvite();
                        application.Raid = raid;
                        application.Character = m;

                        raid.Applications.Add(application);
                        m.RaidApplications.Add(application);
                        
                        ds.RaidInvitations.Add(application);
                        ds.SaveChanges();
                    }
                }
                
            }
            
            return "Group application sent";
        }
        public string ApplyToRaid(RaidApplicationInfo item)
        {
            var raid = ds.Raids.Include("Event.Game").Include("Applications.Character").Include("Invites.Character").SingleOrDefault(i => i.Id == item.Id);
            var character = ds.Characters.Include("UserAccount.ApplicationUser").Include("Game").Include("RaidApplications.Raid").Include("RaidInvites.Raid").SingleOrDefault(i => i.Id == item.CharacterId && i.Game.Id == raid.Game.Id);

            if (raid == null)
                return "Raid not found";
            if (character == null)
                return "Character not found";
            if (character.UserAccount.Name != HttpContext.Current.User.Identity.Name)
                return "This character doesn't belong to you";

            //Check if user has application or invite
            var checkInvite = raid.Invites.SingleOrDefault(i => i.Character.Id == character.Id);
            if (checkInvite != null)
                return "Character has an invite already";

            var checkApplication = raid.Applications.SingleOrDefault(i => i.Character.Id == character.Id);
            if (checkApplication != null)
                return "Character has already applied";

            var application = new RaidInvite();
            application.Raid = raid;
            application.Character = character;

            raid.Applications.Add(application);
            character.RaidApplications.Add(application);

            ds.RaidInvitations.Add(application);
            ds.SaveChanges();
            return character.Name + " applied to " + raid.Event.Name ;
        }
        public IEnumerable<CharacterBase> getMyGroupCharacters(string name, int rId)
        {
            var raid = ds.Raids.Include("Event.Game").SingleOrDefault(i => i.Id == rId);
            var characters = ds.Characters.Include("UserAccount.ApplicationUser").Include("Group.Leader").Where(i => i.Game.Id == raid.Game.Id);
            var list = new List<CharacterBase>();
            foreach(var c in characters)
            {
                if( c.UserAccount.Name == name && c.Group != null && c.Group.Leader.Id == c.Id)
                {
                    list.Add(mapper.Map<CharacterBase>(c));
                }
            }
            return list;
        }

        public IEnumerable<CharacterBase> getMyCharacters(string name, int rId)
        {
            var raid = ds.Raids.Include("Event.Game").SingleOrDefault(i => i.Id == rId);
            var characters = ds.Characters.Include("UserAccount.ApplicationUser").Where(i => i.Game.Id == raid.Game.Id);
            var list = new List<CharacterBase>();
            foreach(var c in characters)
            {
                if (c.UserAccount.Name == name) {
                    //check if user applied or has invite
                    var applied = ds.RaidInvitations.Where(i => i.Character.Id == c.Id && i.Raid.Id == raid.Id);
                    if (applied.Count() == 0)
                    {
                        list.Add(mapper.Map<CharacterBase>(c));
                    }
                }
            }
            return list;
        }
        public string RaidDecline(int? id)
        {
            string message = null;

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                message = "How did you do this?";
            }

            var inv = ds.RaidInvitations.Include("Character.RaidInvites").Include("Character.UserAccount.ApplicationUser").
                Include("Raid.Invites").SingleOrDefault(e => e.Id == id);

            if (message == null && inv.Character.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                message = "This invite doesn't belong to you";
            }

            if (message == null && inv == null)
            {
                message = "This invite doesn't exist";
            }

            inv.Character.RaidInvites.Remove(inv);
            inv.Declined = true;
            ds.SaveChanges();

            return (message == null) ? "Declined" : message;

            // SEND MESSAGE TO CHARACTER AND GUILD IN THIS TOO
        }
        public string GroupAccept(int? id)
        {
            string message = null;

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                message = "How did you do this?";
            }

            var inv = ds.GroupInvitations.Include("Character.Group").Include("Character.GroupInvites").Include("Character.UserAccount.ApplicationUser").
                Include("Group.Invites").SingleOrDefault(e => e.Id == id);

            if (message == null && inv.Character.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                message = "This invite doesn't belong to you";
            }

            if (message == null && inv == null)
            {
                message = "This invite doesn't exist";
            }

            if (message == null && inv.Character.Guild != null)
            {
                message = "This character is already in a guild";
            }

            if (message == null)
            {
                message = "Accepted";
                inv.Character.Group = inv.Group;
                inv.Group.Members.Add(inv.Character);
            }
            //Find the character
            var character = ds.Characters.Include("GroupInvites.Group.Invites").Include("GroupApplications.Group.Applications").SingleOrDefault(i => i.Id == inv.Character.Id);
            //Remove his invites from all guilds
            foreach (var g in character.GroupInvites)
            {
                g.Group.Invites.Remove(g);
            }
            //Remove his guild invites
            character.GroupInvites.Clear();
            //Remove his applications to all guilds
            foreach (var g in character.GroupApplications)
            {
                g.Group.Applications.Remove(g);
            }
            //Remove his applications
            character.GroupApplications.Clear();
            //Delete all guild apps/invites from the character
            ds.GroupInvitations.RemoveRange(character.GroupApplications);
            ds.GroupInvitations.RemoveRange(character.GroupInvites);
            ds.GroupInvitations.Remove(inv);
            ds.SaveChanges();

            return message;

            // SEND MESSAGE TO CHARACTER AND GUILD IN THIS TOO
        }

        public string GroupDecline(int? id)
        {
            string message = null;

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                message = "How did you do this?";
            }

            var inv = ds.GroupInvitations.Include("Character.Group").Include("Character.GroupInvites").Include("Character.UserAccount.ApplicationUser").
                Include("Group.Invites").SingleOrDefault(e => e.Id == id);

            if (message == null && inv.Character.UserAccount.Name != HttpContext.Current.User.Identity.Name)
            {
                message = "This invite doesn't belong to you";
            }

            if (message == null && inv == null)
            {
                message = "This invite doesn't exist";
            }

            inv.Character.GroupInvites.Remove(inv);
            inv.Declined = true;
            //inv.Group.Invites.Remove(inv);

            //ds.GuildInvitations.Remove(inv);
            ds.SaveChanges();

            return (message == null) ? "Declined" : message;

            // SEND MESSAGE TO CHARACTER AND GUILD IN THIS TOO
        }

        public void sendGroupInviteById(int cId, int gId)
        {
            var character = ds.Characters.SingleOrDefault(c => c.Id == cId);
            var group = ds.Groups.SingleOrDefault(g => g.Id == gId);
            if (character != null && group != null)
            {
                var invite = new GroupInvite { Character = character, Group = group };
                var check = ds.GroupInvitations.SingleOrDefault(i => i.Character.Id == invite.Character.Id && i.Group.Id == invite.Group.Id);
                if (check == null)
                {
                    ds.GroupInvitations.Add(invite);
                    character.GroupInvites.Add(invite);
                    ds.SaveChanges();
                }
            }
        }
        public void sendGuildInviteById(int cId, int gId)
        {
            var character = ds.Characters.SingleOrDefault(c => c.Id == cId);
            var guild = ds.Guilds.SingleOrDefault(g => g.Id == gId);
            if (character != null && guild != null)
            {
                var invite = new GuildInvite { Character = character, Guild = guild };
                var check = ds.GuildInvitations.SingleOrDefault(i => i.Character.Id == invite.Character.Id && i.Guild.Id == invite.Guild.Id);
                if (check == null)
                {
                    ds.GuildInvitations.Add(invite);
                    character.GuildInvites.Add(invite);
                    ds.SaveChanges();
                }
            }
        }
        public String AcceptGuildApplication(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var i = ds.GuildInvitations.Include("Character").Include("Guild").SingleOrDefault(x => x.Id == id);

            var g = ds.Guilds.Include("Members").Include("Invites")
                            .Include("Applications").Include("Leader.UserAccount.ApplicationUser").SingleOrDefault(e => e.Id == i.Guild.Id);

            var c = ds.Characters.Include("Guild").Include("GuildInvites").Include("GuildApplications").SingleOrDefault(x => x.Id == i.Character.Id);

            if (g == null || i == null || i == null ||
                HttpContext.Current.User.Identity.Name != g.Leader.UserAccount.ApplicationUser.UserName)
            {
                return "Random useless error";
            }
            if (c.Guild != null)
            {
                return "The character is in a guild";
            }
            

            //Remove the application
            g.Applications.Remove(i);
            c.GuildApplications.Remove(i);
            ds.GuildInvitations.Remove(i);
            //Remove other guild invites/apps
            var apps = ds.GuildInvitations.Where(e => e.Character.Id == c.Id);
            ds.GuildInvitations.RemoveRange(apps);
            //Add Character to guild
            g.Members.Add(c);
            c.Guild = g;
            ds.SaveChanges();
            return  c.Name + " add to " + g.Name;
        }
        public String DenyGuildApplication(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var app = ds.GuildInvitations.Include("Character").Include("Guild.Leader.UserAccount.ApplicationUser").SingleOrDefault(i => i.Id == id);
            if(app != null)
            {
                if(HttpContext.Current.User.Identity.Name == app.Guild.Leader.UserAccount.ApplicationUser.UserName)
                {
                    var g = ds.Guilds.Include("Applications").SingleOrDefault(i => app.Guild.Id == i.Id);
                    var c = ds.Characters.Include("GuildApplications").SingleOrDefault(i => i.Id == app.Character.Id);
                    g.Applications.Remove(app);
                    app.Declined = true;
                    //c.GuildApplications.Remove(app);
                    //ds.GuildInvitations.Remove(app);
                    ds.SaveChanges();
                    return "Application has been denied";
                }
                else
                {
                    return "You can't do this";
                }
            }
            return "Something broke.. no application found";
        }
        public String DenyRaidApplication(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var app = ds.RaidInvitations.Include("Character").Include("Raid.Leader.UserAccount.ApplicationUser").SingleOrDefault(i => i.Id == id);
            if (app != null)
            {
                if (HttpContext.Current.User.Identity.Name == app.Raid.Leader.UserAccount.ApplicationUser.UserName)
                {
                    var r = ds.Raids.Include("Applications").SingleOrDefault(i => app.Raid.Id == i.Id);
                    var c = ds.Characters.Include("RaidApplications").SingleOrDefault(i => i.Id == app.Character.Id);
                    r.Applications.Remove(app);
                    app.Declined = true;
                    ds.SaveChanges();
                    return "Application has been denied";
                }
                else
                {
                    return "You can't do this";
                }
            }
            return "Something broke.. no application found";
        }
        public String AcceptRaidApplication(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var i = ds.RaidInvitations.Include("Character").Include("Raid").SingleOrDefault(x => x.Id == id);

            var r = ds.Raids.Include("Members").Include("Invites").Include("Event")
                            .Include("Applications").Include("Leader.UserAccount.ApplicationUser").SingleOrDefault(e => e.Id == i.Raid.Id);

            var c = ds.Characters.Include("Raids").Include("RaidInvites").Include("RaidApplications").SingleOrDefault(x => x.Id == i.Character.Id);

            if (r == null || i == null || i == null ||
                HttpContext.Current.User.Identity.Name != r.Leader.UserAccount.ApplicationUser.UserName)
            {
                return "Random useless error";
            }
            
            var invite = new RaidInvite { Character = c, Raid = r };

            //Remove the application
            r.Applications.Remove(i);
            c.RaidApplications.Remove(i);
            ds.RaidInvitations.Remove(i);
            //Send the invite
            c.Raids.Add(r);
            r.Members.Add(c);
            ds.SaveChanges();
            return c.Name + " added to " + r.Event.Name ;
        }
        public String AcceptGroupApplication(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var i = ds.GroupInvitations.Include("Character").Include("Group").SingleOrDefault(x => x.Id == id);

            var g = ds.Groups.Include("Members").Include("Invites")
                            .Include("Applications").Include("Leader.UserAccount.ApplicationUser").SingleOrDefault(e => e.Id == i.Group.Id);

            var c = ds.Characters.Include("Group").Include("GroupInvites").Include("GroupApplications").SingleOrDefault(x => x.Id == i.Character.Id);

            if (g == null || i == null ||
                HttpContext.Current.User.Identity.Name != g.Leader.UserAccount.ApplicationUser.UserName)
            {
                return "Random useless error";
            }
            if (c.Group != null)
            {
                return "The character is in a guild";
            }
            var invite = new GroupInvite { Character = c, Group = g };

            //Remove the application
            g.Applications.Remove(i);
            c.GroupApplications.Remove(i);
            ds.GroupInvitations.Remove(i);
            //remove all other guild invites/apps
            var apps = ds.GroupInvitations.Where(e => e.Character.Id == c.Id);
            ds.GroupInvitations.RemoveRange(apps);
            //Add to group
            c.Group = g;
            g.Members.Add(c);
            ds.SaveChanges();
            return "Group invite to " + c.Name + " Sent";
        }
        public String DenyGroupApplication(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var app = ds.GroupInvitations.Include("Character").Include("Group.Leader.UserAccount.ApplicationUser").SingleOrDefault(i => i.Id == id);
            if (app != null)
            {
                if (HttpContext.Current.User.Identity.Name == app.Group.Leader.UserAccount.ApplicationUser.UserName)
                {
                    var g = ds.Groups.Include("Applications").SingleOrDefault(i => app.Group.Id == i.Id);
                    var c = ds.Characters.Include("GroupApplications").SingleOrDefault(i => i.Id == app.Character.Id);
                    g.Applications.Remove(app);
                    app.Declined = true;
                    //c.GuildApplications.Remove(app);
                    //ds.GuildInvitations.Remove(app);
                    ds.SaveChanges();
                    return "Application has been denied";
                }
                else
                {
                    return "You can't do this";
                }
            }
            return "Something broke.. no application found";
        }
        public String RaidInvite(int cid, int gid)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var c = ds.Characters.Include("Raids").Include("RaidApplications.Raid").Include("UserAccount.ApplicationUser").Include("RaidInvites").SingleOrDefault(e => e.Id == cid);


            var r = ds.Raids.Include("Members").Include("Event.Game").Include("Invites.Character").Include("Applications.Character").Include("Leader.UserAccount.ApplicationUser").SingleOrDefault(e => e.Id == gid);

            if (r == null || c == null ||
                HttpContext.Current.User.Identity.Name != r.Leader.UserAccount.ApplicationUser.UserName)
            {
                return "Random useless error";
            }
            
            //Check if character has invite to Raid
            var invite = r.Invites.SingleOrDefault(i => i.Character.Id == c.Id);
            if (invite != null)
            {
                return "Character has invite already";
            }

            var send = new RaidInvite { Character = c, Raid = r };
            c.RaidInvites.Add(send);
            r.Invites.Add(send);
            ds.RaidInvitations.Add(send);
            //Check for an application to delete
            var application = r.Applications.SingleOrDefault(i => i.Character.Id == c.Id);
            if(application != null)
            {
                r.Applications.Remove(application);
                ds.RaidInvitations.Remove(application);
                c.RaidApplications.Remove(application);
            }
            ds.SaveChanges();

            return "You have sent the guild invite";
        }
        public String GuildInvite(int cid, int gid)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var c = ds.Characters.Include("Guild").Include("UserAccount.ApplicationUser").Include("GuildInvites").SingleOrDefault(e => e.Id == cid);


            var g = ds.Guilds.Include("Members").Include("Invites.Character").Include("Applications.Character").Include("Leader.UserAccount.ApplicationUser").SingleOrDefault(e => e.Id == gid);

            if (g == null || c == null ||
                HttpContext.Current.User.Identity.Name != g.Leader.UserAccount.ApplicationUser.UserName)
            {
                return "Random useless error";
            }
            if (c.Guild != null)
            {
                return "The character is in a guild";
            }
            //Check if character has invite/application to guild
            var invite = ds.GuildInvitations.Where(i => i.Guild.Id == g.Id && i.Character.Id == c.Id);
            if (invite.Count() > 0)
            {
                return "Character has applied or invited already";
            }

            var send = new GuildInvite { Character = c, Guild = g };
            c.GuildInvites.Add(send);
            g.Invites.Add(send);
            ds.GuildInvitations.Add(send);
            ds.SaveChanges();

            return "You have sent the guild invite";
        }
        public String GroupInvite(int cid, int gid)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var c = ds.Characters.Include("Group").Include("UserAccount.ApplicationUser").Include("GroupInvites").SingleOrDefault(e => e.Id == cid);


            var g = ds.Groups.Include("Members").Include("Invites.Character").Include("Applications.Character").Include("Leader.UserAccount.ApplicationUser").SingleOrDefault(e => e.Id == gid);

            if (g == null || c == null ||
                HttpContext.Current.User.Identity.Name != g.Leader.UserAccount.ApplicationUser.UserName)
            {
                return "Random useless error";
            }
            if (c.Group != null)
            {
                return "The character is in a guild";
            }
            //Check if character has invite/application to guild
            var invite = ds.GroupInvitations.Where(i => i.Group.Id == g.Id && i.Character.Id == c.Id);
            if (invite.Count() > 0)
            {
                return "Character has applied or invited already";
            }

            var send = new GroupInvite { Character = c, Group = g };
            c.GroupInvites.Add(send);
            g.Invites.Add(send);
            ds.GroupInvitations.Add(send);
            ds.SaveChanges();

            return "You have sent the guild invite";
        }
        public GroupInviteBase getGroupInviteById(int id)
        {
            return mapper.Map<GroupInviteBase>(ds.GroupInvitations.Include("Character").SingleOrDefault(i => i.Id == id));
        }
        public GuildInviteBase getGuildInviteById(int id)
        {
            return mapper.Map<GuildInviteBase>(ds.GuildInvitations.Include("character").SingleOrDefault(i => i.Id == id));
        }
        public IEnumerable<GroupInviteBase> groupInvitesGetById(int id)
        {
            var inv = ds.GroupInvitations.Include("character").Include("group.Leader").Where(ch => ch.Character.Id == id);
            return mapper.Map<IEnumerable<GroupInviteBase>>(inv);
        }
        public IEnumerable<GuildInviteBase> guildInvitesGetById(int id)
        {
            var inv = ds.GuildInvitations.Include("character").Include("guild").Where(ch => ch.Character.Id == id);

            return mapper.Map<IEnumerable<GuildInviteBase>>(inv);
        }
        //public void disbandGuild(int id)
        //{
        //    var guild = ds.Guilds.Find(id);
        //    foreach (var c in guild.Members)
        //    {
        //        c.Guild = null;
        //    }
        //    ds.Guilds.Remove(guild);
        //    ds.SaveChanges();

        //}
        public String GroupLeave(int? id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }

            var c = ds.Characters.Include("Group.Leader").Include("UserAccount.ApplicationUser").SingleOrDefault(e => e.Id == id);
            if(c.Group == null)
            {
                return "You're not in a group";
            }

            var g = ds.Groups.Include("Members").Include("Invites.Character").Include("Applications.Character").Include("Leader").SingleOrDefault(e => e.Id == c.Group.Id);
            if (g == null || c == null ||
                HttpContext.Current.User.Identity.Name != c.UserAccount.Name)
            {
                return "Random useless error";
            }
            if(g.Leader.Id != c.Id)
            {
                g.Members.Remove(c);
                c.Group = null;
                ds.SaveChanges();
                return "You've left the group";
            }
            else
            {
                foreach(var m in g.Members)
                {
                    m.Group = null;
                }
                foreach (var character in g.AllCharacters)
                {
                    character.Group = null;
                }
                foreach (var invite in g.Invites)
                {
                    invite.Character.GroupInvites.Remove(invite);
                }
                foreach (var app in g.Applications)
                {
                    app.Character.GroupApplications.Remove(app);
                }
                ds.GroupInvitations.RemoveRange(g.Invites);
                ds.GroupInvitations.RemoveRange(g.Applications);
                ds.Groups.Remove(g);
                ds.SaveChanges();
                return "Group has been disbanded";
            }

        }
        public String GuildDisband(int? id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var c = ds.Characters.Include("Guild").Include("UserAccount.ApplicationUser").SingleOrDefault(e => e.Id == id);
            if (c.Guild == null)
            {
                return "You are not in a guild";
            }
            
            var g = ds.Guilds.Include("Members").Include("Invites.Character").Include("Applications.Character").Include("Leader").SingleOrDefault(e => e.Id == c.Guild.Id);
            

            if (g == null || c == null ||
                HttpContext.Current.User.Identity.Name != c.UserAccount.Name)
            {
                return "Random useless error";
            }
            if(c.Id != g.Leader.Id)
            {
                return "You ain't the leader";
            }
            foreach(var character in g.AllCharacters)
            {
                character.Guild = null;
            }
            foreach(var invite in g.Invites)
            {
                invite.Character.GuildInvites.Remove(invite);
            }
            foreach(var app in g.Applications)
            {
                app.Character.GuildApplications.Remove(app);
            }
            g.Invites.Clear();
            g.Applications.Clear();
            var clearAppsInvites = ds.GuildInvitations.Where(i => i.Guild.Id == g.Id);
            ds.GuildInvitations.RemoveRange(clearAppsInvites);
            ds.Guilds.Remove(g);
            ds.SaveChanges();

            return "You have disbanded the guild";
        }
        public String RaidDisband(int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var raid = ds.Raids.Include("Leader.UserAccount.ApplicationUser").Include("Members").Include("Invites.Character.RaidInvites").Include("Applications.Character.RaidApplications").SingleOrDefault(i => i.Id == id);

            if (raid == null)
            {
                return "Cant find the raid";
            }
            if (HttpContext.Current.User.Identity.Name != raid.Leader.UserAccount.Name)
            {
                return "You don't have permission to do this";
            }

            
            foreach (var character in raid.AllCharacters)
            {
                character.Raids.Remove(raid);
            }
            foreach (var invite in raid.Invites)
            {
                invite.Character.RaidInvites.Remove(invite);
            }
            foreach (var app in raid.Applications)
            {
                app.Character.RaidApplications.Remove(app);
            }
            raid.Invites.Clear();
            raid.Applications.Clear();
            var clearAppsInvites = ds.RaidInvitations.Where(i => i.Raid.Id == raid.Id);
            ds.RaidInvitations.RemoveRange(clearAppsInvites);
            ds.Raids.Remove(raid);
            ds.SaveChanges();

            return "You have disbanded the raid";
        }
        public String GroupDisband(int? id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var c = ds.Characters.Include("Group").Include("UserAccount.ApplicationUser").SingleOrDefault(e => e.Id == id);
            if (c.Group == null)
            {
                return "You are not in a group";
            }

            var g = ds.Groups.Include("Members").Include("Invites.Character").Include("Applications.Character").Include("Leader").SingleOrDefault(e => e.Id == c.Group.Id);


            if (g == null || c == null ||
                HttpContext.Current.User.Identity.Name != c.UserAccount.Name)
            {
                return "Random useless error";
            }
            if (c.Id != g.Leader.Id)
            {
                return "You ain't the leader";
            }
            foreach (var character in g.AllCharacters)
            {
                character.Group = null;
            }
            foreach (var invite in g.Invites)
            {
                invite.Character.GroupInvites.Remove(invite);
            }
            foreach (var app in g.Applications)
            {
                app.Character.GroupApplications.Remove(app);
            }
            g.Invites.Clear();
            g.Applications.Clear();
            var clearAppsInvites = ds.GroupInvitations.Where(i => i.Group.Id == g.Id);
            ds.GroupInvitations.RemoveRange(clearAppsInvites);
            ds.Groups.Remove(g);
            ds.SaveChanges();

            return "You have disbanded the group";
        }
        public IEnumerable<CharacterBase> getGuildCharList(int guildId)
        {
            var guildInvites = ds.GuildInvitations.Include("Character.Game").Where(i => i.Guild.Id == guildId);
            var guild = ds.Guilds.Include("Game").SingleOrDefault(i => i.Id == guildId);
            var chars = ds.Characters.Include("Guild").Include("Game");
            var x = new List<CharacterBase>();
            foreach (var c in chars)
            {
                if (c.Game.Id == guild.Game.Id && c.Guild == null && guildInvites.Where(i =>  i.Character.Id== c.Id && i.Guild.Id == guildId).Count() == 0)
                    x.Add(mapper.Map<CharacterBase>(c));
            }
            return x;
        }
        public IEnumerable<CharacterBase> getGroupCharList(int groupId)
        {
            var groupInvites = ds.GroupInvitations.Include("Character.Game").Where(i => i.Group.Id == groupId);
            var group = ds.Groups.Include("Game").SingleOrDefault(i => i.Id == groupId);
            var chars = ds.Characters.Include("Group").Include("Game");
            var x = new List<CharacterBase>();

            foreach (var c in chars)
            {
                if(c.Game.Id == group.Game.Id && c.Group == null && groupInvites.Where(i => i.Character.Id == c.Id && i.Group.Id == groupId).Count() == 0)
                {
                    x.Add(mapper.Map<CharacterBase>(c));
                }
            }
            return x;
        }
        public GuildBase GuildEdit(GuildEditLeaderInfo newItem)
        {
            var o = ds.Guilds.Include("Game").Include("Members").SingleOrDefault(a => a.Id == newItem.Id);

            if (o == null || HttpContext.Current.User.Identity.Name != o.Leader.UserAccount.Name)
                return null;
            else
            {
                if(newItem.LeaderId != 0)
                {
                    o.Leader = ds.Characters.SingleOrDefault(i => i.Id == newItem.LeaderId);
                }
                o.Description = newItem.Description;
                o.Name = newItem.Name;
                ds.SaveChanges();
                return mapper.Map<GuildBase>(o);
            }

        }
        public bool leaveGuild(string cName, int id, int gameId)
        {
            var guild = ds.Guilds.Include("Game").Include("Members").SingleOrDefault(a => a.Id == id && a.Game.Id == gameId);
            var c = ds.Characters.Include("Guild").SingleOrDefault(a => a.Name == cName);
            guild.Members.Remove(c);
            c.Guild = null;
            if (guild.Leader != null && guild.Leader.Id == c.Id)
                ds.Guilds.Remove(guild);
            ds.SaveChanges();

            return true;
        }

        public String GuildLeave(int id){
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var c = ds.Characters.Include("UserAccount.ApplicationUser").Include("Guild").SingleOrDefault(e => e.Id == id);
            if (c.Guild == null)
            {
                return "You are not in a guild";
            }

            var g = ds.Guilds.Include("Members").Include("Invites.Character").Include("Applications.Character").Include("Leader").SingleOrDefault(e => e.Id == c.Guild.Id);


            if (g == null || c == null ||
                HttpContext.Current.User.Identity.Name != c.UserAccount.Name)
            {
                return "Random useless error";
            }
            c.Guild = null;
            g.Members.Remove(c);
            ds.SaveChanges();

            return "You have left the guild";
        }
        public String RaidLeave(int cid, int rid)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "How do you do this?";
            }
            var c = ds.Characters.Include("UserAccount.ApplicationUser").Include("Raids").SingleOrDefault(e => e.Id == cid);
            if (c == null)
            {
                return "Character can't be found";
            }

            var r = ds.Raids.Include("Members").Include("Invites.Character").Include("Applications.Character").Include("Leader").SingleOrDefault(e => e.Id == rid);

            if (r == null || c == null ||
                HttpContext.Current.User.Identity.Name != c.UserAccount.Name)
            {
                return "Random useless error";
            }
            c.Raids.Remove(r);
            r.Members.Remove(c);
            ds.SaveChanges();

            return "You have left the raid";
        }
        public CharacterDetails CharacterGetById(int? id)
        {
            var i = (ds.Characters.Include("Game").Include("Group.Leader").Include("Guild.Leader").
                Include("Raids.Event").Include("Raids.Leader").Include("UserAccount.ApplicationUser").SingleOrDefault(a => a.Id == id));

            return (i == null) ? null : mapper.Map<CharacterDetails>(i);
        }

        public CharacterGuildFind CharacterGetPossibleGuilds(int? id)
        {
            var list = new List<GuildFind>();
            var c = ds.Characters.Include("Game").Include("Guild").Include("GuildApplications").Include("GuildInvites").SingleOrDefault(e => e.Id == id);
            if (c == null) { return null; }

            var g = ds.Guilds.Include("Leader.UserAccount.ApplicationUser").Include("Game").Where(e => e.Game.Id == c.Game.Id);
            if (g.Count() == 0) { return null; }

            var i = ds.GuildInvitations.Include("Character").Include("Guild").Where(it => it.Character.Id == c.Id);
            var thing = new CharacterGuildFind();


            thing.Guilds = mapper.Map<IEnumerable<GuildFind>>(g);
            // Needs fixing
            //if (i.Count() == 0)
            //{
            //    // Fuck if I remember why this is here
            //}
            //else
            //{
            //    foreach(var gu in g)
            //    {
            //        var pending = false;
            //        foreach(var inv in i)
            //        {
            //            pending = inv.Guild.Id == gu.Id;
            //        }
            //        if (!pending)
            //        {
            //            list.Add(mapper.Map<GuildFind>(gu));
            //        }
            //    }
            //    thing.Guilds = list;
            //}
            thing.Character = mapper.Map<CharacterGuild>(c);

            return thing;
        }
        public IEnumerable<CharacterBase> getRaidCharList(int? id)
        {
            var raid = ds.Raids.Include("Leader.UserAccount.ApplicationUser").Include("Members").Include("Event.Game")
                               .Include("Invites.Character").SingleOrDefault(i => i.Id == id);
            var characters = ds.Characters.Include("RaidApplications").Where(i => i.Game.Id == raid.Game.Id);
            var list = new List<CharacterBase>();
            if (raid != null) {
                foreach (var c in characters)
                {
                    var invited = raid.Invites.SingleOrDefault(i => i.Character.Id == c.Id);
                    var member = raid.Members.SingleOrDefault(i => i.Id == c.Id);
                    if (raid.Leader.Id != c.Id && invited == null && member == null)
                    {
                        list.Add(mapper.Map<CharacterBase>(c));
                    }
                }
                return list;
            }
            return null;
        }
        public IEnumerable<CharacterBase> getRaidCharListForGuild(int? id)
        {
            var raid = ds.Raids.Include("Leader.UserAccount.ApplicationUser").Include("Leader.Guild").Include("Members").Include("Event.Game")
                               .Include("Invites.Character").SingleOrDefault(i => i.Id == id);
            if (raid.Leader.Guild == null)
                return null;
            var characters = ds.Characters.Include("RaidApplications").Include("Guild").Where(i => i.Game.Id == raid.Game.Id);
            var list = new List<CharacterBase>();
            if (raid != null)
            {
                foreach (var c in characters)
                {
                    var invited = raid.Invites.SingleOrDefault(i => i.Character.Id == c.Id);
                    var member = raid.Members.SingleOrDefault(i => i.Id == c.Id);
                    if (raid.Leader.Id != c.Id && invited == null && member == null && c.Guild != null && c.Guild.Id == raid.Leader.Guild.Id)
                    {
                        list.Add(mapper.Map<CharacterBase>(c));
                    }
                }
                return list;
            }
            return null;
        }
        public IEnumerable<GroupBase> CharacterGetPossibleGroups(int? id)
        {
            var c = ds.Characters.Include("UserAccount.ApplicationUser").Include("Game").Include("Group").Include("GroupApplications").Include("GroupInvites").SingleOrDefault(e => e.Id == id);
            if (c == null) { return new List<GroupBase>(); }

            var g = ds.Groups.Include("Leader").Include("Game").Where(e => e.Game.Id == c.Game.Id);
            if (g == null || g.Count() == 0) { return new List<GroupBase>(); }

            var i = ds.GroupInvitations.Include("Character").Include("Group").Where(it => it.Character.Id == c.Id);
            var thing = new List<GroupBase>();

            if (i.Count() == 0)
            {
                foreach(var group in g)
                {
                    thing.Add(mapper.Map<GroupBase>(group));
                }
            }
            else
            {
                foreach (var gu in g)
                {
                    var pending = false;
                    foreach (var inv in i)
                    {
                        pending = inv.Group.Id == gu.Id;
                    }
                    if (!pending && gu.Members.Count() < 5)
                    {
                        thing.Add(mapper.Map<GroupBase>(gu));
                    }
                }
            }
            foreach(var x in thing)
            {
                x.CurrChar = c.Id;
            }
            return thing;
        }
        //public CharacterAdd testGetById(int id)
        //{
        //    var test = ds.Characters.Include("Guild").Include("Group").Include("Game").Include("UserAccount").SingleOrDefault(i => i.Id == id);
        //    var g = ds.Guilds.Include("Leader").Include("Characters").Include("Game");
        //    var guildInv = mapper.Map<IEnumerable<GuildInviteBase>>(ds.GuildInvitations.Where(ch => ch.Character.Id == id));
        //    var groupInv = mapper.Map<IEnumerable<GroupInviteBase>>(ds.GroupInvitations.Where(ch => ch.Character.Id == id));
        //    var test2 = mapper.Map<CharacterAdd>(test);
        //    foreach (var guild in g)
        //    {
        //        foreach (var c in guild.Members)
        //        {
        //            if (c.Id == id)
        //            {
        //                test.Guild = guild;
        //            }

        //        }
        //    }
        //    test.GuildInvites.Add(guildInv);
        //    test.GroupInvite = groupInv;
        //    return test2;
        //}
        public GuildBase GuildgetByName(string newItem, int gid)
        {
            var g = ds.Guilds.SingleOrDefault(i => i.Name == newItem && i.Game.Id == gid);
            return mapper.Map<GuildBase>(g);
        }
        public GuildBase GuildAdd(GuildAdd newItem)
        {
            var o = (newItem == null) ? null : mapper.Map<Guild>(newItem);
            var g = ds.Games.SingleOrDefault(i => newItem.GameId == i.Id);
            var c = ds.Characters.Include("Guild").SingleOrDefault(i => newItem.LeaderId == i.Id);
            if (o != null && c != null  && c.Guild == null && g != null)
            {
                o.Name = o.Name.Trim();
                o.Description = o.Description.Trim();
                o.Game = g;
                o.Leader = c;
                c.Guild = o;
                ds.Guilds.Add(o);
                ds.SaveChanges();
            }

            return (o == null) ? null : mapper.Map<GuildBase>(o);
        }

        public IEnumerable<CharacterBase> CharacterByUserId(int id)
        {
            // Leaving in for admonition 
            // var i = (ds.Characters.Include("Guild").Include("Game").Where(a => a.User.Id == id));

            var i = ds.Characters.Include("UserAccount.Characters").Include("Game").Where(e => e.UserAccount.Id == id);
            return mapper.Map<IEnumerable<CharacterBase>>(i);
        }

        public IEnumerable<GuildBase> GuildGetAll()
        {
            return mapper.Map<IEnumerable<GuildBase>>(ds.Guilds.Include("Leader.UserAccount.ApplicationUser").Include("Members").Include("Game").OrderBy(u => u.Name));
        }

        public GuildDetails GuildGetById(int id)
        {
            var test = ds.Guilds.Include("Game").Include("Leader.UserAccount.ApplicationUser").Include("Members.UserAccount.ApplicationUser")
                                .Include("Invites.Character").Include("Applications.Character").SingleOrDefault(a => a.Id == id);
            return mapper.Map<GuildDetails>(test);
        }

        public IEnumerable<UserAccountBase> UserAccountGetAll()
        {
            return mapper.Map<IEnumerable<UserAccountBase>>(ds.UserAccounts.Include("ApplicationUser"));
        }

        public UserAccountBase UserAccountGetByIdWithChar(int id)
        {
            var x = ds.UserAccounts.Include("Characters").Include("ApplicationUser").SingleOrDefault(a => a.Id == id);
            return (x == null) ? null : mapper.Map<UserAccountBase>(x);
        }



        /// <summary>
        /// 
        /// Load functions
        /// 
        /// </summary>
        /// <returns></returns>

        public async Task LoadUserAccounts()
        {

            // Get a reference to the objects we need
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(ds));

            try
            {
                var admin = new ApplicationUser { UserName = "admin", Email = "admin@example.com" };
                var result0 = await userManager.CreateAsync(admin, "Password123!");

                //if (!result0.Succeeded)
                //{
                //    var exceptionText = result0.Errors.Aggregate("User Creation Failed - Identity Exception. Errors were: \n\r\n\r", (current, error) => current + (" - " + error + "\n\r"));
                //    throw new Exception(exceptionText);
                //}

                if (result0.Succeeded)
                {
                    await userManager.AddClaimAsync(admin.Id, new Claim(ClaimTypes.Role, "Admin"));
                    await userManager.AddClaimAsync(admin.Id, new Claim(ClaimTypes.Email, admin.Email));
                }

                var josh = new ApplicationUser { UserName = "wierdo1111", Email = "josh@example.com" };
                var result1 = await userManager.CreateAsync(josh, "Password123!");

                if (result1.Succeeded)
                {
                    await userManager.AddClaimAsync(josh.Id, new Claim(ClaimTypes.Role, "PowerUser"));
                    await userManager.AddClaimAsync(josh.Id, new Claim(ClaimTypes.Email, josh.Email));
                }

                var daniel = new ApplicationUser { UserName = "BlankLQ", Email = "daniel@example.com" };
                var result2 = await userManager.CreateAsync(daniel, "Password123!");

                if (result2.Succeeded)
                {
                    await userManager.AddClaimAsync(daniel.Id, new Claim(ClaimTypes.Role, "PowerUser"));
                    await userManager.AddClaimAsync(daniel.Id, new Claim(ClaimTypes.Email, daniel.Email));
                }

                var matt = new ApplicationUser { UserName = "MixMasterMattP", Email = "matt@example.com" };
                var result3 = await userManager.CreateAsync(matt, "Password123!");

                if (result3.Succeeded)
                {
                    await userManager.AddClaimAsync(matt.Id, new Claim(ClaimTypes.Role, "PowerUser"));
                    await userManager.AddClaimAsync(matt.Id, new Claim(ClaimTypes.Email, matt.Email));
                }

                ds.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string output = "";

                foreach (var eve in e.EntityValidationErrors)
                {
                    output += "Entity of type \"" + eve.Entry.Entity.GetType().Name + "\" in state \"" + eve.Entry.State.ToString() + "\" has the following validation errors: ";

                    foreach (var ve in eve.ValidationErrors)
                    {
                        output += " - Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\"";
                    }
                }
                throw new Exception(output);
            }
        }

        public void LoadGames()
        {
            ds.Games.Add(new Game
            {
                Name = "Overwatch",
                Description = "A Game made by Blizzard."
            });

            ds.Games.Add(new Game
            {
                Name = "Diablo",
                Description = "A Game made by Blizzard."
            });

            ds.Games.Add(new Game
            {
                Name = "World of Warcraft",
                Description = "A Game made by Blizzard."
            });

            try
            {
                ds.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string output = "";

                foreach (var eve in e.EntityValidationErrors)
                {
                    output += "Entity of type \"" + eve.Entry.Entity.GetType().Name + "\" in state \"" + eve.Entry.State.ToString() + "\" has the following validation errors: ";

                    foreach (var ve in eve.ValidationErrors)
                    {
                        output += " - Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\"";
                    }
                }
                throw new Exception(output);
            }
        }

        public void LoadCharacters()
        {
            var matt = ds.Users.FirstOrDefault(a => a.UserName == "MixMasterMattP").UserAccount;
            var dan = ds.Users.FirstOrDefault(a => a.UserName == "BlankLQ").UserAccount;
            var josh = ds.Users.FirstOrDefault(a => a.UserName == "wierdo1111").UserAccount;

            /////

            var diab = ds.Games.FirstOrDefault(a => a.Name == "Diablo");
            var wow = ds.Games.FirstOrDefault(a => a.Name == "World of Warcraft");
            var ow = ds.Games.FirstOrDefault(a => a.Name == "Overwatch");

            /////

            var m = ds.Characters.Add(new Character { Name = "Ebiils", UserAccount = matt, Game = ow });
            var m2 = ds.Characters.Add(new Character { Name = "Ebils", UserAccount = matt, Game = wow });
            ow.Characters.Add(m);
            wow.Characters.Add(m2);

            var d2 = ds.Characters.Add(new Character { Name = "LQBlank", UserAccount = dan, Game = diab });
            var d = ds.Characters.Add(new Character { Name = "BlankLQ", UserAccount = dan, Game = ow });
            ow.Characters.Add(d2);
            diab.Characters.Add(d);

            var j2 = ds.Characters.Add(new Character { Name = "11weird11", UserAccount = josh, Game = ow });
            var j = ds.Characters.Add(new Character { Name = "wierdo1111", UserAccount = josh, Game = diab });
            ow.Characters.Add(j2);
            diab.Characters.Add(j);


            /////

            for (int i = 1; i <= 5; i++)
            {
                var newChar = ds.Characters.Add(new Character { Name = "wierdo" + i * 7, UserAccount = josh, Game = ow });
                ow.Characters.Add(newChar);
            }

            try
            { 
                ds.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string output = "";

                foreach (var eve in e.EntityValidationErrors)
                {
                    output += "Entity of type \"" + eve.Entry.Entity.GetType().Name + "\" in state \"" + eve.Entry.State.ToString() + "\" has the following validation errors: ";

                    foreach (var ve in eve.ValidationErrors)
                    {
                        output += " - Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\"";
                    }
                }
                throw new Exception(output);
            }
        }

        public void LoadGuilds()
        {
            var ow = ds.Games.FirstOrDefault(a => a.Name == "Overwatch");

            var m = ds.Characters.FirstOrDefault(a => a.Name == "Ebiils");
            var d = ds.Characters.FirstOrDefault(a => a.Name == "LQBlank");
            var j = ds.Characters.FirstOrDefault(a => a.Name == "11weird11");

            var guild = ds.Guilds.Add(new Guild
            {
                Name = "Escaper",
                Game = ow,
                Leader = m,
                Description = "Blah Blah Blah, something Matt would say."
            });

            m.Guild = guild;
            guild.Members.Add(d);
            d.Guild = guild;
            guild.Members.Add(j);
            j.Guild = guild;

            try
            {
                ds.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string output = "";

                foreach (var eve in e.EntityValidationErrors)
                {
                    output += "Entity of type \"" + eve.Entry.Entity.GetType().Name + "\" in state \"" + eve.Entry.State.ToString() + "\" has the following validation errors: ";

                    foreach (var ve in eve.ValidationErrors)
                    {
                        output += " - Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\"";
                    }
                }
                throw new Exception(output);
            }
        }

        public void LoadEvents()
        {
            var matt = ds.Users.FirstOrDefault(a => a.UserName == "MixMasterMattP").UserAccount;
            var dan = ds.Users.FirstOrDefault(a => a.UserName == "BlankLQ").UserAccount;
            var josh = ds.Users.FirstOrDefault(a => a.UserName == "wierdo1111").UserAccount;

            /////

            var diab = ds.Games.FirstOrDefault(a => a.Name == "Diablo");
            var wow = ds.Games.FirstOrDefault(a => a.Name == "World of Warcraft");
            var ow = ds.Games.FirstOrDefault(a => a.Name == "Overwatch");

            /////

            var event1 = ds.Events.Add(new Event
            {
                Name = "Romp and Stomp",
                Description = "Run around and kill stuff",
                Creator = josh,
                DateCreated = DateTime.Now.AddDays(-21),
                Game = diab,
                SizeLimit = 20
            });
            diab.Events.Add(event1);
            josh.Events.Add(event1);

            var event2 = ds.Events.Add(new Event
            {
                Name = "Casual Dance Party",
                Description = "Run around and dance",
                Creator = matt,
                DateCreated = DateTime.Now.AddDays(-10),
                Game = wow,
                SizeLimit = 200
            });
            wow.Events.Add(event2);
            matt.Events.Add(event2);

            var event3 = ds.Events.Add(new Event
            {
                Name = "Six Mei Extravaganza",
                Description = "Run around and freeze stuff",
                Creator = dan,
                DateCreated = DateTime.Now.AddDays(-10),
                Game = ow,
                SizeLimit = 200
            });
            ow.Events.Add(event3);
            dan.Events.Add(event3);

            try
            {
                ds.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string output = "";

                foreach (var eve in e.EntityValidationErrors)
                {
                    output += "Entity of type \"" + eve.Entry.Entity.GetType().Name + "\" in state \"" + eve.Entry.State.ToString() + "\" has the following validation errors: ";

                    foreach (var ve in eve.ValidationErrors)
                    {
                        output += " - Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\"";
                    }
                }
                throw new Exception(output);
            }
        }

        public void LoadRaids()
        {
            var m = ds.Characters.FirstOrDefault(a => a.Name == "Ebiils");
            var d = ds.Characters.FirstOrDefault(a => a.Name == "BlankLQ");
            var j = ds.Characters.FirstOrDefault(a => a.Name == "wierdo1111");

            var event1 = ds.Events.FirstOrDefault(e => e.Name == "Romp and Stomp");
            var event2 = ds.Events.FirstOrDefault(e => e.Name == "Casual Dance Party");
            var event3 = ds.Events.FirstOrDefault(e => e.Name == "Six Mei Extravaganza");

            var raid1 = ds.Raids.Add(new Raid
            {
                Event = event1,
                Leader = j,
                ScheduledTime = DateTime.Now.AddDays(10)
            });
            j.Raids.Add(raid1);
            event1.Raids.Add(raid1);

            var raid2 = ds.Raids.Add(new Raid
            {
                Event = event2,
                Leader = m,
                ScheduledTime = DateTime.Now.AddDays(12)
            });
            m.Raids.Add(raid2);
            event2.Raids.Add(raid2);


            var raid3 = ds.Raids.Add(new Raid
            {
                Event = event3,
                Leader = d,
                ScheduledTime = DateTime.Now.AddDays(6)
            });
            d.Raids.Add(raid3);
            event3.Raids.Add(raid3);

            try
            {
                ds.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string output = "";

                foreach (var eve in e.EntityValidationErrors)
                {
                    output += "Entity of type \"" + eve.Entry.Entity.GetType().Name + "\" in state \"" + eve.Entry.State.ToString() + "\" has the following validation errors: ";

                    foreach (var ve in eve.ValidationErrors)
                    {
                        output += " - Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\"";
                    }
                }
                throw new Exception(output);
            }
        }

        public void LoadGuildInvites()
        {
            var guild = ds.Guilds.FirstOrDefault(e => e.Name == "Escaper");

            var d = ds.Characters.SingleOrDefault(e => e.Name == "BlankLQ");
            var j = ds.Characters.Where(e => e.UserAccount.ApplicationUser.UserName == "wierdo1111" && e.Game.Name == "Overwatch").Take(3);

            var i1 = ds.GuildInvitations.Add(new GuildInvite
            {
                Character = d,
                Guild = guild
            });
            d.GuildInvites.Add(i1);
            guild.Invites.Add(i1);


            foreach (var chara in j)
            {
                var i2 = ds.GuildInvitations.Add(new GuildInvite
                {
                    Character = chara,
                    Guild = guild
                });

                chara.GuildInvites.Add(i2);
                guild.Invites.Add(i2);
            }

            try
            {
                ds.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string output = "";

                foreach (var eve in e.EntityValidationErrors)
                {
                    output += "Entity of type \"" + eve.Entry.Entity.GetType().Name + "\" in state \"" + eve.Entry.State.ToString() + "\" has the following validation errors: ";

                    foreach (var ve in eve.ValidationErrors)
                    {
                        output += " - Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\"";
                    }
                }
                throw new Exception(output);
            }
        }

        public async Task ReloadData()
        {
            RemoveDatabase();

            await LoadUserAccounts();
            LoadGames();
            LoadCharacters();
            LoadGuilds();
            LoadEvents();
            LoadRaids();
            LoadGuildInvites();
        }

        public bool RemoveDatabase()
        {
            try
            {
                // Delete database
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}