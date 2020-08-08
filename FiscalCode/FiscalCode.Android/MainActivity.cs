﻿using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Views;

using FiscalCode.Views;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace FiscalCode.Droid
{
    [Activity(Icon = "@mipmap/icon", Label = "@string/AppName", Theme = "@style/MainTheme",
              MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        static internal Activity Instance { get; private set; }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            DependencyService.Register<Message>();
            DependencyService.Register<PhotoLibrary>();

            MobileAds.Initialize(ApplicationContext, "ca-app-pub-2308630572499783~6760055483");
            Window.DecorView.ImportantForAutofill = ImportantForAutofill.NoExcludeDescendants;

            MessagingCenter.Subscribe<CardPage>(this, "Orientation.Unspecified", sender =>
                                                RequestedOrientation = ScreenOrientation.Unspecified);

            MessagingCenter.Subscribe<CardPage>(this, "Orientation.ForceLandScape", sender =>
                                                RequestedOrientation = ScreenOrientation.Landscape);


            base.OnCreate(savedInstanceState);

#pragma warning disable IDE0001
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
#pragma warning restore IDE0001

            Instance = this;
            LoadApplication(new App());
        }
    }
}
