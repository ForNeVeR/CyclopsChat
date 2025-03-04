using System.Drawing;

namespace Cyclops.Core;

public class CaptchaSystemMessage : SystemConferenceMessage
{
    public CaptchaSystemMessage()
    {
        IsErrorMessage = true;
    }

    public Image Bitmap { get; set; }
}
