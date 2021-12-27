using System.Linq;
using System.Windows.Media.Imaging;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.x;

namespace Cyclops.Core.Resource.Helpers;

internal static class CaptchaHelper
{
    internal static bool ExtractImage(Message msg, ref BitmapImage image, ref string captchaChallenge)
    {
        var captchaElement = msg.OfType<Element>().GetNodeByName<Element>("captcha");
        if (captchaElement != null && captchaElement["x"] != null)
        {
            captchaChallenge = captchaElement["x"].OfType<Field>().FirstOrDefault(i => string.Equals(i.Var, "challenge")).Val;

            var element = msg.OfType<Element>().GetNodeByName<Element>("data");
            if (element != null && !element.ChildNodes.IsNullOrEmpty())
            {
                try
                {
                    var captchaInBase64 = element.FirstChild.Value;
                    image = ImageUtils.Base64ToBitmapImage(captchaInBase64);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
        return false;
    }
}
