using FiscalCode.Localization;
using FiscalCode.Views;
using Syncfusion.Licensing;
using Xamarin.Forms;

namespace FiscalCode
{
    public partial class App : Application
    {
        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense("MjYxNUAzMTM2MmUzMjJlMzBLampFdnVWeEVBTFk1T044bFlDOUNBNU5RNEI4bVZwNUIxa1hvd3ROcGJzPQ==");

            InitializeComponent();

            Locale.SetLocale();

            var netLanguage = DependencyService.Get<ILocale>().GetCurrent();

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
