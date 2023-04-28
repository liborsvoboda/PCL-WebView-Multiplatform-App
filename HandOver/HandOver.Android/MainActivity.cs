using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android;
using Android.Support.V4.Content;
using Android.Support.V4.App;

namespace HandOver.Droid
{
    [Activity(Label = "HandOver", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize,
        WindowSoftInputMode = Android.Views.SoftInput.StateAlwaysHidden, Name = "cz.jobs.vfc.scanner.Main")]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            //check hardware rights
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] {
                    Manifest.Permission.ChangeWifiState,
                    Manifest.Permission.AccessNetworkState,
                    Manifest.Permission.AccessWifiState,
                    Manifest.Permission.Internet,
                    Manifest.Permission.SetOrientation,
                    Manifest.Permission.ReadExternalStorage,
                    Manifest.Permission.WriteExternalStorage,
                }, (int)Permission.Granted);
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}