using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RaidExec.Controllers
{
    public class ReloadDBController : AsyncController
    {
        private Manager m = new Manager();

        // GET: ReloadDB
        public async Task<ActionResult> Index()
        {
            await m.ReloadData();
            return RedirectToAction("Index", "Home");
        }
    }
}
