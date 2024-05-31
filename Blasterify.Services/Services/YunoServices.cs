using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.Text;

namespace Blasterify.Services.Services
{
    public class YunoServices
    {
        static string? PublicAPIKey;
        static string? PrivateSecretKey;
        static string UrkBase = "https://api-sandbox.y.uno/v1/";
        public static readonly string AccountId = "0d8f44ff-15fc-4a8c-b65e-fa5dcdf84ccc";

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

        public static async Task<string> SendPostMethod<T>(T entity, String method)
        {
            String UrlMetodo = UrkBase + method;
            String jsonText = String.Empty;
            HttpResponseMessage response;
            HttpClient client = new ();
            client.BaseAddress = new Uri(UrlMetodo);

            HttpRequestMessage request = new (HttpMethod.Post, UrlMetodo);

            jsonText = JsonConvert.SerializeObject(entity);

            request.Content = new StringContent(jsonText, Encoding.UTF8, "application/json");

            Random rnd = new Random();
            int num = rnd.Next();

            request.Headers.Add("accept", "application/json");
            request.Headers.Add("charset", "utf-8");
            request.Headers.Add("public-api-key", PublicAPIKey);
            request.Headers.Add("private-secret-key", PrivateSecretKey);
            request.Headers.Add("X-idempotency-key", num.ToString());

            response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }
    }
}