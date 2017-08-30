using System.Diagnostics;
using System.Globalization;
using System.Threading;

using FiscalCode.Droid;
using FiscalCode.Localization;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocaleAndroid))]

namespace FiscalCode.Droid
{
    public class LocaleAndroid : ILocale
    {
        public string GetCurrent()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLanguage = androidLocale.Language.Replace('_', '-');
            var netLocale = androidLocale.ToString().Replace('_', '-');

            Debug.WriteLine($"android: {androidLocale.ToString()}");
            Debug.WriteLine($"netlang: {netLanguage}");
            Debug.WriteLine($"netlocale: {netLocale}");

            var ci = new CultureInfo(netLocale);
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = ci;

            Debug.WriteLine($"thread: {Thread.CurrentThread.CurrentCulture}");
            Debug.WriteLine($"threadUI: {Thread.CurrentThread.CurrentUICulture}");

            return netLocale;
        }

        public void SetLocale()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLocale = androidLocale.ToString().Replace('_', '-');
            var ci = new CultureInfo(netLocale);

            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}
