using System.Windows.Media.Imaging;

namespace Cyclops.Core
{
    public class CaptchaSystemMessage : SystemConferenceMessage
    {
        public CaptchaSystemMessage()
        {
            IsErrorMessage = true;
        }

        public BitmapImage Bitmap { get; set; }
    }
}