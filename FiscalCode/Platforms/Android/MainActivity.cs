using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;

using Plugin.Firebase.CloudMessaging;

namespace FiscalCode;

[Activity(Label = "@string/app_name", Icon = "@mipmap/appicon", Theme = "@style/Maui.SplashTheme", MainLauncher = true,
          ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                                 ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        var idiom = DeviceInfo.Idiom;

        if (idiom != DeviceIdiom.Watch)
            MobileAds.Initialize(this);

        base.OnCreate(savedInstanceState);

        if (Intent is not null)
            HandleIntent(Intent);

        CreateNotificationChannelIfNeeded();
    }

    protected override void OnNewIntent(Intent? intent)
    {
        base.OnNewIntent(intent);

        if (intent is not null)
            HandleIntent(intent);
    }

    private static void HandleIntent(Intent intent) => FirebaseCloudMessagingImplementation.OnNewIntent(intent);

    private void CreateNotificationChannelIfNeeded()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            CreateNotificationChannel();
    }

    private void CreateNotificationChannel()
    {
        var channelId = $"{PackageName}.general";

        if (GetSystemService(NotificationService) is NotificationManager notificationManager)
        {
            var channel = new NotificationChannel(channelId, "General", NotificationImportance.Default);
            notificationManager?.CreateNotificationChannel(channel);
            FirebaseCloudMessagingImplementation.ChannelId = channelId;
            //FirebaseCloudMessagingImplementation.SmallIconRef = Resource.Drawable.ic_push_small;
        }
    }
}
