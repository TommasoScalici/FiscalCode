using System.Diagnostics;
using System.Globalization;
using System.Threading;

using FiscalCode.iOS;
using FiscalCode.Localization;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocaleiOS))]

namespace FiscalCode.iOS
{
    public class LocaleiOS : ILocale
    {
        public string GetCurrent()
        {
            var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
            var iosLanguageAuto = NSLocale.AutoUpdatingCurrentLocale.LanguageCode;
            var netLocale = iosLocaleAuto.Replace('_', '-');
            var netLanguage = iosLanguageAuto.Replace('_', '-');

            Debug.WriteLine($"nslocaleid: {iosLocaleAuto}");
            Debug.WriteLine($"nslanguage: {iosLanguageAuto}");
            Debug.WriteLine($"ios: {iosLanguageAuto} {iosLocaleAuto}");
            Debug.WriteLine($"net: {netLanguage} {netLocale}");

            var ci = new CultureInfo(netLocale);
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = ci;

            Debug.WriteLine("thread:  " + Thread.CurrentThread.CurrentCulture);
            Debug.WriteLine("threadUI:" + Thread.CurrentThread.CurrentUICulture);

            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var pref = NSLocale.PreferredLanguages[0];
                netLanguage = pref.Replace("_", "-");
                Debug.WriteLine("preferred:" + netLanguage);
            }

            return netLanguage;
        }

        public void SetLocale()
        {
            var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
            var netLocale = iosLocaleAuto.Replace("_", "-");
            var ci = new CultureInfo(netLocale);
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}
