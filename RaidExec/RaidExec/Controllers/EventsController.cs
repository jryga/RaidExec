using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private Manager m = new Manager();

        // GET: Events
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(m.EventGetIndex());
        }

        // GET: Events/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            var o = m.EventDetailsGetById(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(o);
            }
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            var games = (User.IsInRole("Admin")) ? m.GameGetAll() : m.GameGetAllByAccountName(User.Identity.Name);

            var form = new EventAddForm
            {
                GameList = new SelectList(games, "Id", "Name")
            };

            return View(form);
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventAdd newItem)
        {
            if (newItem.GameId == 0)
            {
                ModelState.AddModelError("GameList", "You must select a game.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var item = m.EventAdd(newItem);
                    if (item != null)
                    {
                        return RedirectToAction("Details", new { id = item.Id });
                    }
                }
                catch (Exception e)
                {
                    return HttpNotFound();
                }
            }
         
            var form = m.mapper.Map<EventAddForm>(newItem);

            var games = (User.IsInRole("Admin")) ? m.GameGetAll() : m.GameGetAllByAccountName(User.Identity.Name);
            form.GameList = new SelectList(games, "Id", "Name", newItem.GameId);

            return View(form);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            var o = m.EventGetById(id.GetValueOrDefault());

            if (o == null || (o.Creator.Name != User.Identity.Name) && !User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }

            return View(m.mapper.Map<EventEditForm>(o));
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, EventEdit newItem)
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
                var item = m.EventEdit(newItem);
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

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            var o = m.EventDetailsGetById(id.GetValueOrDefault());

            if (o == null || (o.Creator.Name != User.Identity.Name) && !User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            else
            {
                return View(o);
            }
        }

        // POST: Events/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(EventBase deletedItem)
        {
            // check if valid
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            try
            {
                if (m.EventRemove(deletedItem.Id))
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

        // GET: Events/CreateRaid/5
        public ActionResult CreateRaid(int? id)
        {
            var o = m.EventGetById(id);

            if (o == null)
            {
                return HttpNotFound();

            }

            var c = m.CharacterGetAllForUserByGame(User.Identity.Name, o.GameId);
            if (c.Count() == 0)
            {
                return RedirectToAction("NoCharacters", new { id = o.GameId, eid = id });
            }

            var form = m.mapper.Map<EventRaidAddForm>(o);
            form.ScheduledTime = DateTime.Now;
            form.LeaderList = new SelectList(c, "Id", "Name");

            return View(form);
        }

        // POST: Events/CreateRaid/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRaid(int? id, EventRaidAdd newItem)
        {
            if (id.GetValueOrDefault() != newItem.Id)
            {
                return RedirectToAction("Index");
            }

            if (newItem.LeaderId == 0)
            {
                ModelState.AddModelError("LeaderList", "You must select a character.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var item = m.RaidAdd(newItem);
                    if (item != null)
                    {
                        return RedirectToAction("Details", "Raids", new { id = item.Id });
                    }
                }
                catch (Exception e)
                {
                    return HttpNotFound();
                }
            }

            var o = m.EventGetById(id);
            var c = m.CharacterGetAllForUserByGame(User.Identity.Name, o.GameId);
            var form = m.mapper.Map<EventRaidAddForm>(newItem);

            if (c.Count() == 0)
            {
                return RedirectToAction("NoCharacters", new { id = o.GameId });
            }
            form.LeaderList = new SelectList(c, "Id", "Name");

            return View(form);
        }

    }
}
