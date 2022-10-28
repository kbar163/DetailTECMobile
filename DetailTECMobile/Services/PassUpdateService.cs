using DetailTECMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DetailTECMobile.Services
{
    public static class PassUpdateService
    {
        static string BaseURL = "http://10.0.2.2:7163/api/manage/";
        static HttpClient client;

        static PassUpdateService()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(BaseURL)

            };
            client.Timeout = TimeSpan.FromSeconds(12);
        }

        public static async Task<bool> UpdateRemoteCustomer(Customer customer)
        {

            var isUpdated = false;
            try
            {
                string body = JsonConvert.SerializeObject(customer);
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), BaseURL + "customer");
                request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                var actionResponse = JsonConvert.DeserializeObject<ActionResponse>(responseString);
                isUpdated = actionResponse.actualizado;
                return isUpdated;
            }
            catch(Exception e)
            {
                return isUpdated;
            }
            
        }
    }
}
