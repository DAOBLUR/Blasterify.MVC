using Blasterify.Services.Data;
using Blasterify.Services.Models;
using Blasterify.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Blasterify.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly DataContext _context;

        public TestController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            string userClientcountry = "US";
            int totalPrice = 322;
            string getCustomerId = "30fee24f-9e08-4d18-ac2c-80f94eb0565e";
            string getMerchantOrderId = "1020";

            var checkoutSessionRequest = new Blasterify.Models.Yuno.CheckoutSessionRequest
            {
                Country = userClientcountry,
                Amount = new Blasterify.Models.Yuno.Amount
                {
                    Currency = "USD",
                    Value = totalPrice
                },
                Customer_Id = getCustomerId,
                Merchant_Order_Id = getMerchantOrderId,
                Payment_Description = "Rent movies.",
                Account_Id = YunoServices.AccountId
            };

            var response = await YunoServices.SendPostMethod(checkoutSessionRequest, new String("checkout/sessions"));

            var checkoutSession = JsonConvert.DeserializeObject<Blasterify.Models.Yuno.CheckoutSession>(response);

            return Ok(checkoutSession);
        }

        [HttpPost]
        [Route("Create2")]
        public async Task<IActionResult> Create2(string checkoutSession, string token)
        {
            string userClientcountry = "US";
            int totalPrice = 322;
            string getCustomerId = "";
            string getMerchantOrderId = "";


            var paymentRequest = new Blasterify.Models.Yuno.PaymentRequest
            {
                Country = "BR",
                Amount = new Blasterify.Models.Yuno.Amount
                {
                    Currency = "BRL",
                    Value = 100
                },
                Customer_Payer = new Blasterify.Models.Yuno.CustomerPayer
                {
                    Customer_Id = "cfae0941-7234-427a-a739-ef4fce966c79",
                    Id = "30fee24f-9e08-4d18-ac2c-80f94eb0565e",
                    First_Name = "DAOBLUR",
                    Last_Name = "Private",
                    Nationality = "BR",
                    Email = "kpachac@ulasalle.edu.pe",
                    Merchant_Customer_Id = "1020"
                },
                Checkout = new Blasterify.Models.Yuno.Checkout.Checkout
                {
                    Session = checkoutSession,
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
                                Number = "1234567890123456",
                                Expiration_Month = 12,
                                Expiration_Year = 2024,
                                Security_Code = "123",
                                Holder_Name = "DAOBLUR Private"
                            },
                            Verify = false
                        }
                    },
                    Token = token,
                    Type = "CARD"
                },

                Account_Id = YunoServices.AccountId,
                Description = "SUCCESSFUL",
                Merchant_Order_Id = "1020"
            };

            var response = await YunoServices.SendPostMethod(paymentRequest, new String("payments"));

            var payment = JsonConvert.DeserializeObject<Blasterify.Models.Yuno.Payment.Payment>(response);

            return Ok(payment);
        }
    }
}
