using FiscalCode.Utilities;

using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(FiscalCode.iOS.Message))]
namespace FiscalCode.iOS
{
    public class Message : IMessage
    {
        const double longDelay = 3.5;
        const double shortDelay = 1.0;

        public void LongAlert(string message) => ShowAlert(message, longDelay);
        public void ShortAlert(string message) => ShowAlert(message, shortDelay);

        void ShowAlert(string message, double seconds)
        {
            var alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);

            using (var alertDelay = NSTimer.CreateScheduledTimer(seconds, _ => DismissMessage(alert, _)))
            {
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
            }
        }

        static void DismissMessage(UIAlertController alert, NSTimer alertDelay)
        {
            if (alert != null)
                alert.DismissViewController(true, null);

            if (alertDelay != null)
                alertDelay.Dispose();
        }
    }
}
