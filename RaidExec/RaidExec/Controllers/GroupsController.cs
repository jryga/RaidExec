using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    public class GroupsController : Controller
    {
        Manager m = new Manager();
        // GET: Groups
        public ActionResult Index(int? id)
        {
            return View(m.GroupGetAllByGame(id.GetValueOrDefault()));
        }
        public ActionResult FindGroup(int? id)
        {
            return View(m.CharacterGetPossibleGroups(id.GetValueOrDefault()));
        }
        public ActionResult GroupKick(int id)
        {
            return Content("<p>" + m.GroupKick(id) + "</p>");
        }
        public ActionResult ApplyGroup(int? gid, int? cid)
        {
            //return RedirectToAction("Index");
            return Content("<p>" + m.GroupApply(gid, cid) + "</p>");
        }
        public ActionResult GroupCancelApp(int? id)
        {
            return Content("<p>" + m.CancelGroupApp(id.GetValueOrDefault()) + "</p>");
        }
        public ActionResult GroupCancelInv(int? id)
        {
            return Content("<p>" + m.CancelGroupInv(id.GetValueOrDefault()) + "</p>");
        }
        public ActionResult DenyApplication(int id)
        {
            return Content("<p>" + m.DenyGroupApplication(id) + "</p>");
        }
        public ActionResult AcceptApplication(int id)
        {
            return Content("<p>" + m.AcceptGroupApplication(id) + "</p>");
        }
        public ActionResult AddGroup(int? id)
        {
            
            return Content("<p>" + m.AddGroup(id.GetValueOrDefault()) + "</p>");
        }
        public ActionResult GroupLeave(int? id)
        {
            return Content("<p>" + m.GroupLeave(id.GetValueOrDefault()) + "</p>");
        }
        public ActionResult GroupDisband(int? id)
        {
            return Content("<p>" + m.GroupDisband(id.GetValueOrDefault()) + "</p>");
        }
        // GET: Groups/Details/5
        public ActionResult Details(int id)
        {
            return View(m.getGroupById(id));
        }
        public ActionResult GroupInvite(int id)
        {
            var group = m.getGroupById(id);
            if (group == null)
                return HttpNotFound();
            else
            {
                var list = new GroupEditForm();
                list.CharList = new MultiSelectList
                    (items: m.getGroupCharList(id),
                    dataValueField: "Id",
                    dataTextField: "Name");
                list.Leader = group.Leader;
                list.Id = group.Id;
                return View(list);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GroupInvite(GroupEditInfo item)
        {
            foreach (var i in item.Characters)
            {
                m.GroupInvite(i, item.Id);
            }
            //Frustrated cheese alert
            var game = m.CharacterGetById(item.Characters.First()).Game.Id;
            return RedirectToAction("Details", new { id = item.Id });
        }
        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
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

        // GET: Groups/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Groups/Edit/5
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

        // GET: Groups/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Groups/Delete/5
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
