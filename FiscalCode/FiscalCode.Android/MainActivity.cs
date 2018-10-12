using System.IO;

using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using FiscalCode.Views;
using Plugin.Permissions;
using Plugin.Toasts;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace FiscalCode.Droid
{
    [Activity(Icon = "@mipmap/icon", Label = "@string/AppName", Theme = "@style/MainTheme",
              MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            DependencyService.Register<ToastNotification>();
            ToastNotification.Init(this);

            MobileAds.Initialize(ApplicationContext, "ca-app-pub-2308630572499783~6760055483");
            Window.DecorView.ImportantForAutofill = ImportantForAutofill.NoExcludeDescendants;

            MessagingCenter.Subscribe<CardPage>(this, "Orientation.Unspecified", sender =>
                                                RequestedOrientation = ScreenOrientation.Unspecified);

            MessagingCenter.Subscribe<CardPage>(this, "Orientation.ForceLandScape", sender =>
                                                RequestedOrientation = ScreenOrientation.Landscape);

            MessagingCenter.Subscribe<CardPage, SKImage>(this, "SaveToGallery", (sender, e) => SaveToGallery(e));
            MessagingCenter.Subscribe<CardPage, SKImage>(this, "Share", (sender, e) => Share(e));

            base.OnCreate(savedInstanceState);

#pragma warning disable IDE0001
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
#pragma warning restore IDE0001

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void SaveToGallery(SKImage image)
        {
            var dateTime = System.DateTime.Now;
            var savePath = $"{Environment.ExternalStorageDirectory}/DCIM/Camera/IMG_"
                           + $"{dateTime.Year}{dateTime.Month}{dateTime.Day}_{dateTime.Hour}{dateTime.Minute}{dateTime.Second}.jpg";

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Permission.Granted
                && !File.Exists(savePath))
            {
                var file = File.Create(savePath);
                var skData = image.Encode(SKEncodedImageFormat.Jpeg, 100);
                skData.SaveTo(file);
            }
        }

        void Share(SKImage image)
        {
            var dateTime = System.DateTime.Now;
            var savePath = $"{CacheDir}/IMG_"
                           + $"{dateTime.Year}{dateTime.Month}{dateTime.Day}_{dateTime.Hour}{dateTime.Minute}{dateTime.Second}.jpg";

            if (!File.Exists(savePath))
            {
                var file = File.Create(savePath);
                var skData = image.Encode(SKEncodedImageFormat.Jpeg, 100);
                skData.SaveTo(file);

                var javaFile = new Java.IO.File(savePath);
                var uri = FileProvider.GetUriForFile(this, $"{Application.PackageName}.fileprovider", javaFile);
                var intent = new Intent(Intent.ActionSend);

                intent.SetType("image/*");
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                intent.PutExtra(Intent.ExtraStream, uri);

                StartActivity(Intent.CreateChooser(intent, string.Empty));
            }
        }
    }
}
