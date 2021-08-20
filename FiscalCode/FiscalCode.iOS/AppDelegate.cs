
using FiscalCode.Utilities;
using FiscalCode.Views;

using Foundation;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfAutoComplete.XForms.iOS;
using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FiscalCode.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            SfAutoCompleteRenderer.Init();
            SfListViewRenderer.Init();
            Forms.Init();

            DependencyService.Register<Message>();
            DependencyService.Register<PhotoLibrary>();


            UINavigationBar.Appearance.TintColor = UIColor.Black;

            MessagingCenter.Subscribe<CardPage>(this, "Orientation.Unspecified", sender =>
                                                UIDevice.CurrentDevice.SetValueForKey(NSNumber.FromNInt((int)UIDeviceOrientation.Portrait),
                                                                                      new NSString("orientation")));

            MessagingCenter.Subscribe<CardPage>(this, "Orientation.ForceLandScape", sender =>
                                                UIDevice.CurrentDevice.SetValueForKey(NSNumber.FromNInt((int)UIDeviceOrientation.LandscapeLeft),
                                                                                      new NSString("orientation")));

            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
