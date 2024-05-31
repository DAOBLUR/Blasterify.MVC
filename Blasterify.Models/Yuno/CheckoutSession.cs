using System;

namespace Blasterify.Models.Yuno
{
    public class CheckoutSession
    {
        public string Merchant_Order_Id { get; set; }
        public string Checkout_Session { get; set; }
        public string Country { get; set; }
        public string Payment_Description { get; set; }
        public string Customer_Id { get; set; }
        public string Callback_Url { get; set; }
        public Amount Amount { get; set; }
        public DateTime Created_At { get; set; }
        public string Metadata { get; set; }
        public string Workflow { get; set; }
        public Installments Installments { get; set; }
    }
}