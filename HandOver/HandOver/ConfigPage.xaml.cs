using HandOver.Classes;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HandOver.Singleton;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;

namespace HandOver
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigPage : ContentPage
    {
        public Config Item { get; set; }

        public ConfigPage()
        {
            Device.BeginInvokeOnMainThread(() => {
                InitializeComponent();
                Item = App.Config;
                BindingContext = this;

                if (Item.status == "err")
                {
                    Error.IsVisible = true;
                    ErrorMessage.Text = App.Config.reason;
                }
            });

        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            Save();
        }

        private async void Connect_Clicked(object sender, EventArgs e)
        {
            try
            {
                Error.IsVisible = false;
                ErrorMessage.Text = "";
                Save();

                using (HttpClient httpClient = new HttpClient())
                {
                    string json = await httpClient.GetStringAsync(start_path.Text + sn.Text);
                    RemoteJson downloaded = JsonConvert.DeserializeObject<RemoteJson>(json);
                    if (downloaded.status.ToLower() == "ok")
                    {
                        App.Config.path = path.Text = downloaded.path;
                        App.Config.start_path = start_path.Text = (downloaded.start_path.Length > 0) ? downloaded.start_path : App.Config.start_path;
                        App.Config.offline_pass = offline_pass.Text = (downloaded.offline_pass.Length > 0) ? downloaded.offline_pass : App.Config.offline_pass;
                        App.Config.status = Item.status = downloaded.status;
                        App.Config.reason = Item.reason = ErrorMessage.Text = downloaded.reason;
                        _ = Functions.SaveSettings();
                        App.ServerAccessTimer.Enabled = true;
                    }
                    else
                    {
                        Error.IsVisible = true;
                        App.Config.status = downloaded.status;
                        App.Config.reason = ErrorMessage.Text = downloaded.reason;
                    }
                }
            }
            catch
            {
                Error.IsVisible = true;
                App.Config.reason = ErrorMessage.Text = "Configuration file download failed";
            }
        }

        private void Save()
        {
            App.Config.start_path = start_path.Text = (!string.IsNullOrWhiteSpace(start_path.Text)) ? start_path.Text : DefaultValues.start_path;
            App.Config.offline_pass = offline_pass.Text = (!string.IsNullOrWhiteSpace(offline_pass.Text)) ? offline_pass.Text : DefaultValues.offline_pass;
            App.Config.path = path.Text = (!string.IsNullOrWhiteSpace(path.Text)) ? path.Text : DefaultValues.path;
            App.Config.refreshInterval = (!string.IsNullOrWhiteSpace(refreshInterval.Text)) ? int.Parse(refreshInterval.Text) : int.Parse(DefaultValues.refreshIntervalDefault);
            refreshInterval.Text = App.Config.refreshInterval.ToString();
            _ = Functions.SaveSettings();
        }

        protected override bool OnBackButtonPressed()
        {
            Environment.Exit(0);
            return true;
        }
    }
}
