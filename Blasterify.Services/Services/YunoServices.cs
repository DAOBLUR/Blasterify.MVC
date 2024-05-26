using Newtonsoft.Json;
using System.Text;

namespace Blasterify.Services.Services
{
    public class YunoServices
    {
        static string? PublicAPIKey;
        static string? PrivateSecretKey;
        public static readonly List<string> ErrorCodes = new () {
            "CUSTOMER_ID_DUPLICATED"
        };

        public static void SetKeys(string publicAPIKey, string privateSecretKey)
        {
            PublicAPIKey = publicAPIKey;
            PrivateSecretKey = privateSecretKey;
        }

        public static async Task<string> CreateCustomer(Blasterify.Models.Yuno.CustomerRequest customerRequest)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("charset", "utf-8");
            client.DefaultRequestHeaders.Add("public-api-key", PublicAPIKey);
            client.DefaultRequestHeaders.Add("private-secret-key", PrivateSecretKey);
            
            var json = JsonConvert.SerializeObject(customerRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await client.PostAsync("https://api-sandbox.y.uno/v1/customers", content);

            var jsonString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<Blasterify.Models.Yuno.CustomerRequest>(jsonString);
                return data!.id;
            }
            else
            {
                var data = JsonConvert.DeserializeObject<Blasterify.Models.Yuno.ErrorResponse>(jsonString);
                return data!.Code;
            }
        }
    }
}