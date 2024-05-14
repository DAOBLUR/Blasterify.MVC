using Blasterify.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Web.Caching;

namespace Blasterify.Client.Controllers
{
    public class ShopController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        #region SERVICES

        public async Task<Movie> GetMovieAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{MvcApplication.ServicesPath}/Movie/Get?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var gatMovie = JsonConvert.DeserializeObject<Movie>(jsonString);

                return gatMovie;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> CreateRent(Rent preRent)
        {
            var json = JsonConvert.SerializeObject(preRent);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{MvcApplication.ServicesPath}/Rent/Create", content);
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

        public async Task<bool> CreateRentItems(List<RentItem> preRentItems)
        {
            var json = JsonConvert.SerializeObject(preRentItems);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{MvcApplication.ServicesPath}/Rent/CreateRentItems", content);
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

        public async Task GetRentDetailAsync()
        {
            HttpResponseMessage response = await client.GetAsync($"{MvcApplication.ServicesPath}/Rent/GetRentDetail?rentId={(Guid)Session["RentId"]}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var gatAllMovies = JsonConvert.DeserializeObject<RentDetail>(jsonString);

                Session["RentDetail"] = gatAllMovies;
            }
        }

        #endregion

        #region VIEWS

        public async Task<ActionResult> Movie(int id)
        {
            var movie = await GetMovieAsync(id);
            return View(movie ?? new Movie());
        }

        public ActionResult RentConfirmation()
        {
            if (Session["ClientUser"] == null)
            {
                return RedirectToAction("LogIn", "Access");
            }
            else
            {
                return View();
            }
        }

        public async Task<ActionResult> CompletedRentDetail()
        {
            if (Session["ClientUser"] == null)
            {
                return RedirectToAction("LogIn", "Access");
            }
            else
            {
                await GetRentDetailAsync();
                return View(Session["RentDetail"]);
            }
        }

        #endregion

        #region REQUEST

        [HttpGet]
        public JsonResult GetCartCount()
        {
            var cart = (Dictionary<int, RentMovie>)Session["Cart"];

            if (cart == null)
            {
                
                return Json(
                    new Result(
                        false,
                        new
                        {
                            Count = 0
                        },
                        "Error"
                    ),
                    JsonRequestBehavior.AllowGet
                );
            }
            else
            {
                return Json(
                    new Result(
                        true,
                        new
                        {
                            Count = cart.Count
                        },
                        "Success"
                    ),
                    JsonRequestBehavior.AllowGet
                );
            }
        }

        [HttpPost]
        public async Task<JsonResult> PayNowRequest(DateTime expirationDate, int cvv, string cardholderName)
        {
            if(Session["PreRent"] != null)
            {
                var preRent = (PreRent) Session["PreRent"];

                var rent = new Rent() 
                {
                    Id = preRent.Id,
                    RentDate = DateTime.UtcNow,
                    ClientUserId = preRent.ClientUserId
                };

                var result = false;

                if (await CreateRent(rent))
                {
                    var rentItems = new List<RentItem>();

                    foreach (var item in (Dictionary<int, RentMovie>) Session["Cart"])
                    {
                        rentItems.Add(new RentItem()
                        {
                            Id = 0,
                            MovieId = item.Value.MovieId,
                            RentId = rent.Id,
                            RentDuration = item.Value.RentDuration,
                        });
                    };

                    result = await CreateRentItems(rentItems);

                    if (result)
                    {
                        Session["PreRent"] = null;
                        Session["Cart"] = null;
                        Session["RentId"] = preRent.Id;
                    }
                }

                return Json(
                    new Result(
                        true,
                        new
                        {
                            Url = "/Shop/CompletedRentDetail"
                        },
                        "Success"
                    ),
                    JsonRequestBehavior.AllowGet
                );
            }
            else
            {
                return Json(
                    new Result(
                        false,
                        new
                        {
                            Url = "/Home/Index"
                        },
                        "Error"
                    ),
                    JsonRequestBehavior.AllowGet
                );
            }
        }

        [HttpPost]
        public JsonResult CancelRentConfirmationRequest()
        {
            return Json(
                new Result(
                    true,
                    new
                    {
                        Url = "/Home/Index"
                    },
                    "Canceling Rent"
                ),
                JsonRequestBehavior.AllowGet
            );
        }

        #endregion
    }
}