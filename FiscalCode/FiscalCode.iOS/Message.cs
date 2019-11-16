using FiscalCode.Utilities;

using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(FiscalCode.iOS.Message))]
namespace FiscalCode.iOS
{
    internal class Message : IMessage
    {
        const double longDelay = 3.5;
        const double shortDelay = 1.5;

        NSTimer alertDelay;

        public void LongAlert(string message) => ShowAlert(message, longDelay);
        public void ShortAlert(string message) => ShowAlert(message, shortDelay);

        void ShowAlert(string message, double seconds)
        {
            var alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            alertDelay = NSTimer.CreateScheduledTimer(seconds, alertDelay => DismissMessage(alert, alertDelay));
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        void DismissMessage(UIAlertController alert, NSTimer alertDelay)
        {
            if (alert != null)
                alert.DismissViewController(true, null);

            if (alertDelay != null)
                alertDelay.Dispose();

            if (this.alertDelay != null)
                alertDelay.Dispose();
        }
    }
}