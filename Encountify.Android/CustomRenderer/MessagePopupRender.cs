using Android.App;
using Android.Widget;
using Encountify.CustomRenderer;
using Encountify.Droid.CustomRenderer;

[assembly: Xamarin.Forms.Dependency(typeof(MessagePopupRender))]

namespace Encountify.Droid.CustomRenderer
{
    class MessagePopupRender : MessagePopup
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}