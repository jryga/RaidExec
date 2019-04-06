using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;

namespace RaidExec.Controllers
{
    [Authorize]
    public class UserAccountsController : Controller
    {
        private Manager m = new Manager();

        // GET: UserAccount/Details/5
        public ActionResult _Home(int? id)
        {     
            var o = m.UserAccountGetHomeByName(User.Identity.Name);
            //var o = m.UserAccountGetHome(id.GetValueOrDefault());


            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(o);
            }
        }

        // GET: UserAccount
        public ActionResult Index()
        {
            var c = m.UserAccountGetAll();
            return View(c);
        }

        // GET: UserAccount/Details/5
        public ActionResult Details(int? id)
        {
            UserAccountHome o;
            if (id == -1)
            {
                o = m.UserAccountGetHomeByName(User.Identity.Name);
            }
            else
            {
                o = m.UserAccountGetHomeById(id);
            }
            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(o);
            }
        }

        // GET: UserAccount/Details/5
        public ActionResult CharacterDetails(int? id)
        {
            var o = m.CharacterGetById(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(o);
            }
        }

        // POST: UserAccount/Edit/5
        public ActionResult Edit()
        {
            var o = m.UserAccountGetByName(User.Identity.Name);

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {

                var editForm = m.mapper.Map<UserAccountEditForm>(o);

                return View(editForm);
            }
        }



        // POST: UserAccount/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, UserAccountEditInfo newItem)
        {

            // Validate the input
            if (!ModelState.IsValid)
            {
                // Our "version 1" approach is to display the "edit form" again
                return RedirectToAction("edit", new { id = newItem.Id });
            }

            if (id.GetValueOrDefault() != newItem.Id)
            {
                // This appears to be data tampering, so redirect the user away
                return RedirectToAction("index");
            }

            // Attempt to do the update
            var editedItem = m.EditUserAccountBase(newItem);



            if (editedItem == null)
            {
                // There was a problem updating the object
                // Our "version 1" approach is to display the "edit form" again
                return RedirectToAction("edit", new { id = newItem.Id });
            }
            else
            {
                // Show the details view, which will have the updated data
                return RedirectToAction("details", new { id = newItem.Id });
            }
        }




        public ActionResult CharacterList(int? id)
        {
            var o = m.CharacterGetAllForUser(User.Identity.Name);

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {

                return View(o);
            }
        }


        // POST: UserAccount/AddGame/5
        public ActionResult CreateCharacter()
        {
            var o = m.UserAccountGetByName(User.Identity.Name);

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {

                var editForm = m.mapper.Map<UserAccountAddCharacter>(o);
                editForm.Name = "";
                editForm.GameList = new SelectList(m.GameGetAll(), "Id", "Name");

                return View(editForm);
            }
        }


        [HttpPost]
        public ActionResult CreateCharacter(int? id, UserAccountAddCharacter newItem)
        {

            Regex rgx = new Regex("^[a-zA-Z0-9_]{4,20}$");
            if (!rgx.Match(newItem.Name).Success)
            {
                ModelState.AddModelError("Name", "Alphanumeric and underscores in Name only");
            }
            if (!rgx.Match(newItem.Class).Success)
            {
                ModelState.AddModelError("Class", "Alphanumeric and spaces in Class only");
            }
            if (newItem.Level < 0)
            {
                ModelState.AddModelError("Level", "Level must be greater than or equal to 0");
            }
            // Validate the input
            if (!ModelState.IsValid)
            {
                newItem.GameList = new SelectList(m.GameGetAll(), "Id", "Name");
                return View(newItem);
            }

            if (id.GetValueOrDefault() != newItem.Id)
            {
                // This appears to be data tampering, so redirect the user away
                return RedirectToAction("index");
            }

            // Attempt to do the update
            var editedItem = m.CharacterCreate(newItem, User.Identity.Name);



            if (editedItem == null)
            {
                // There was a problem updating the object
                // Our "version 1" approach is to display the "edit form" again
                return RedirectToAction("CreateCharacter", new { id = newItem.Id });
            }
            else
            {
                // Show the details view, which will have the updated data
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }


        // POST: UserAccount/Edit/5
        public ActionResult CharacterEdit(int? id)
        {
            var o = m.CharacterGetById(id.GetValueOrDefault());
            if (o.UserAccount.Name != User.Identity.Name)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {

                var editForm = m.mapper.Map<CharacterEditForm>(o);

                return View(editForm);
            }
        }



        // POST: UserAccount/Edit/5
        [HttpPost]
        public ActionResult CharacterEdit(int? id, CharacterEditInfo newItem)
        {

            Regex rgx = new Regex("^[a-zA-Z0-9_]{4,20}$");
            if (!rgx.Match(newItem.Name).Success)
            {
                ModelState.AddModelError("Name", "Alphanumeric and underscores in Name only");
            }
            if (!rgx.Match(newItem.Class).Success)
            {
                ModelState.AddModelError("Class", "Alphanumeric and spaces in Class only");
            }
            if (newItem.Level < 0)
            {
                ModelState.AddModelError("Level", "Level must be greater than or equal to 0");
            }

            // Validate the input
            if (!ModelState.IsValid)
            {

                var form = m.mapper.Map<CharacterEditForm>(newItem);
                return View(form);
            }

            if (id.GetValueOrDefault() != newItem.Id)
            {
                // This appears to be data tampering, so redirect the user away
                return RedirectToAction("index");
            }

            // Attempt to do the update
            var editedItem = m.EditCharacter(newItem);



            if (editedItem == null)
            {
                // There was a problem updating the object
                // Our "version 1" approach is to display the "edit form" again
                return RedirectToAction("CharacterEdit", new { id = newItem.Id });
            }
            else
            {
                // Show the details view, which will have the updated data
                return RedirectToAction("Details", "Characters", new { id = newItem.Id });
            }
        }


        // GET: UserAccount/Delete/5
        public ActionResult Delete()
        {
            var itemToDelete = m.UserAccountGetHomeByName(User.Identity.Name);

            if (itemToDelete == null)
            {
                // Don't leak info about the delete attempt
                // Simply redirect
                return RedirectToAction("index");
            }
            else
            {
                return View(itemToDelete);
            }
        }

        //// POST: UserAccount/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int? id, FormCollection collection)
        //{
        //    var result = m.UserAccountDelete(id.GetValueOrDefault());

        //    // "result" will be true or false
        //    // We probably won't do much with the result, because 
        //    // we don't want to leak info about the delete attempt

        //    // In the end, we should just redirect to the list view
        //    return RedirectToAction("index");
        //}


        // GET: UserAccount/Delete/5
        public ActionResult DeleteCharacter(int? id)
        {
            var itemToDelete = m.CharacterGetById(id.GetValueOrDefault());

            if (itemToDelete.UserAccount.Name != User.Identity.Name)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (itemToDelete == null)
            {
                // Don't leak info about the delete attempt
                // Simply redirect
                return RedirectToAction("Details", "Character", new { id = id });
            }
            else
            {
                return View(itemToDelete);
            }
        }

        // POST: UserAccount/Delete/5
        [HttpPost]
        public ActionResult DeleteCharacter(int? userid, int? id)
        {
            var o = m.CharacterGetById(id.GetValueOrDefault());
            var result = m.CharacterDelete(o.UserAccount.Id, id.GetValueOrDefault());


            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
