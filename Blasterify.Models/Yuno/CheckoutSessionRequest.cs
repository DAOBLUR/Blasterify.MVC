namespace Blasterify.Models.Yuno
{
    public class CheckoutSessionRequest
    {
        public string Country { get; set; }
        public Amount Amount { get; set; }
        public string Customer_Id { get; set; }
        public string Merchant_Order_Id { get; set; }
        public string Payment_Description { get; set; }
        public string Account_Id { get; set; }
    }
}