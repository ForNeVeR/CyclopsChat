using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Data;

public interface IXmppDataExtractor
{
    IExtendedUserData? GetExtendedUserData(IPresence presence);

    DateTime? GetDelayStamp(IMessage message);
    CaptchaRequest? GetCaptchaRequest(IMessage message);
    IAdminItem? GetAdminItem(IExtendedUserData nickChange);
}
