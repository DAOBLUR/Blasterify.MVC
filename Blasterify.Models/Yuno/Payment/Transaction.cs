using System;

namespace Blasterify.Models.Yuno.Payment
{
    public class Transaction
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }
        public string Provider_Id { get; set; }
        public PaymentMethod Payment_Method { get; set; }
        public string Response_Code { get; set; }
        public string Response_Message { get; set; }
        public object Reason { get; set; }
        public string Description { get; set; }
        public string Merchant_Reference { get; set; }
        public ProviderData Provider_Data { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
    }
}