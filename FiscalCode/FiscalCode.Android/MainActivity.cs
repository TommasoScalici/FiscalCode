using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Views;
using Xamarin.Forms.Platform.Android;

namespace FiscalCode.Droid
{
    [Activity(Label = "@string/AppName", Icon = "@mipmap/icon", Theme = "@style/MainTheme",
              MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            MobileAds.Initialize(ApplicationContext, "ca-app-pub-2308630572499783~6760055483");
            Window.DecorView.ImportantForAutofill = ImportantForAutofill.NoExcludeDescendants;

#pragma warning disable IDE0001
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
#pragma warning restore IDE0001
            LoadApplication(new App());
        }
    }
}
