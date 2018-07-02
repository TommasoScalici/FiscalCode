using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms.Platform.Android;

namespace FiscalCode.Droid
{
    [Activity(Label = "FiscalCode", Icon = "@mipmap/icon", Theme = "@style/MainTheme",
              MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

#pragma warning disable IDE0001
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
#pragma warning restore IDE0001
            LoadApplication(new App());
        }
    }
}
