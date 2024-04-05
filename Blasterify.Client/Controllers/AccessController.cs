using Blasterify.Client.Models;
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
        public ActionResult LogIn()
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
        public ActionResult SignUp(ClientUser clientUser)
        {
            if(clientUser != null) {

            }

            return RedirectToAction("Index", "Access");
        }

        [HttpPost]
        public ActionResult LogIn(string email, string password)
        {
            Session["Email"] = email;
            Session["Username"] = "";
            return View();
        }
    }
}