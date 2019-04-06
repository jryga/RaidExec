using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    [Authorize]
    public class RaidsController : Controller
    {
        private Manager m = new Manager();

        // GET: Raids
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(m.RaidGetAll());
        }

        // GET: Raids/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            var o = m.RaidDetailsGetById(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(o);
            }
        } 
        public ActionResult RaidGroupApply(int? id)
        {
            var o = m.RaidDetailsGetById(id);
            var form = new RaidApplicationForm();
            form.Leader = o.Leader;
            form.EventName = o.Event.Name;
            form.Id = o.Id;
            form.CharList = new SelectList(m.getMyGroupCharacters(HttpContext.User.Identity.Name, o.Id), dataValueField: "Id", dataTextField: "Name");
            return View(form);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RaidGroupApply(RaidApplicationInfo item)
        {
            ViewBag.ApplyMessage = null;

            if (item.CharacterId == 0)
            {
                ModelState.AddModelError("CharList", "You must select a character.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ViewBag.ApplyMessage = m.ApplyGroupToRaid(item);
                }
                catch (Exception e)
                {
                    return HttpNotFound();
                }
            }

            var o = m.RaidDetailsGetById(item.Id);
            var form = new RaidApplicationForm();
            form.Leader = o.Leader;
            form.EventName = o.Event.Name;
            form.Id = o.Id;
            form.CharList = new SelectList(m.getMyGroupCharacters(HttpContext.User.Identity.Name, o.Id), dataValueField: "Id", dataTextField: "Name");
            return View(form);
        }

        public ActionResult RaidCancelApp(int? id)
        {
            return Content("<p>" + m.CancelRaidApp(id.GetValueOrDefault()) + "</p>");
        }
        public ActionResult RaidCancelInv(int? id)
        {
            return Content("<p>" + m.CancelRaidInv(id.GetValueOrDefault()) + "</p>");
        }
        public ActionResult RaidKick(int cid, int rid)
        {
            return Content("<p>" + m.RaidKick(cid, rid) + "</p>");
        }
        public ActionResult RaidLeave(int cid, int rid)
        {
            return Content("<p>" + m.RaidLeave(cid, rid) + "</p>");
        }
        public ActionResult RaidDisband(int? id)
        {
            return Content("<p>" + m.RaidDisband(id.GetValueOrDefault()) + "</p>");
        }

        public ActionResult RaidApply(int? id)
        {
            var o = m.RaidDetailsGetById(id);
            var form = new RaidApplicationForm();
            form.Leader = o.Leader;
            form.EventName = o.Event.Name;
            form.Id = o.Id;
            form.CharList = new SelectList(m.getMyCharacters(HttpContext.User.Identity.Name, o.Id), dataValueField: "Id", dataTextField: "Name");
            return View(form);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RaidApply(RaidApplicationInfo item)
        {
            ViewBag.ApplyMessage = null;

            if (item.CharacterId == 0)
            {
                ModelState.AddModelError("CharList", "You must select a character.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ViewBag.ApplyMessage = m.ApplyToRaid(item);
                }
                catch (Exception e)
                {
                    return HttpNotFound();
                }
            }

            var o = m.RaidDetailsGetById(item.Id);
            var form = new RaidApplicationForm();
            form.Leader = o.Leader;
            form.EventName = o.Event.Name;
            form.Id = o.Id;
            form.CharList = new SelectList(m.getMyCharacters(HttpContext.User.Identity.Name, o.Id), dataValueField: "Id", dataTextField: "Name");
            return View(form);
        }

        public ActionResult DenyApplication(int id)
        {
            return Content("<p>" + m.DenyRaidApplication(id) + "</p>");
        }
        public ActionResult AcceptApplication(int id)
        {
            return Content("<p>" + m.AcceptRaidApplication(id) + "</p>");
        }
        public ActionResult RaidInviteGuild(int? id)
        {
            var o = m.RaidDetailsGetById(id);
            var c = m.CharacterGetById(o.Leader.Id);
            if (c.Guild == null)
                return Content("<p> You're not in a guild </p>");
            if (o == null || o.Leader.UserAccount.Name != HttpContext.User.Identity.Name)
            {
                return HttpNotFound();
            }
            else
            {
                var form = new RaidInviteForm();
                form.EventName = o.Event.Name;
                form.ScheduledTime = o.ScheduledTime;
                form.CharList = new MultiSelectList(m.getRaidCharListForGuild(id), dataValueField: "Id", dataTextField: "Name");
                form.Id = o.Id;
                return View(form);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RaidInviteGuild(RaidInviteInfo item)
        {
            foreach (var i in item.Characters)
            {
                m.RaidInvite(i, item.Id);
            }
            return RedirectToAction("Details", new { id = item.Id });
        }
        public ActionResult RaidInvite(int? id)
        {
            var o = m.RaidDetailsGetById(id.GetValueOrDefault());

            if(o == null || o.Leader.UserAccount.Name != HttpContext.User.Identity.Name)
            {
                return HttpNotFound();
            }
            else
            {
                var form = new RaidInviteForm();
                form.EventName = o.Event.Name;
                form.ScheduledTime = o.ScheduledTime;                   
                form.CharList = new MultiSelectList(m.getRaidCharList(id), dataValueField:"Id", dataTextField: "Name");
                form.Id = o.Id;
                return View(form);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RaidInvite(RaidInviteInfo item)
        {
            foreach(var i in item.Characters)
            {
                m.RaidInvite(i, item.Id);
            }
            return RedirectToAction("Details", new { id = item.Id});
        }
        public ActionResult RaidAccept(int? cId, int? iId)
        {
            var c = m.CharacterGetById(cId);
            if (c == null)
                return Content("Not your invite to accept");
            return Content("<p>"+m.RaidAccept(iId.GetValueOrDefault())+"</p>");

        }

        public ActionResult RaidDecline(int? cId, int? iId)
        {
            var c = m.CharacterGetById(cId);
            if (c == null)
                return Content("Not your invite to decline");
            return Content("<p>"+m.RaidDecline(iId.GetValueOrDefault())+"</p>");
        }
        // GET: Raids/Edit/5
        public ActionResult Edit(int? id)
        {
            var o = m.RaidGetById(id.GetValueOrDefault());

            if (o == null || (o.Leader.UserAccount.Name != User.Identity.Name) && !User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }

            return View(m.mapper.Map<RaidEditForm>(o));
        }

        // POST: Raids/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, RaidEdit newItem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", new { id = newItem.Id });
            }

            if (id.GetValueOrDefault() != newItem.Id)
            {
                return RedirectToAction("index");
            }

            try
            {
                var item = m.RaidEdit(newItem);
                if (item != null)
                {
                    return RedirectToAction("Details", new { id = item.Id });
                }

                return RedirectToAction("Edit", new { id = newItem.Id });
            }
            catch (Exception e)
            {
                // ...
            }

            return HttpNotFound();
        }

        // GET: Raids/Delete/5
        public ActionResult Delete(int? id)
        {
            var o = m.RaidDetailsGetById(id.GetValueOrDefault());

            if (o == null || (o.Leader.UserAccount.Name != User.Identity.Name) && !User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            else
            {
                return View(o);
            }
        }

        // POST: Raids/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(RaidBase deletedItem)
        {
            // check if valid
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            try
            {
                if (m.RaidRemove(deletedItem.Id))
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                // ...
            }

            return HttpNotFound();
        }
    }
}
