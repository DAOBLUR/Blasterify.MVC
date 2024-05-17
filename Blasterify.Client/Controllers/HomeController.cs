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
                var gatAllMovies = JsonConvert.DeserializeObject<List<Blasterify.Models.Model.MovieModel>> (jsonString);

                HttpContext.Cache.Insert("Movies", gatAllMovies, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration);
            }
            else
            {
                HttpContext.Cache.Insert("Movies", null, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration);
            }
        }

        public async Task GetLastCart()
        {
            HttpResponseMessage response = await client.GetAsync($"{MvcApplication.ServicesPath}/Rent/GetLastPreRent?clientUserId={GetClientUser().Id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var gatAllPreRents = JsonConvert.DeserializeObject<Blasterify.Models.Response.PreRentResponse>(jsonString);

                Session["Cart"] = new Blasterify.Models.Model.PreRentModel(gatAllPreRents);
            }
            else
            {
                Session["Cart"] = new Blasterify.Models.Model.PreRentModel();
            }
        }

        public async Task<bool> CreatePreRent(Blasterify.Models.Request.PreRentRequest preRentRequest)
        {
            var json = JsonConvert.SerializeObject(preRentRequest);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{MvcApplication.ServicesPath}/Rent/Create", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Guid>(jsonString);
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
            return (ClientUser) Session["ClientUser"];
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
            var cart = (Blasterify.Models.Model.PreRentModel) Session["Cart"];

            if (cart == null)
            {
                return 0;
            }
            else
            {
                return cart.PreRentItems.Count;
            }
        }

        public void AddMovieToCart(Blasterify.Models.Model.MovieModel movie)
        {
            var cart = (Blasterify.Models.Model.PreRentModel) Session["Cart"];

            if (cart != null)
            {
                cart.PreRentItems.Add(movie.Id, new Blasterify.Models.Model.PreRentItemModel(movie));
                Session["Cart"] = cart;
            }
        }

        public Blasterify.Models.Model.PreRentModel GetCart()
        {
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new Blasterify.Models.Model.PreRentModel();
            }
            var cart = (Blasterify.Models.Model.PreRentModel) Session["Cart"];
            return cart;

        }

        #endregion

        #region Views
        public async Task<ActionResult> Index()
        {
            if (VerifySession())
            {
                await GetAllMoviesAsync();
                await GetLastCart();
                var movies = HttpContext.Cache["Movies"] as List<Blasterify.Models.Model.MovieModel>;
                return View(movies);
            }
            else
            {
                //return RedirectToAction("LogIn", "Access");

                await GetAllMoviesAsync();
                var movies = HttpContext.Cache["Movies"] as List<Blasterify.Models.Model.MovieModel>;
                return View(movies);
            }
        }

        public ActionResult MyAccount()
        {
            var clientUser = (ClientUser) Session["ClientUser"] ?? new ClientUser();

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

                if (cart == null || cart.PreRentItems.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(cart.PreRentItems.Values.ToList());

            }
            else
            {
                var cart = GetCart();

                if (cart == null || cart.PreRentItems.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(cart.PreRentItems.Values.ToList());
                
                //return RedirectToAction("LogIn", "Access");
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
            var movies = HttpContext.Cache["Movies"] as List<Blasterify.Models.Model.MovieModel>;

            if (movies == null)
            {
                await GetAllMoviesAsync();
            }

            var getMovie = movies.Where(m => m.Id == id).FirstOrDefault();

            if (GetCart() == null)
            {
                Session["Cart"] = new Blasterify.Models.Response.PreRentResponse();
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
                    data = cart.PreRentItems.Values.ToList()
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
                if (cart.PreRentItems[movieId].RentDuration < 12) cart.PreRentItems[movieId].RentDuration++;
            }
            else
            {
                if (cart.PreRentItems[movieId].RentDuration > 1) cart.PreRentItems[movieId].RentDuration--;
            }

            Session["Cart"] = cart;

            return Json(
                new
                {
                    data = new
                    {
                        cartCount = GetCartCount(),
                        rentDuration = cart.PreRentItems[movieId].RentDuration
                    }
                },
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public JsonResult DeleteRentMovieRequest(int movieId)
        {
            var cart = GetCart();
            cart.PreRentItems.Remove(movieId);
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
                cart.Name = name;
                cart.Address = address;
                cart.CardNumber = cardNumber;
                cart.ClientUserId = GetClientUser().Id;

                var result = await CreatePreRent(new Blasterify.Models.Request.PreRentRequest(cart));

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
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("RentConfirmation", "Shop");
            }
        }

        #endregion

    }
}