using DetailTECMobile.Data;
using DetailTECMobile.Models;
using DetailTECMobile.Services;
using DetailTECMobile.Views;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DetailTECMobile
{
    public partial class App : Application
    {
        public static DatabaseContext database;
        public static Customer loggedUser { get; set; }
        public static List<string> horarios { get; set; }
        

        public static bool FirstRun
        {
            get => Preferences.Get(nameof(FirstRun), true);
            set => Preferences.Set(nameof(FirstRun), value);
        }

        public static DatabaseContext Database
        {
            get
            {
                if(database == null)
                {
                    database = new DatabaseContext(Path.Combine
                        (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DetailDB.db"));
                }
               
                return database;
            }
        }
        public App()
        {
            InitializeComponent();
            MainPage = new LoginView();
            loggedUser = new Customer();
            horarios = new List<string>();
        }

        protected override void OnStart()
        {
            
            if(FirstRun)
            {
                Database.CreateCustomerTable();
                Database.CreateAddressTable();
                Database.CreatePhoneTable();
                Database.CreateBillTable();
                Database.CreateOfficeTable();
                Database.CreateWashTable();
                Database.CreateAppointmentTable();
                Database.CreatePendingAppTable();
                FirstRun = false;
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
