using System;

namespace Blasterify.Models.Yuno.Payment
{
    public class CustomerPayer
    {
        public string Id { get; set; }
        public string Merchant_Customer_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public object Gender { get; set; }
        public object Date_Of_Birth { get; set; }
        public string Email { get; set; }
        public object Nationality { get; set; }
        public object Ip_Address { get; set; }
        public object Device_Fingerprint { get; set; }
        public BrowserInfo Browser_Info { get; set; }
        public object Document { get; set; }
        public object Phone { get; set; }
        public object Billing_Address { get; set; }
        public object Shipping_Address { get; set; }
        public DateTime Merchant_Customer_Created_At { get; set; }
    }
}