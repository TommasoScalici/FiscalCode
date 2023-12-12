using Android.App;
using Android.Runtime;

using static Android.Manifest;

[assembly: MetaData("com.google.android.gms.ads.APPLICATION_ID", Value = "ca-app-pub-2308630572499783~6760055483")]

// Needed for Internet access
[assembly: UsesPermission(Permission.AccessNetworkState)]
[assembly: UsesPermission(Permission.Internet)]

//// Needed for Picking photo/vid
[assembly: UsesPermission(Permission.ReadExternalStorage, MaxSdkVersion = 32)]
[assembly: UsesPermission(Permission.ReadMediaAudio)]
[assembly: UsesPermission(Permission.ReadMediaImages)]
[assembly: UsesPermission(Permission.ReadMediaVideo)]

// Needed for Taking photo/vide
[assembly: UsesPermission(Permission.Camera)]
[assembly: UsesPermission(Permission.WriteExternalStorage, MaxSdkVersion = 32)]

// Filter devices that not have camera
[assembly: UsesFeature("android.hardware.camera", Required = true)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = true)]

namespace FiscalCode;

[Application]
public class MainApplication(IntPtr handle, JniHandleOwnership ownership)
    : MauiApplication(handle, ownership)
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
