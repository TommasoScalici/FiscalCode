using System.Globalization;

using FiscalCode.Localization;
using FiscalCode.Resources;
using FiscalCode.Views;
using Xamarin.Forms;

namespace FiscalCode
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Locale.SetLocale();

            var netLanguage = DependencyService.Get<ILocale>().GetCurrent();
            AppResources.Culture = new CultureInfo(netLanguage);

            MainPage = new NavigationPage(new MainPage());
        }


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
