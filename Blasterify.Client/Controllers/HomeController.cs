using Blasterify.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blasterify.Client.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        public List<Movie> MyMovies = new List<Movie>();

        #region Services
        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7276/api/Movie/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Movie>>(jsonString);
                return data;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Views
        public async Task<ActionResult> Index()
        {
            MyMovies = await GetAllMoviesAsync();
            Session["MyMovies"] = MyMovies;
            return View(MyMovies);
        }

        [HttpPost]
        public ActionResult Index(int id)
        {
            Session["auto"] = id;
            if (MyMovies.Count == 0) MyMovies = (List<Movie>)Session["MyMovies"];
            return View(MyMovies);
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            return RedirectToAction("LogIn", "Access");
        }

        [HttpPost]
        public ActionResult SetSessionValue(int id)
        {
            Session["auto"] = id;
            if (MyMovies.Count == 0) MyMovies = (List<Movie>)Session["MyMovies"];
            return Json(new { success = true, message = "Invalid email or password" });
        }
        
        public ActionResult MyAccount()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        #endregion
    }
}