﻿using DetailTECMobile.Data;
using DetailTECMobile.Models;
using DetailTECMobile.Views;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DetailTECMobile
{
    public partial class App : Application
    {
        public static DatabaseContext database;
        public static Customer loggedUser { get; set; }

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
            Database.CreateCustomerTable();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
