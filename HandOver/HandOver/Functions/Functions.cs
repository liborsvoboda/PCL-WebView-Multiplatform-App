using HandOver.Classes;
using HandOver.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HandOver
{

    class Functions
    {

        private static IDevice device = DependencyService.Get<IDevice>();
        public static string deviceIdentifier = (device != null) ? device.GetIdentifier() : Plugin.DeviceInfo.CrossDeviceInfo.Current.Id;

        public static bool LoadLocalSetting()
        {
            try
            {
                if (File.Exists(App.AppStatus.LocalConfigFile))
                {
                    if (!File.Exists(Path.Combine(App.AppStatus.BasePath, App.AppStatus.ConfigFile))) { Thread.Sleep(10000); }

                    try
                    {
                        using (StreamReader sr = new StreamReader(App.AppStatus.LocalConfigFile, fn_file_detect_encoding(App.AppStatus.LocalConfigFile)))
                        {
                            App.Config.sn = sr.ReadLine().Replace("\n", "").Replace("\r", "");
                            App.Config.start_path = sr.ReadLine() + "?sn=";
                            App.Config.status = "";
                            App.Config.reason = "";
                            App.Config.loading = true;
                            App.Config.notLoading = false;
                            sr.Close();
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        App.Config.status = "err";
                        App.Config.reason = "File " + App.AppStatus.LocalConfigFile + " cannot be Readed";
                        return false;
                    }
                }
                else {
                    App.Config.status = "err";
                    App.Config.reason = "File " + App.AppStatus.LocalConfigFile + " not Exist";
                    return false; 
                }
            } catch
            {
                App.Config.status = "err";
                App.Config.reason = "Please enable storage access";
                return false;
            }
        }

        public static bool LoadSettings()
        {
            if (File.Exists(Path.Combine(App.AppStatus.BasePath, App.AppStatus.ConfigFile)))
            {
                using (StreamReader sr = new StreamReader(Path.Combine(App.AppStatus.BasePath, App.AppStatus.ConfigFile),fn_file_detect_encoding(Path.Combine(App.AppStatus.BasePath, App.AppStatus.ConfigFile))))
                {
                    string fileContent = sr.ReadToEnd();
                    App.Config = JsonConvert.DeserializeObject<Config>(fileContent);
                    if (App.Config.status != "err") {
                        App.Config.status = "";
                        App.Config.reason = "";
                    }
                    App.Config.loading = true;
                    App.Config.notLoading = false;
                    sr.Close();
                }
                return true;
            }
            else {

                return false;
            }
        }

        public static bool SaveSettings()
        {
            try
            {
                using (StreamWriter sw = File.CreateText(Path.Combine(App.AppStatus.BasePath, App.AppStatus.ConfigFile)))
                {
                    sw.Write(JsonConvert.SerializeObject(App.Config));
                    sw.Flush();
                    sw.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string CheckPlatform()
        {
            string platform;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    platform = "iOS";
                    break;
                case Device.Android:
                    platform = "Android";
                    break;
                case Device.UWP:
                    platform = "UWP";
                    break;
                default:
                    platform = "";
                    break;
            }
            return platform;
        }

        public async static Task<bool> CheckOnline()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var json = await httpClient.GetStringAsync(App.Config.start_path + App.Config.sn);
                    RemoteJson downloaded = JsonConvert.DeserializeObject<RemoteJson>(json);
                    if (downloaded.status.ToLower() == "ok")
                    {
                        App.Config.path = downloaded.path;
                        App.Config.start_path = (downloaded.start_path.Length > 0) ? downloaded.start_path : App.Config.start_path;
                        App.Config.offline_pass = (downloaded.offline_pass.Length > 0) ? downloaded.offline_pass : App.Config.offline_pass;
                        App.Config.status = downloaded.status;
                        App.Config.reason = downloaded.reason;
                        SaveSettings();
                        return true;
                    }
                    else
                    {
                        App.Config.status = downloaded.status;
                        App.Config.reason = downloaded.reason;
                        return false;
                    }
                }
            }
            catch
            {
                App.Config.status = "err";
                App.Config.reason = "Configuration file download failed";
                return false;
            }
        }

        public static Encoding fn_file_detect_encoding(string FileName)
        {
            string enc = "";
            if (File.Exists(FileName))
            {
                FileStream filein = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                if ((filein.CanSeek))
                {
                    byte[] bom = new byte[5];
                    filein.Read(bom, 0, 4);
                    // EF BB BF       = utf-8
                    // FF FE          = ucs-2le, ucs-4le, and ucs-16le
                    // FE FF          = utf-16 and ucs-2
                    // 00 00 FE FF    = ucs-4
                    if ((((bom[0] == 0xEF) && (bom[1] == 0xBB) && (bom[2] == 0xBF)) || ((bom[0] == 0xFF) && (bom[1] == 0xFE)) || ((bom[0] == 0xFE) && (bom[1] == 0xFF)) || ((bom[0] == 0x0) && (bom[1] == 0x0) && (bom[2] == 0xFE) && (bom[3] == 0xFF))))
                        enc = "Unicode";
                    else
                        enc = "ASCII";
                    // Position the file cursor back to the start of the file
                    filein.Seek(0, SeekOrigin.Begin);
                }
                filein.Close();
            }
            if (enc == "Unicode")
                return System.Text.Encoding.UTF8;
            else
                return System.Text.Encoding.Default;
        }
    }
}