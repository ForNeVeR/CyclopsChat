using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Data;

public class SharpXmppDataExtractor : IXmppDataExtractor
{
    public IExtendedUserData? GetExtendedUserData(IPresence presence)
    {
        throw new NotImplementedException();
    }

    public PhotoData? GetPhotoData(IPresence presence)
    {
        throw new NotImplementedException();
    }

    public DateTime? GetDelayStamp(IMessage message)
    {
        throw new NotImplementedException();
    }

    public CaptchaRequest? GetCaptchaRequest(IMessage message)
    {
        throw new NotImplementedException();
    }

    public IAdminItem? GetAdminItem(IExtendedUserData nickChange)
    {
        throw new NotImplementedException();
    }
}
