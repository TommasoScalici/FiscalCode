using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;

namespace FiscalCode;

[Activity(Label = "@string/app_name", Icon = "@mipmap/appicon", Theme = "@style/Maui.SplashTheme", MainLauncher = true,
          ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                                 ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        MobileAds.Initialize(this);
        base.OnCreate(savedInstanceState);
    }
}
