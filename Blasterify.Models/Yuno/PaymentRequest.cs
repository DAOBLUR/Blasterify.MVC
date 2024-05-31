using System;
using System.Collections.Generic;
using System.Text;

namespace Blasterify.Models.Yuno
{
    public class PaymentRequest
    {
        public string Country { get; set; }
        public Amount Amount { get; set; }
        public CustomerPayer Customer_Payer { get; set; }
        public string Workflow { get; set; }
        public PaymentMethod Payment_Method { get; set; }
        public string Account_Id { get; set; }
        public string Description { get; set; }
        public string Merchant_Order_Id { get; set; }
        public Checkout.Checkout Checkout { get; set; } 
    }
}
