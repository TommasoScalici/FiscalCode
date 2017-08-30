using System.Globalization;

using FiscalCode.Localization;
using FiscalCode.UWP;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocaleUWP))]

namespace FiscalCode.UWP
{
    public class LocaleUWP : ILocale
    {
        public string GetCurrent()
        {
            var lang = CultureInfo.CurrentUICulture.Name;
            var culture = CultureInfo.CurrentCulture.Name;
            return lang;
        }

        public void SetLocale()
        {

        }
    }
}
