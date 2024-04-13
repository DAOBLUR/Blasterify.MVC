using Blasterify.Client.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace Blasterify.Client.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        #region Services

        public async Task GetAllMoviesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7276/api/Movie/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var gatAllMovies = JsonConvert.DeserializeObject<List<Movie>>(jsonString);

                HttpContext.Cache.Insert("Movies", gatAllMovies, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration);
            }
            else
            {
                HttpContext.Cache.Insert("Movies", null, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration);
            }
        }

        #endregion

        #region Functions

        public bool VerifySession()
        {
            if (Session["ClientUser"] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int GetCartCount()
        {
            var cart = (Dictionary<int, Movie>)Session["Cart"];

            if (cart == null)
            {
                return 0;
            }
            else
            {
                return cart.Count;
            }
        }

        public void AddMovieToCart(Movie movie)
        {
            var cart = (Dictionary<int, Movie>)Session["Cart"];

            if (cart != null)
            {
                cart.Add(movie.Id, movie);
                Session["Cart"] = cart;
            }
        }

        public Dictionary<int, Movie> GetCart()
        {
            if(Session["Cart"] == null)
            {
                Session["Cart"] = new Dictionary<int, Movie>();
            }
            var cart = (Dictionary<int, Movie>)Session["Cart"];
            return cart;
            
        }
        #endregion

        #region Views
        public async Task<ActionResult> Index()
        {
            if (VerifySession())
            {
                await GetAllMoviesAsync();
                var movies = HttpContext.Cache["Movies"] as List<Movie>;
                return View(movies);
            }
            else
            {
                return RedirectToAction("LogIn", "Access");
            }
        }

        public ActionResult MyAccount()
        {
            var clientUser = (ClientUser)Session["ClientUser"];

            return View(clientUser);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult Cart()
        {
            if (VerifySession())
            {
                var cart = GetCart();

                if(cart == null) 
                {
                     return RedirectToAction("Index", "Home");
                }
                
                return View(cart.Values.ToList());

            }
            else
            {
                return RedirectToAction("LogIn", "Access");
            }
        }

        #endregion

        #region Request

        [HttpPost]
        public ActionResult LogOut()
        {
            if (VerifySession())
            {
                Session["ClientUser"] = null;
                Session["Cart"] = null;
                return RedirectToAction("LogIn", "Access");
            }
            else
            {
                return RedirectToAction("LogIn", "Access");
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddToCart(int id)
        {
            var movies = HttpContext.Cache["Movies"] as List<Movie>;

            if (movies == null)
            {
                await GetAllMoviesAsync();
            }

            var getMovie = movies.Where(m => m.Id == id).FirstOrDefault();

            if (GetCart() == null)
            {
                Session["Cart"] = new Dictionary<int, Movie>();
                AddMovieToCart(getMovie);
            }
            else
            {
                AddMovieToCart(getMovie);
            }

            return Json(new { isSuccess = true, message = "Success", cartCount = GetCartCount() }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}