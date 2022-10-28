using DetailTECMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DetailTECMobile.Services
{
    public static class AppointmentService
    {
        static string BaseURL = "http://10.0.2.2:7163/api/manage/";
        static HttpClient client;

        static AppointmentService()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(BaseURL)

            };
            client.Timeout = TimeSpan.FromSeconds(12);
        }

        public static async Task<MultivalueAppointment> GetAllAppointments()
        {
            MultivalueAppointment appointments = new MultivalueAppointment();
            try
            {
                var json = await client.GetStringAsync("appointment/all");

                appointments = JsonConvert.DeserializeObject<MultivalueAppointment>(json);
            }
            catch (Exception e)
            {
                appointments.exito = false;
            }

            return appointments;
        }
    }
}
