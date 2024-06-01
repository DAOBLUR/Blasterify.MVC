using Blasterify.Services.Data;
using Blasterify.Services.Models;
using Blasterify.Services.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;


namespace Blasterify.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly DataContext _context;

        public RentController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<Blasterify.Models.Yuno.YunoCredentials>> Create(Blasterify.Models.Request.PreRentRequest preRent)
        {
            var id = Guid.NewGuid();

            var yunoCredentials = new Blasterify.Models.Yuno.YunoCredentials();

            if (preRent.Id == Guid.Empty || preRent.Date == DateTime.MinValue)
            {
                // Yuno
                var getMerchantCustomerId = (await _context!.ClientUsers!.FindAsync(preRent.ClientUserId))!.MerchantOrderId;

                var getCustomerRequest = await YunoServices.GetCustomer(getMerchantCustomerId!);
                var getCustomer = JsonConvert.DeserializeObject<Blasterify.Models.Yuno.CustomerPayer>(getCustomerRequest);

                double totalPrice = 0;

                foreach (var item in preRent.PreRentItems!)
                {
                    totalPrice += item.Price;
                }

                var checkoutSessionRequest = new Blasterify.Models.Yuno.CheckoutSessionRequest
                {
                    Country = "US",
                    Amount = new Blasterify.Models.Yuno.Amount
                    {
                        Currency = "USD",
                        Value = Convert.ToInt32(totalPrice)
                    },
                    Customer_Id = getCustomer!.Id,
                    Merchant_Order_Id = getCustomer!.Merchant_Customer_Id,
                    Payment_Description = "Rent movies.",
                    Account_Id = YunoServices.AccountId
                };

                var response = await YunoServices.SendPostMethod(checkoutSessionRequest, new String("checkout/sessions"));

                var checkoutSession = JsonConvert.DeserializeObject<Blasterify.Models.Yuno.CheckoutSession>(response);
                
                //--------------------------------------------
                
                var rent = new Rent()
                {
                    Id = id,
                    Date = DateTime.UtcNow,
                    Name = preRent.Name,
                    Address = preRent.Address,
                    CardNumber = preRent.CardNumber,
                    IsEnabled = true, //For Cart
                    ClientUserId = preRent.ClientUserId,
                    StatusId = 2, //Pending
                    CheckoutSession = checkoutSession!.Checkout_Session
                };
                await _context.Rents!.AddAsync(rent);

                foreach (var item in preRent.PreRentItems!)
                {
                    await _context.RentItems!.AddAsync(new RentItem()
                    {
                        Id = 0,
                        Price = item.Price,
                        RentDuration = item.RentDuration,
                        RentId = id,
                        MovieId = item.MovieId
                    });
                }

                yunoCredentials.RentId = id;
                yunoCredentials.PublicAPIKey = YunoServices.GetPublicAPIKey();
                yunoCredentials.CheckoutSession = checkoutSession!.Checkout_Session;
            }
            else
            {
                // Yuno
                var getMerchantCustomerId = (await _context!.ClientUsers!.FindAsync(preRent.ClientUserId))!.MerchantOrderId;

                var getCustomerRequest = await YunoServices.GetCustomer(getMerchantCustomerId!);
                var getCustomer = JsonConvert.DeserializeObject<Blasterify.Models.Yuno.CustomerPayer>(getCustomerRequest);

                double totalPrice = 0;

                foreach (var item in preRent.PreRentItems!)
                {
                    totalPrice += item.Price;
                }

                var checkoutSessionRequest = new Blasterify.Models.Yuno.CheckoutSessionRequest
                {
                    Country = "US",
                    Amount = new Blasterify.Models.Yuno.Amount
                    {
                        Currency = "USD",
                        Value = Convert.ToInt32(totalPrice)
                    },
                    Customer_Id = getCustomer!.Id,
                    Merchant_Order_Id = getCustomer!.Merchant_Customer_Id,
                    Payment_Description = "Rent movies.",
                    Account_Id = YunoServices.AccountId
                };

                var response = await YunoServices.SendPostMethod(checkoutSessionRequest, new String("checkout/sessions"));

                var checkoutSession = JsonConvert.DeserializeObject<Blasterify.Models.Yuno.CheckoutSession>(response);

                //--------------------------------------------

                id = preRent.Id;
                var rent = await _context!.Rents!.FindAsync(id);

                rent!.Date = DateTime.UtcNow;
                rent!.Name = preRent.Name;
                rent!.Address = preRent.Address;
                rent!.CardNumber = preRent.CardNumber;
                rent!.IsEnabled = true; //For Cart
                rent!.StatusId = 2; //Pending
                rent!.CheckoutSession = checkoutSession!.Checkout_Session;

                //await _context.SaveChangesAsync();

                var rentItems = await _context!.RentItems!.Where(pr => pr.RentId == preRent.Id).ToListAsync();

                foreach (var item in preRent.PreRentItems!)
                {
                    var rentItem = rentItems.Find(ri => ri.Id == item.Id);

                    if (rentItem == null)
                    {
                        await _context.RentItems!.AddAsync(new RentItem()
                        {
                            Id = 0,
                            Price = item.Price,
                            RentDuration = item.RentDuration,
                            RentId = preRent.Id,
                            MovieId = item.MovieId
                        });
                    }
                    else
                    {
                        rentItem.Price = item.Price;
                        rentItem.RentDuration = item.RentDuration;

                        rentItems.Remove(rentItem);

                        await _context.SaveChangesAsync();
                    }
                }

                foreach (var item in rentItems)
                {
                    _context.RentItems!.Remove(item);
                }

                yunoCredentials.RentId = id;
                yunoCredentials.PublicAPIKey = YunoServices.GetPublicAPIKey();
                yunoCredentials.CheckoutSession = checkoutSession!.Checkout_Session;
            }

            await _context.SaveChangesAsync();

            return Ok(yunoCredentials);
        }

        [HttpPost]
        [Route("CreateRentItems")]
        public async Task<IActionResult> CreateRentItems(List<RentItem> rentItems)
        {
            for (int i = 0; i < rentItems.Count; i++)
            {
                await _context!.RentItems!.AddAsync(rentItems[i]);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetAllRentsClientUser")]
        public async Task<ActionResult<IEnumerable<Rent>>> GetAllRentsClientUser(int clientUserId)
        {
            var rents = await _context!.Rents!.Where(r => r.ClientUserId == clientUserId).ToListAsync();
            return Ok(rents);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Rent>>> GetAll()
        {
            var rents = await _context!.Rents!.ToListAsync();
            return Ok(rents);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(Guid id)
        {
            var rent = await _context!.Rents!.FindAsync(id);

            if (rent == null)
            {
                return NotFound();
            }

            return Ok(rent);
        }

        [HttpGet]
        [Route("GetRentDetail")]
        public async Task<IActionResult> GetRentDetail(Guid rentId)
        {
            var rent = await _context.Rents!.FindAsync(rentId);

            var rentItems = await _context.RentItems!.Where(ri => ri.RentId == rentId).ToListAsync();

            var rentDetail = new Blasterify.Models.Model.RentDetailModel()
            {
                Id = rentId,
                Date = rent!.Date,
                Name = rent.Name,
                Address = rent.Address,
                CardNumber = rent.CardNumber,
                RentItemDetailModels = new List<Blasterify.Models.Model.RentItemDetailModel>(rentItems.Count)
            };

            foreach (var item in rentItems)
            {
                var movie = await _context.Movies!.FindAsync(item.MovieId);
                rentDetail.RentItemDetailModels.Add(new Blasterify.Models.Model.RentItemDetailModel()
                {
                    MovieId = item.MovieId,
                    RentDuration = item.RentDuration,
                    Title = movie!.Title,
                    Duration = movie.Duration,
                    Description = movie.Description,
                    FirebasePosterId = movie.FirebasePosterId,
                    Price = item.Price
                });
            }

            return Ok(rentDetail);
        }

        [HttpGet]
        [Route("GetLastPreRent")]
        public async Task<IActionResult> GetLastPreRent(int clientUserId)
        {
            var getPreRent = new Blasterify.Models.Response.PreRentResponse
            {
                PreRentItems = new List<Blasterify.Models.Response.PreRentItemResponse>()
            };

            SqlParameter parameter = new("@ClientUserId", clientUserId);
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "GetLastPreRent";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(parameter);

                _context.Database.OpenConnection();

                using var result = await command.ExecuteReaderAsync();
                while (await result.ReadAsync())
                {
                    getPreRent.Id = result.GetGuid(0);
                    getPreRent.Date = result.GetDateTime(1);
                    getPreRent.ClientUserId = result.GetInt32(2);
                }
            }

            if (getPreRent.Id == Guid.Empty && getPreRent.Date == default && getPreRent.ClientUserId == default)
            {
                return NotFound();
            }

            parameter = new("@PreRentId", getPreRent.Id);
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "GetLastPreRentItems";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(parameter);

                _context.Database.OpenConnection();

                using var result = await command.ExecuteReaderAsync();
                while (await result.ReadAsync())
                {
                    var preRentItem = new Blasterify.Models.Response.PreRentItemResponse
                    {
                        Id = result.GetInt32(0),
                        MovieId = result.GetInt32(1),
                        RentDuration = result.GetInt32(2),
                        Title = result.GetString(3),
                        FirebasePosterId = result.GetString(4),
                        Price = (double)result.GetDecimal(5),
                    };

                    getPreRent.PreRentItems!.Add(preRentItem);
                }
            }

            if (getPreRent.PreRentItems!.Count <= 0)
            {
                return NotFound();
            }

            return Ok(getPreRent);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Guid id, Rent rent)
        {
            var getRent = await _context!.Rents!.FindAsync(id);
            getRent!.Date = rent.Date;
            getRent!.ClientUserId = rent.ClientUserId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("CompleteRent")]
        public async Task<IActionResult> CompleteRent(Blasterify.Models.Model.CompleteRentRequest completeRentRequest)
        {
            var getRent = await _context!.Rents!.FindAsync(completeRentRequest.RentId);

            if (getRent == null)
            {
                return NotFound();
            }

            var getRentItems = await _context!.RentItems!.Where(ri => ri.RentId == completeRentRequest.RentId).ToListAsync();
            double totalPrice = 0;
            string message = "";

            foreach (var item in getRentItems!)
            {
                totalPrice += item.Price * item.RentDuration;
                var getMovie = await _context.Movies!.FindAsync(item.MovieId);
                message += $"\n- {getMovie!.Title} - {item.Price}";
            }

            // YUNO
            var getCustomerRequest = await YunoServices.GetCustomer($"{getRent.ClientUserId!}");
            var getCustomer = JsonConvert.DeserializeObject<Blasterify.Models.Yuno.CustomerPayer>(getCustomerRequest);

            var paymentRequest = new Blasterify.Models.Yuno.PaymentRequest
            {
                Country = "US",
                Amount = new Blasterify.Models.Yuno.Amount
                {
                    Currency = "USD",
                    Value = Convert.ToInt32(totalPrice)
                },
                Customer_Payer = new Blasterify.Models.Yuno.CustomerPayer
                {
                    Customer_Id = getCustomer!.Id,//"cfae0941-7234-427a-a739-ef4fce966c79",
                    Id = getCustomer.Id,
                    First_Name = getCustomer.First_Name,
                    Last_Name = getCustomer.Last_Name,
                    Nationality = "US",
                    Email = getCustomer.Email,
                    Merchant_Customer_Id = getCustomer.Merchant_Customer_Id,
                },
                Checkout = new Blasterify.Models.Yuno.Checkout.Checkout
                {
                    Session = getRent.CheckoutSession,
                },
                Workflow = "SDK_CHECKOUT",
                Payment_Method = new Blasterify.Models.Yuno.PaymentMethod
                {
                    Detail = new Blasterify.Models.Yuno.Detail
                    {
                        Card = new Blasterify.Models.Yuno.Card
                        {
                            Card_Data = new Blasterify.Models.Yuno.CardData
                            {
                                Number = getRent.CardNumber,
                                Expiration_Month = 12,
                                Expiration_Year = 2024,
                                Security_Code = "123",
                                Holder_Name = getRent.Name
                            },
                            Verify = false
                        }
                    },
                    Token = completeRentRequest.Token,
                    Type = "CARD"
                },

                Account_Id = YunoServices.AccountId,
                Description = "SUCCESSFUL",
                Merchant_Order_Id = getCustomer.Merchant_Customer_Id,
            };

            var response = await YunoServices.SendPostMethod(paymentRequest, new String("payments"));

            var payment = JsonConvert.DeserializeObject<Blasterify.Models.Yuno.Payment.Payment>(response);

            //--------------------------------------------

            BackgroundJob.Schedule(() => Email.FinishRent(getCustomer.Email, $"{getCustomer.First_Name} {getCustomer.Last_Name}", message), new DateTimeOffset(DateTime.UtcNow));

            getRent!.StatusId = 1;
            getRent!.IsEnabled = false;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var rent = await _context!.Rents!.FindAsync(id);
            _context.Rents.Remove(rent!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}