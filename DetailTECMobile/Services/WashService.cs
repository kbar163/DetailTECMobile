using DetailTECMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DetailTECMobile.Services
{
    public static class WashService
    {


        static string BaseURL = "http://10.0.2.2:7163/api/manage/";
        static HttpClient client;

        static WashService()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(BaseURL)

            };
            client.Timeout = TimeSpan.FromSeconds(12);
        }

        public static async Task<MultivalueWash> GetAllWashTypes()
        {
            MultivalueWash washtypes = new MultivalueWash();
            try
            {
                var json = await client.GetStringAsync("wash/all");

                washtypes = JsonConvert.DeserializeObject<MultivalueWash>(json);
            }
            catch (Exception e)
            {
                washtypes.exito = false;
            }

            return washtypes;
        }
    }
}

