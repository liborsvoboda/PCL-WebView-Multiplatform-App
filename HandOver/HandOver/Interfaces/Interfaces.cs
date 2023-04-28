using System.IO;
using Xamarin.Forms;
using static Android.Bluetooth.BluetoothClass;

namespace HandOver.Interfaces
{
    public interface IKeyboardInteractions
    {
        void HideKeyboard();
    }

    public interface IDevice
    {
        string GetIdentifier();
    }

    public interface IFileService
    {
        bool FileExists(string filePath);
    }

    public class FileService : IFileService
    {
        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string checkImageExist(string imageName)
        {
            if (File.Exists(imageName + ".xml"))
            {
                return imageName + ".xml";
            }
            else if (File.Exists(imageName + ".png"))
            {
                return imageName + ".png";
            }
            else if (File.Exists(imageName + ".jpg"))
            {
                return imageName + ".jpg";
            }
            else if (File.Exists(imageName + ".pdf"))
            {
                return imageName + ".pdf";
            }
            else { return imageName; }
        }
    }
}
