using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Data;

public interface IXmppDataExtractor
{
    CaptchaRequest? GetCaptchaRequest(IMessage message);
    ExtendedUserData? GetExtendedUserData(IPresence presence);
}
