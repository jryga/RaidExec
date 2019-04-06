using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private Manager m = new Manager();


        // GET: Games
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(m.GameGetAll());
        }

        // GET: Games/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            return View(m.GameDetailsGetById(id));
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            return View(new GameAddForm());
        }

        // POST: Games/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin, PowerUser")]
        public ActionResult Create(GameAdd newItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var item = m.GameAdd(newItem);
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

            var form = m.mapper.Map<GameAddForm>(newItem);
            return View(form);
        }

        // GET: Games/Edit/5
        [Authorize(Roles = "Admin, PowerUser")]
        public ActionResult Edit(int? id)
        {
            var o = m.GameGetById(id.GetValueOrDefault());

            if (o == null)
                return HttpNotFound();

            return View(m.mapper.Map<GameEditForm>(o));
        }

        // POST: Games/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, PowerUser")]
        public ActionResult Edit(int? id, GameEditInfo newItem)
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
                var item = m.GameEdit(newItem);
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

        // GET: Games/Delete/5
        [Authorize(Roles = "Admin, PowerUser")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Games/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, PowerUser")]
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
