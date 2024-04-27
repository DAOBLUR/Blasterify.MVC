using Blasterify.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            HttpResponseMessage response = await client.GetAsync($"{MvcApplication.ServicesPath}/Movie/GetAll");
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

        public async Task<bool> CreatePreRent(PreRent preRent)
        {
            var json = JsonConvert.SerializeObject(preRent);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{MvcApplication.ServicesPath}/PreRent/Create", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<object>(jsonString);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreatePreRentItems(List<PreRentItem> preRentItems)
        {
            var json = JsonConvert.SerializeObject(preRentItems);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{MvcApplication.ServicesPath}/PreRent/CreatePreRentItems", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<object>(jsonString);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Functions

        public ClientUser GetClientUser()
        {
            return (ClientUser)Session["ClientUser"];
        }

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
            var cart = (Dictionary<int, RentMovie>)Session["Cart"];

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
            var cart = (Dictionary<int, RentMovie>)Session["Cart"];

            if (cart != null)
            {
                var rentMovie = new RentMovie(movie);

                cart.Add(movie.Id, rentMovie);
                Session["Cart"] = cart;
            }
        }

        public Dictionary<int, RentMovie> GetCart()
        {
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new Dictionary<int, RentMovie>();
            }
            var cart = (Dictionary<int, RentMovie>)Session["Cart"];
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

                if (cart == null || cart.Count == 0)
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
                Session["Cart"] = new Dictionary<int, RentMovie>();
                AddMovieToCart(getMovie);
            }
            else
            {
                AddMovieToCart(getMovie);
            }

            return Json(
                new
                {
                    isSuccess = true,
                    message = "Success",
                    cartCount = GetCartCount()
                },
                JsonRequestBehavior.AllowGet
            );
        }


        //RENT
        [HttpPost]
        public JsonResult GetCartRequest()
        {
            var cart = GetCart();

            return Json(
                new
                {
                    data = cart.Values.ToList()
                },
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public JsonResult UpdateRentDurationRequest(int movieId, bool isAdd)
        {
            var cart = GetCart();

            if (isAdd)
            {
                if (cart[movieId].RentDuration < 12) cart[movieId].RentDuration++;
            }
            else
            {
                if (cart[movieId].RentDuration > 1) cart[movieId].RentDuration--;
            }

            Session["Cart"] = cart;

            return Json(
                new
                {
                    data = new
                    {
                        cartCount = GetCartCount(),
                        rentDuration = cart[movieId].RentDuration
                    }
                },
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public JsonResult DeleteRentMovieRequest(int movieId)
        {
            var cart = GetCart();
            cart.Remove(movieId);
            Session["Cart"] = cart;

            return Json(
                new
                {
                    data = new
                    {
                        cartCount = GetCartCount(),
                    }
                },
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public async Task<JsonResult> ContinueToPaymentRequest(string name, string address, string cardNumber)
        {
            if (GetCartCount() > 0)
            {
                var cart = GetCart();
                var preRent = new PreRent()
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    ClientUserId = GetClientUser().Id
                };

                var result = false;

                if(await CreatePreRent(preRent))
                {
                    var preRentItems = new List<PreRentItem>();

                    foreach (var item in cart)
                    {
                        preRentItems.Add(new PreRentItem()
                        {
                            Id = 0,
                            MovieId = item.Value.MovieId,
                            RentId = preRent.Id,
                            RentDuration = item.Value.RentDuration,
                        });
                    };

                    result = await CreatePreRentItems(preRentItems);

                    if(result)
                    {
                        Session["PreRent"] = preRent;
                    }
                }

                return Json(
                    new Result(
                        true,
                        new
                        {
                            Url = "/Shop/RentConfirmation"
                        },
                        "Success"
                    ),
                    JsonRequestBehavior.AllowGet
                );
            }
            else
            {
                return Json(
                    new
                    {
                        status = false
                    },
                    JsonRequestBehavior.AllowGet
                );
            }
        }

        [HttpPost]
        public ActionResult GoToRentConfirmationRequest()
        {
            if (GetCartCount() > 0)
            {
                var cart = GetCart();
                return RedirectToAction("RentConfirmation", "Shop");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}