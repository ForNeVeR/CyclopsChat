using System.Xml.Linq;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Data;

public class SharpXmppDataExtractor : IXmppDataExtractor
{
    public IExtendedUserData? GetExtendedUserData(IPresence presence)
    {
        var xmppPresence = presence.Unwrap();
        var x = xmppPresence.Element(XNamespace.Get(Namespaces.Muc) + Elements.X);
        return x.WrapUserData();
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
