using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blasterify.Client.Controllers
{
    public class AccessController : Controller
    {
        // GET: Access
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SignUp(object client)
        {
            //return View();

            return RedirectToAction("Index", "Access");
        }

        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            return View();
        }
    }
}