using HandOver.Classes;
using HandOver.Interfaces;
using HandOver.Singleton;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HandOver
{
    public partial class App : Application
    {
        public static Config Config = new Config();
        public static AppStatus AppStatus = new AppStatus();
        public static Timer ServerAccessTimer = new Timer() { Enabled = false, Interval = AppStatus.RefreshInterval * 1000 };
        private static string lastReason = "";
        private static bool hideKeyboard = true;

        public App()
        {
            if (AppStatus.Platform == "UWP") {
                InitializeComponent();
                MainPage = AppStatus.TargetPage;
            }
            else
            {
                Device.BeginInvokeOnMainThread(() => {
                    InitializeComponent();
                    MainPage = AppStatus.TargetPage;
                });
            }
        }

        protected override void OnStart()
        {
            bool localSettingLoaded = false;
            try
            {
                 localSettingLoaded = Functions.LoadLocalSetting();
            }
            catch { }

            if (Functions.CheckPlatform() == "Android" && localSettingLoaded) {

                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string json = httpClient.GetStringAsync(Config.start_path + Config.sn).GetAwaiter().GetResult();
                        RemoteJson downloaded = JsonConvert.DeserializeObject<RemoteJson>(json);
                        if (downloaded.status.ToLower() == "ok")
                        {
                            Config.path = downloaded.path;
                            Config.start_path = (downloaded.start_path.Length > 0) ? downloaded.start_path : Config.start_path;
                            Config.offline_pass = (downloaded.offline_pass.Length > 0) ? downloaded.offline_pass : Config.offline_pass;
                            Config.status = downloaded.status;
                            _ = Functions.SaveSettings();
                            ServerAccessTimer.Enabled = true;
                        }
                        else
                        {
                            Config.status = downloaded.status;
                            Config.reason = downloaded.reason;
                        }
                    }
                } catch
                {
                    Config.status = "err";
                    Config.reason = "Configuration file download failed";
                }

                ServerAccessTimer.Interval = 1;
                ServerAccessTimer.Enabled = true;
            }
            else
            {
                if (Functions.LoadSettings())
                {
                    ServerAccessTimer.Interval = 1;
                    ServerAccessTimer.Enabled = true;
                }
                else
                {
                    AppStatus.TargetPage = new ConfigPage { Title = "Config Page" };
                }
            }
            if (AppStatus.Platform == "UWP")
            {
                Device.BeginInvokeOnMainThread(() => {
                    if (MainPage.Title != AppStatus.TargetPage.Title) MainPage = AppStatus.TargetPage;
                    MainPage.Focus();
                });
                ServerAccessTimer.Elapsed += new ElapsedEventHandler(OnserverAccessElapsedTime);

            }
            else
            {
                Device.BeginInvokeOnMainThread(() => {
                    if (MainPage.Title != AppStatus.TargetPage.Title) MainPage = AppStatus.TargetPage;
                    MainPage.Focus();
                    ServerAccessTimer.Elapsed += new ElapsedEventHandler(OnserverAccessElapsedTime);
                });
            }



        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void OnserverAccessElapsedTime(object source, ElapsedEventArgs e)
        {
            //Device.BeginInvokeOnMainThread(() => {
            ServerAccessTimer.Interval = AppStatus.RefreshInterval * 1000;
            if (!AppStatus.BackButtonPressed)
            {
                if (Functions.CheckOnline().Result)
                {
                    if (MainPage.Title != "Web Page") AppStatus.TargetPage = new WebViewPage { Title = "Web Page" };
                    AppStatus.StatusOnline = true;
                }
                else
                {
                    AppStatus.TargetPage = new OfflinePage { Title = "Offline Page", Item = Config };
                    AppStatus.StatusOnline = false;
                }

                Config.loading = false;
                Config.notLoading = true;
            }

            Device.BeginInvokeOnMainThread(() => {
                if (MainPage.Title != AppStatus.TargetPage.Title)
                {
                    MainPage = AppStatus.TargetPage;

                }
                else if (MainPage.Title == AppStatus.TargetPage.Title && MainPage.Title == "Offline Page" && lastReason != Config.reason)
                {
                    MainPage = AppStatus.TargetPage;
                }

                try
                {
                    if (((UrlWebViewSource)((WebView)((ContentPage)MainPage).Content).Source).Url.ToLower().Contains("closeapp"))
                    {
                        Environment.Exit(0);
                    }
                }
                catch { }
            });

            MainPage.Focus();

            try { if (MainPage.Title == "Offline Page") { ((Entry)MainPage.FindByName("exit_code")).IsVisible =true; ((Entry)MainPage.FindByName("exit_code")).Focus(); lastReason = Config.reason;
                    //IKeyboardInteractions keyboardInteractions = DependencyService.Get<IKeyboardInteractions>();
                    //keyboardInteractions.HideKeyboard();
                    hideKeyboard = true;
                }
            }
            catch { }

            try { if (MainPage.Title == "Web Page" && hideKeyboard) { MainPage.FindByName<WebView>("content").Focus();
                    IKeyboardInteractions keyboardInteractions = DependencyService.Get<IKeyboardInteractions>();
                    keyboardInteractions.HideKeyboard();
                    hideKeyboard = false;
                } }
            catch { }

        }

    }
}
