using CommunityToolkit.Maui;

using FiscalCode.Services;

using Microsoft.Extensions.Logging;

using MudBlazor.Services;

using Microsoft.Maui.LifecycleEvents;

using Plugin.Firebase.Auth;


#if !IOS
using Plugin.MauiMTAdmob;
#endif

using Plugin.Firebase.Bundled.Shared;

#if IOS
using Plugin.Firebase.Bundled.Platforms.iOS;
#elif ANDROID
using Plugin.Firebase.Bundled.Platforms.Android;
using Plugin.Firebase.Crashlytics;
#endif

namespace FiscalCode;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>()
               .UseMauiCommunityToolkit()
#if !IOS
               .UseMauiMTAdmob()
#endif
               .RegisterFirebaseServices()
               .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

        builder.Services.AddHttpClient();
        builder.Services.AddLocalization();
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<BirthplaceDataService>();
        builder.Services.AddSingleton<FiscalCodeDataService>();
        builder.Services.AddSingleton<FiscalCodeOCRAnalyzerService>();

        return builder.Build();
    }

    private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events =>
        {
#if IOS
            events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
                CrossFirebase.Initialize(CreateCrossFirebaseSettings());
                return false;
            }));
#elif ANDROID
            events.AddAndroid(android => android.OnCreate((activity, _) =>
                CrossFirebase.Initialize(activity, CreateCrossFirebaseSettings())));

            CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
#endif
        });



        builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
        return builder;
    }

#if ANDROID || IOS
    private static CrossFirebaseSettings CreateCrossFirebaseSettings() =>
        new(
            isAuthEnabled: true,
            isAnalyticsEnabled: true,
            isCloudMessagingEnabled: true
        );
#endif
}