using Newtonsoft.Json;
using System.Text;

namespace Blasterify.Services.Services
{
    public class YunoServices
    {
        private static readonly string publicAPIKey = "sandbox_gAAAAABmUMNXVdBwRm0wIsrU2cCUka1J8UoITkVo2P0gx7fRxguQI_6M1fU_51RpakcAsoxzL7rWwJOlIhZVKv-zrQyQCpMGwAXzh3RzDhnCoPZWzb415LlVIWAOz7Tm5lMpnSkGI_guHyMwaH8NdIMi78p8N6cxbggKeH4z3Fz1BmkRpXAFhyyrFfrfYar8XNvfR_7n9ZqPhjX1ELx9pNz4Zvtt_OiC7A5-IMbYOjNa1dp4mC8C-rG9x7iPzPoXD3zSHI8Jes60";
        private static readonly string privateSecretKey = "gAAAAABmUMNXbtEDkxpJXEMeVTwrSfW3fsrVmb7GC1dcbuq4Hu_rZs9XSOP0dzXMo6QsVjuTCBUtAmQ13VYlzPPcwkMlyzLR-uN6WEwL7tqB6BhZTZ5xb3QZkZDW_Km1ZPs4jS4AmmQL5NpDHIqtXm08MvLN04hXvP3J52mg_WuA9xfZJ8eGKeu0Xw4HkBREVoD-ATPtRNoU0nWxDZVQwlJOEGc84xtzUT1hfOD9AI8efBOIhw5W5-VvbAm0dnA9SZ-QgY0LyvYE";

        public static async Task CreateCustomer(Blasterify.Models.Yuno.CustomerRequest customerRequest)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("charset", "utf-8");
            client.DefaultRequestHeaders.Add("public-api-key", publicAPIKey);
            client.DefaultRequestHeaders.Add("private-secret-key", privateSecretKey);

            var json = JsonConvert.SerializeObject(customerRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api-sandbox.y.uno/v1/customers", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<object>(jsonString);
            }

            Console.WriteLine("{0}", response.Content);
        }
    }
}