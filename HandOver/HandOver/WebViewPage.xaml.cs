using HandOver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandOver
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebViewPage : ContentPage
    {
        private int counter = 0; 
        public WebViewPage()
        {
            Device.BeginInvokeOnMainThread(() => {
                InitializeComponent();
                content.Source = App.Config.path;

                try{ content.Focus(); }
                catch { }
            });
        }

        protected override bool OnBackButtonPressed()
        {

            Device.BeginInvokeOnMainThread(() =>
            {

                App.AppStatus.TargetPage = new OfflinePage { Title = "Offline Page", Item = App.Config };
                //base.OnBackButtonPressed();
                App.AppStatus.BackButtonPressed = true;
            });
            //App.ServerAccessTimer.Enabled = false;
            return true;
        }
    }

}