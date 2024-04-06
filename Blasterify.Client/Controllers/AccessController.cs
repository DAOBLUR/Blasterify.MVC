using Blasterify.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;

namespace Blasterify.Client.Controllers
{
    public class AccessController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        #region Services
        public async Task<ActionResult> GetClientUser()
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7276/api/Subscription/GetAll");
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

            HttpResponseMessage response = await client.PostAsync("https://localhost:7276/api/ClientUser/Create", content);
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

        public async Task<bool> LogInAsync(LogIn logIn)
        {
            var json = JsonConvert.SerializeObject(logIn);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://localhost:7276/api/ClientUser/LogIn", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ClientUser>(jsonString);

                Session["Email"] = data.Email;
                Session["Username"] = data.Username;
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

        #region Views
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
        public async Task<ActionResult> SignUp(string username, string email, string password, string passwordConfirm, string cardNumber)
        {
            if(password == passwordConfirm) {
                var clientUser = new ClientUser()
                {
                    Id = 0,
                    Username = username,
                    CardNumber = cardNumber,
                    Status = true,
                    Email = email,
                    PasswordHash = HashPassword(password),
                    SuscriptionDate = DateTime.UtcNow,
                    SubscriptionId = 1,
                };
                
                await SignUpAsync(clientUser);

                return RedirectToAction("LogIn", "Access");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LogIn(string email, string password)
        {
            var logIn = new LogIn()
            {
                Email = email,
                PasswordHash = HashPassword(password)
            };

            var islogged = await LogInAsync(logIn);

            if(islogged)
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