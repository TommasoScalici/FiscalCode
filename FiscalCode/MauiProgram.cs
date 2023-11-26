using CommunityToolkit.Maui;

using FiscalCode.Services;

using Microsoft.Extensions.Logging;

using MudBlazor.Services;

#if !IOS
using Plugin.MauiMTAdmob;
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
}
