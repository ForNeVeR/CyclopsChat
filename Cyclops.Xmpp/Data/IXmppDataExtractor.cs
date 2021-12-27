using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Data;

public interface IXmppDataExtractor
{
    public CaptchaRequest? GetCaptchaRequest(IMessage message);
}
