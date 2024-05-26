namespace Blasterify.Models.Yuno
{
    public class CustomerRequest
    {
        public string id { get; set; }
        public string merchant_customer_id { get; set; }
        public string merchant_customer_created_at { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string nationality { get; set; }
        public string country { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public CustomerRequest() { }
    }
}