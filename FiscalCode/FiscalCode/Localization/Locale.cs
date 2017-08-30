using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;

using Xamarin.Forms;

namespace FiscalCode.Localization
{
    public class Locale
    {
        public static string GetLocale() => DependencyService.Get<ILocale>().GetCurrent();
        public static void SetLocale() => DependencyService.Get<ILocale>().SetLocale();

        public static string Localize(string key)
        {
            var netLanguage = GetLocale();
            var resourceManager = new ResourceManager("FiscalCode.Resources.AppResources", typeof(Locale).GetTypeInfo().Assembly);

            Debug.WriteLine($"Localize: {key}");
            return resourceManager.GetString(key, new CultureInfo(netLanguage));
        }
    }
}
