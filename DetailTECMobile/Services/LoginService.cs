using DetailTECMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DetailTECMobile.Services
{
    public static class LoginService
    {
        static string BaseURL = "http://10.0.2.2:7163/api/manage/";
        static HttpClient client;

        static LoginService()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(BaseURL)
            };
        }

        public static async Task<MultivalueCustomer> GetAllCustomers()
        {
            MultivalueCustomer customers = new MultivalueCustomer();
            try
            {
                var json = await client.GetStringAsync("customer/all");
                customers = JsonConvert.DeserializeObject<MultivalueCustomer>(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return customers;
        }
    }
}
