using Android.App;
using Android.Widget;

using FiscalCode.Utilities;

[assembly: Xamarin.Forms.Dependency(typeof(FiscalCode.Droid.MessageAndroid))]
namespace FiscalCode.Droid
{
    public class MessageAndroid : IMessage
    {
        public void LongAlert(string message) => Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        public void ShortAlert(string message) => Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
    }
}