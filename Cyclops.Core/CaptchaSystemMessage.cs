namespace Cyclops.Core;

public class CaptchaSystemMessage : SystemConferenceMessage
{
    public CaptchaSystemMessage()
    {
        IsErrorMessage = true;
    }

    public byte[] Bitmap { get; set; }
}
