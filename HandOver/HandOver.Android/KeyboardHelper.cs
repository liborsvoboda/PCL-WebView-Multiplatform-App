using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using HandOver.Interfaces;
using HandOver.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(KeyboardInteractions))]
namespace HandOver.Droid
{
    public class KeyboardInteractions : IKeyboardInteractions
    {
        [System.Obsolete]
        public void HideKeyboard()
        {

            var view = ((Activity)Forms.Context).CurrentFocus;
            if (view != null)
            {
                var imm = (InputMethodManager)Forms.Context.GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(view.WindowToken, 0);
            }
        }

    }
}