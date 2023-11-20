using FiscalCode.Services;

using Microsoft.Extensions.Logging;

using MudBlazor.Services;

namespace FiscalCode;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>()
               .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

        builder.Services.AddHttpClient();
        builder.Services.AddLocalization();
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddScoped<FiscalCodeDataService>();
        builder.Services.AddSingleton<BirthplaceDataService>();

        return builder.Build();
    }
}
