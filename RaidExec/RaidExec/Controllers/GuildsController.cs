using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    [Authorize]
    public class GuildsController : Controller
    {
        private Manager m = new Manager();
        // GET: Guild
        public ActionResult Index()
        {
            return View(m.GuildGetAll());
        }

        // GET: Guild/Details/5
        public ActionResult Details(int id)
        {
            var o = m.GuildGetById(id);
            return View(o);
        }
        public ActionResult GuildCancelApp(int? id)
        {
            return Content("<p>" + m.CancelGuildApp(id.GetValueOrDefault()) + "</p>");
        }
        public ActionResult GuildCancelInv(int? id)
        {
            return Content("<p>" + m.CancelGuildInv(id.GetValueOrDefault()) + "</p>");
        }
        public ActionResult DenyApplication(int id)
        {
            return Content("<p>" + m.DenyGuildApplication(id) + "</p>");
        }
        public ActionResult AcceptApplication(int id)
        {
            return Content("<p>" + m.AcceptGuildApplication(id) + "</p>");
        }

        public ActionResult GuildLeave(int id)
        {
            return Content("<p>" + m.GuildLeave(id) + "</p>");
        }
        public ActionResult GuildKick(int id)
        {
            return Content("<p>" + m.GuildKick(id) + "</p>");
        }
        public ActionResult GuildDisband(int? id)
        {
            return Content("<p>" + m.GuildDisband(id) + "</p>");
        }
        // GET: Guild/Create
        public ActionResult AddGuild(int? id)
        {
            var form = new GuildAddForm();
            var c = m.CharacterGetById(id.GetValueOrDefault());
            form.LeaderId = c.Id;
            form.LeaderName = c.Name;
            form.GameId = c.Game.Id;
            form.GameName = c.Game.Name;
            return View(form);
        }
        // POST: Guild/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGuild(GuildAdd newItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var checkName = m.GuildgetByName(newItem.Name, newItem.GameId);
                    if (checkName == null)
                    {
                        var item = m.GuildAdd(newItem);
                        if (item != null)
                        {
                            return RedirectToAction("Details", new { id = item.Id });
                        }
                    }
                    else
                    {
                        var chara = m.CharacterGetById(newItem.LeaderId);
                        var f = new GuildAddForm();
                        f.errorName = "Name is already in use.";
                        f.LeaderName = chara.Name;
                        f.LeaderId = chara.Id;
                        f.GameId = chara.Game.Id;
                        f.GameName = chara.Game.Name;
                        return View(f);
                    }
                }
                catch (Exception e)
                {
                    return HttpNotFound();
                }
            }
            var c = m.CharacterGetById(newItem.LeaderId);
            var form = new GuildAddForm();
            form.LeaderName = c.Name;
            form.LeaderId = c.Id;
            form.GameId = c.Game.Id;
            form.GameName = c.Game.Name;
            return View(form);
        }
        
        public ActionResult InviteGuild(int? id)
        {
            var guild = m.GuildGetById(id.GetValueOrDefault());
            if (guild == null)
                return HttpNotFound();
            else
            {
                var list = m.mapper.Map<GuildEditForm>(guild);
                list.CharList = new MultiSelectList
                    (items: m.getGuildCharList(guild.Id),
                    dataValueField: "Id",
                    dataTextField: "Name");
                list.Name = guild.Name;
                list.Id = guild.Id;
                return View(list);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InviteGuild(GuildEditInfo item)
        {
            foreach(var i in item.Characters)
            {
                m.GuildInvite(i, item.Id);
            }
            return RedirectToAction("Index");
        }

        // GET: Guild/Edit/5
        public ActionResult GuildEdit(int? id)
        {
            var guild = m.GuildGetById(id.GetValueOrDefault());
            if (guild == null)
                return HttpNotFound();
            else
            {
                var form = new GuildEditLeaderForm();
                form.Name = guild.Name;
                form.Description = guild.Description;
                form.Leader = guild.Leader;
                form.LeaderId = new SelectList(guild.Members, "Id", "Name",guild.Leader.Id);
                form.GameId = guild.Game.Id;
                return View(form);
            }
            
        }

        // POST: Guild/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuildEdit(int? id, GuildEditLeaderInfo newItem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", new { id = newItem.Id });
            }
            if(id.GetValueOrDefault() != newItem.Id)
            {
                return RedirectToAction("Index");
            }
            try
            {
                var g = m.GuildgetByName(newItem.Name, newItem.GameId);
                if (g == null)
                {
                    var item = m.GuildEdit(newItem);
                    if (item != null)
                    {
                        return RedirectToAction("Details", new { id = item.Id });
                    }
                }
                var guild = m.GuildGetById(id.GetValueOrDefault());

                var form = new GuildEditLeaderForm();
                form.Name = guild.Name;
                form.Description = guild.Description;
                form.Leader = guild.Leader;
                form.LeaderId = new SelectList(guild.Members, "Id", "Name", guild.Leader.Id);
                form.GameId = guild.Game.Id;
                form.errorName = "The name is already in use.";
                return View(form);
            }
            catch(Exception e)
            {

            }
            return HttpNotFound();
        }

        // GET: Guild/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Guild/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult FindGuild(int? id)
        {
            return View(m.CharacterGetPossibleGuilds(id));
        }

        public ActionResult ApplyGuild(int? gid, int? cid)
        {
            //return RedirectToAction("Index");
            return Content("<p>" + m.GuildApply(gid, cid) + "</p>");
        }
    }
}
