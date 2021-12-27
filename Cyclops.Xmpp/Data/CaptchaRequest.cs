namespace Cyclops.Xmpp.Data;

public class CaptchaRequest
{
    public string CaptchaChallenge { get; }
    public string CaptchaImageBodyBase64 { get; }

    public CaptchaRequest(string captchaChallenge, string captchaImageBodyBase64)
    {
        CaptchaChallenge = captchaChallenge;
        CaptchaImageBodyBase64 = captchaImageBodyBase64;
    }
}
