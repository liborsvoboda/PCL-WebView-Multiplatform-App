using System;
using System.ComponentModel;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandOver.Singleton
{
    public class AppStatus
    {
        public bool Loading { get; set; } = true;
        public bool StatusOnline { get; set; } = false;
        public bool BackButtonPressed { get; set; } = false;
        public bool UserLoggedIn { get; set; } = false;
        public string BasePath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);//ApplicationData LocalApplicationData a personal
        public string ExternalPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalizedResources);
        public string ConfigFile { get; set; } = "Config.json";
        public string LocalConfigFile { get; set; } = "/sdcard/Download/handover_info.txt";//"/enterprise/usr/handover_info.txt";
        public Page TargetPage { get; set; } = new OfflinePage { Title = "Offline Page", Item = App.Config };
        public string Platform { get; set; } = Functions.CheckPlatform();
        public int RefreshInterval { get; set; } = int.Parse(DefaultValues.refreshIntervalDefault);
    }

    public class DefaultValues
    {
        //default values 
        public const string start_path = "http://dev.boteg.cz/HandOver/CI1/check/?sn=";
        public const string refreshIntervalDefault = "3";
        public const string offline_pass = "TajneHeslo";
        public const string path = "";
        public static string sn = Functions.deviceIdentifier;//Plugin.DeviceInfo.CrossDeviceInfo.Current.Id;
    }
}
