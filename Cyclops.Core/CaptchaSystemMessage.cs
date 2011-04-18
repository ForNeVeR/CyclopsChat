namespace Cyclops.Core
{
    public class CaptchaSystemMessage : SystemConferenceMessage
    {
        public CaptchaSystemMessage()
        {
            IsErrorMessage = true;
        }

        public string Url { get; set; }
    }
}