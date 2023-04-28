using HandOver.Interfaces;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(Windows.Devices.Spi.SpiDevice))]
namespace XFUniqueIdentifier.WinPhone
{
    public class WinPhoneDevice : IDevice
    {
        public string GetIdentifier()
        {
            return Plugin.DeviceInfo.CrossDeviceInfo.Current.Id;
        }
    }
}