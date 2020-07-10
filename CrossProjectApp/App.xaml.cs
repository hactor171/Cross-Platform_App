using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CrossProjectApp.Services;
using CrossProjectApp.Views;
using System.Diagnostics;
using CrossProjectApp.Models;
using System.IO;

namespace CrossProjectApp
{
    public partial class App : Application
    {

        public const string DATABASE_NAME = "Mobile_database.db";
        public static DatabaseHelper database;
        public static DatabaseHelper Database
        {
            get
            {
                if (database == null)
                {
                    database = new DatabaseHelper(
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
           
        }

        protected override void OnResume()
        {
        }
    }
}
