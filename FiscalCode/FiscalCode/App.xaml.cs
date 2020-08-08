﻿using FiscalCode.Localization;
using FiscalCode.Views;
using Syncfusion.Licensing;
using Xamarin.Forms;

namespace FiscalCode
{
    public partial class App : Application
    {
        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense("MzAwODYxQDMxMzgyZTMyMmUzMEFxSUFyOThmaVhuUlpWclRpVVVtbkJYemlVUEorcS96WTJLaVVnWEhjYkE9");

            InitializeComponent();

            Locale.SetLocale();
            _ = DependencyService.Get<ILocale>().GetCurrent();

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#267F00")
            };
        }


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
