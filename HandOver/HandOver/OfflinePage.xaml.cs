using HandOver.Classes;
using HandOver.Interfaces;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandOver
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfflinePage : ContentPage
    {
        public Config Item { get; set; }

        public OfflinePage()
        {
            Device.BeginInvokeOnMainThread(() => {
                InitializeComponent();
                Item = App.Config;
                BindingContext = this;

                if (!string.IsNullOrWhiteSpace(Item.reason)) if (Item.reason.Length > 0) exit_code.IsVisible = true;

                try { exit_code.Focus(); }
                catch { }
            });
        }

        private void close_application(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.ToLower() == Item.offline_pass.ToLower()) { Environment.Exit(0); }
        }

        private void Label_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Item.reason))
                    if (Item.reason.Length > 0) { Item.loading = false; Item.notLoading = true; }
            }
            catch
            {
                Item.loading = true; Item.notLoading = false;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            exit_code.Unfocus();
            Device.BeginInvokeOnMainThread(() =>
            {
                App.AppStatus.TargetPage = new WebViewPage { Title = "Web Page" };
                //base.OnBackButtonPressed();
                App.AppStatus.BackButtonPressed = false;
            });
            //App.ServerAccessTimer.Enabled = false;
            return true;
        }
    }
}
