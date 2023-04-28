using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using HandOver.Interfaces;
using Xamarin.Forms;
using XFUniqueIdentifier.Droid;
using static Android.Provider.Settings;

[assembly: Dependency(typeof(AndroidDevice))]
namespace XFUniqueIdentifier.Droid
{
    public class AndroidDevice : IDevice
    {
        public string GetIdentifier()
        {
            return Secure.GetString(Forms.Context.ContentResolver, Secure.AndroidId);
        }
    }
}