using Blasterify.Client.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blasterify.Client.Controllers
{
    public class AccessController : BaseController
    {
        private static readonly HttpClient client = new HttpClient();

        #region SERVICES
        public async Task<ActionResult> GetAllSubscriptionAsync()
        {
            HttpResponseMessage response = await client.GetAsync($"{MvcApplication.ServicesPath}/Subscription/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<object>(jsonString);
                return View(data);
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<ActionResult> SignUpAsync(ClientUser clientUser)
        {
            var json = JsonConvert.SerializeObject(clientUser);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{MvcApplication.ServicesPath}/ClientUser/Create", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<object>(jsonString);
                return View(data);
            }
            else
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<object>(jsonString);
                return View("Error");
            }
        }

        public async Task<bool> LogInAsync(LogIn logIn)
        {
            var json = JsonConvert.SerializeObject(logIn);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{MvcApplication.ServicesPath}/ClientUser/LogIn", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ClientUser>(jsonString);

                Session["ClientUser"] = new ClientUser()
                {
                    Id = data.Id,
                    Email = data.Email,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    CardNumber = data.CardNumber,
                };

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Helpers

        public byte[] HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                return bytes;
            }
        }

        #endregion

        #region VIEWS
        public ActionResult LogIn()
        {
            Console.WriteLine(MvcApplication.ServicesPath);
            if (Session["ClientUser"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        public ActionResult SignUp()
        {
            if (Session["ClientUser"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        public ActionResult ResetPassword()
        {
            if (Session["ClientUser"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUpRequest(string firstName, string lastName, string email, string password, string passwordConfirm, string cardNumber)
        {
            if (password == passwordConfirm)
            {
                var clientUser = new ClientUser()
                {
                    Id = 0,
                    FirstName = firstName,
                    LastName = lastName,
                    CardNumber = cardNumber,
                    Status = true,
                    Email = email,
                    PasswordHash = HashPassword(password),
                    SubscriptionDate = DateTime.UtcNow,
                    SubscriptionId = 1,
                };

                await SignUpAsync(clientUser);

                return RedirectToAction("LogIn", "Access");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogInRequest(string email, string password)
        {
            var logIn = new LogIn()
            {
                Email = email,
                PasswordHash = HashPassword(password)
            };

            var islogged = await LogInAsync(logIn);

            if (islogged)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        #endregion
    }
}