using System;

namespace Blasterify.Models.Yuno
{
    public class YunoCredentials
    {
        public string PublicAPIKey { get; set; }
        public string CheckoutSession { get; set; }
        public Guid RentId { get; set; }
    }
}