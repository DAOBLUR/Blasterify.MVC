using Blasterify.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Web.Caching;
using System.Linq;

namespace Blasterify.Client.Controllers
{
    public class ShopController : BaseController
    {
        private static readonly HttpClient client = new HttpClient();

        #region SERVICES

        public async Task<Blasterify.Models.Model.MovieModel> GetMovieAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{MvcApplication.ServicesPath}/Movie/Get?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var gatMovie = JsonConvert.DeserializeObject< Blasterify.Models.Model.MovieModel> (jsonString);

                return gatMovie;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> CompleteRentAsync()
        {
            var cart = GetCart();
            var json = JsonConvert.SerializeObject(cart.Id);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"{MvcApplication.ServicesPath}/Rent/CompleteRent", content);
            if (response.IsSuccessStatusCode)
            {
                Session["RentId"] = cart.Id;
                Session["Cart"] = null;

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Blasterify.Models.Model.RentDetailModel> GetRentDetailAsync()
        {
            HttpResponseMessage response = await client.GetAsync($"{MvcApplication.ServicesPath}/Rent/GetRentDetail?rentId={(Guid)Session["RentId"]}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var gatAllMovies = JsonConvert.DeserializeObject<Blasterify.Models.Model.RentDetailModel>(jsonString);

                Session["RentDetail"] = gatAllMovies;

                return gatAllMovies;
            }
            else 
            {
                return null; 
            }
        }

        #endregion

        #region VIEWS

        public async Task<ActionResult> Movie(int id)
        {
            var movie = await GetMovieAsync(id);
            return View(movie ?? new Blasterify.Models.Model.MovieModel());
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

        public ActionResult CompletedRentDetail()
        {
            if (Session["ClientUser"] == null)
            {
                return RedirectToAction("LogIn", "Access");
            }

            return View();
        }

        #endregion

        #region REQUEST

        [HttpGet]
        public async Task<JsonResult> GetRentDetailRequest()
        {
            if (Session["RentDetail"] == null)
            {
                var rentDetail = await GetRentDetailAsync();

                return Json(
                    new Result(
                        true,
                        new
                        {
                            rentDetail = rentDetail ?? new Blasterify.Models.Model.RentDetailModel()
                        },
                        "Success"
                    ),
                    JsonRequestBehavior.AllowGet
                );
            }
            else
            {
                var rentDetail = Session["RentDetail"] as Blasterify.Models.Model.RentDetailModel;

                return Json(
                    new Result(
                        true,
                        new
                        {
                            rentDetail = rentDetail ?? new Blasterify.Models.Model.RentDetailModel()
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
            if(expirationDate.Year < DateTime.UtcNow.Year)
            {
                return Json(
                    new Result(
                        false,
                        new
                        {
                            Url = "/Home/Index"
                        },
                        "Invalid Card Expiration Date"
                    ),
                    JsonRequestBehavior.AllowGet
                );
            }

            if(cvv < 100 || cvv > 999) 
            {
                return Json(
                    new Result(
                        false,
                        new
                        {
                            Url = "/Home/Index"
                        },
                        "Invalid CVV"
                    ),
                    JsonRequestBehavior.AllowGet
                );
            }
            if(string.IsNullOrEmpty(cardholderName))
            {
                return Json(
                   new Result(
                       false,
                       new
                       {
                           Url = "/Home/Index"
                       },
                       "Invalid Card Holder Name"
                   ),
                   JsonRequestBehavior.AllowGet
               );
            }

            if(GetCart() != null && await CompleteRentAsync())
            {
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