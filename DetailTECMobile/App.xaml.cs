using DetailTECMobile.Data;
using DetailTECMobile.Models;
using DetailTECMobile.Views;
using System;
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
        }

        protected override void OnStart()
        {
            if(FirstRun)
            {
                Database.CreateCustomerTable();
                Database.CreateAddressTable();
                Database.CreatePhoneTable();
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
