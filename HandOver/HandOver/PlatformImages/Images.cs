using HandOver.Interfaces;

namespace HandOver.Images
{
    public class Images
    {
        public static string saveImage = new FileService().checkImageExist("save");
        public static string connectImage = new FileService().checkImageExist("connect");

        public static string SaveImage
        {
            get { return saveImage; }
            set
            {
                saveImage = value;
            }
        }

        public static string ConnectImage
        {
            get { return connectImage; }
            set
            {
                connectImage = value;
            }
        }

    }
}
