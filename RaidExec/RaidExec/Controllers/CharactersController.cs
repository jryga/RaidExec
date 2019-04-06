using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    [Authorize]
    public class CharactersController : Controller
    {
        Manager m = new Manager();
        // GET: Character
        public ActionResult Index()
        {
            return View(m.CharacterGetAll());
        }

        // GET: Character/Details/5
        public ActionResult Details(int? id)
        {
            return View(m.CharacterGetById(id));
        }

        public ActionResult GroupDetails(int? id, int? cId)
        {
            var group = m.getGroupById(id.GetValueOrDefault());
            group.invFlag = group.Leader.Id == cId.GetValueOrDefault();
            return View(group);
        }
        public ActionResult GroupInvite(int? id)
        {
            var group = m.getGroupById(id.GetValueOrDefault());
            if (group == null)
                return HttpNotFound();
            else
            {
                var list = new GroupEditForm();
                
                list.CharList = new MultiSelectList
                    (items: m.getGroupCharList(group.Game.Id),
                    dataValueField: "Id",
                    dataTextField: "Name");
                list.Id = group.Id;
                return View(list);
            }
        }
        [HttpPost]
        public ActionResult GroupInvite(GroupEditInfo item)
        {
            foreach(var i in item.Characters)
            {
                m.sendGroupInviteById(i, item.Id);
            }
            return Content("");
        }
        public ActionResult guildInvites(int? id)
        {
            return View(m.guildInvitesGetById(id.GetValueOrDefault()));
        }
        public ActionResult groupInvites(int? id)
        {
            return View(m.groupInvitesGetById(id.GetValueOrDefault()));
        }
        public ActionResult Leave(string Name, int? id, int? gId)
        {
            m.leaveGuild(Name, id.GetValueOrDefault(), gId.GetValueOrDefault());
            return Content("");

        }
        public ActionResult groupAccept(int? cId, int? iId)
        {
            return Content("<p>" + m.GroupAccept(iId.GetValueOrDefault()) + "</p>");
        }
        public ActionResult groupDecline(int? cId, int? iId)
        {
            return Content("<p>" + m.GroupDecline(iId.GetValueOrDefault()) + "</p>");

        }

        public ActionResult guildAccept(int? id)
        {
            return Content("<p>" + m.GuildAccept(id) + "</p>");
        }

        public ActionResult guildDecline(int? id)
        {
            return Content("<p>" + m.GuildDecline(id) + "</p>");
        }

        // GET: Character/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Character/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult DisbandGuild(int? gId, int? lId)
        {
            var guild = m.GuildGetById(gId.GetValueOrDefault());
            if (guild == null || guild.Leader.Id != lId.GetValueOrDefault())
                return HttpNotFound();
            else
            {
                m.GuildDisband(lId);
                return RedirectToAction("Details", new { id = lId });
            }

        }
        // GET: Guild/Edit/5
        public ActionResult GuildEdit(int? id)
        {
            var guild = m.GuildGetById(id.GetValueOrDefault());
            if (guild == null)
                return HttpNotFound();
            else
            {
                var form = m.mapper.Map<GuildEditForm>(guild);
                form.Characters = guild.Members;
                return View(form);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuildEdit(int? id, GuildEditInfo newItem)
        {
            //if (!ModelState.IsValid)
            //    return RedirectToAction("Details", new { id = newItem.Id });
            //if (id.GetValueOrDefault() != newItem.Id)
            //    return RedirectToAction("Index");

            //var editedItem = m.GuildEdit(newItem);

            //if (editedItem == null)
            //    return RedirectToAction("Index", new { id = newItem.Id });
            //else
                return RedirectToAction("Details",new { id = newItem.Id});
            
        }
        // GET: Character/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Character/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Character/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Character/Delete/5
        [HttpPost]
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
    }
}
