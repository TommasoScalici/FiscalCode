using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FiscalCode.UWP
{
    sealed partial class App : Application
    {
        public App() => InitializeComponent();


        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            //ApplicationLanguages.PrimaryLanguageOverride = "en";
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
                Xamarin.Forms.Forms.Init(args);
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
                rootFrame.Navigate(typeof(MainPage), args.Arguments);

            Window.Current.Activate();
        }
    }
}
