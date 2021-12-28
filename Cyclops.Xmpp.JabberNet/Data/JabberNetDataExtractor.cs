using System.Xml;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.JabberNet.Data.Rooms;
using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;
using jabber.protocol.iq;
using jabber.protocol.x;

namespace Cyclops.Xmpp.JabberNet.Data;

public class JabberNetDataExtractor : IXmppDataExtractor
{
    public CaptchaRequest? GetCaptchaRequest(IMessage message)
    {
        var captchaElement = message.GetNodeByName<XmlNode>("captcha");
        if (captchaElement != null && captchaElement["x"] != null)
        {
            var challenge = captchaElement["x"]?
                .OfType<Field>()
                .FirstOrDefault(i => string.Equals(i.Var, "challenge"))?.Val ?? "";

            var element = message.GetNodeByName<XmlElement>("data");
            if (element == null || element.ChildNodes.IsNullOrEmpty()) return null;

            var captchaInBase64 = element.FirstChild.Value;
            return new CaptchaRequest(challenge, captchaInBase64);
        }

        return null;
    }

    public IExtendedUserData? GetExtendedUserData(IPresence presence) => (presence["x"] as UserX)?.Wrap();
    public DateTime? GetDelayStamp(IMessage message)
    {
        var delay = message.Nodes.OfType<Delay>().SingleOrDefault();
        var stamp = delay?.Stamp;
        return stamp == DateTime.MinValue ? null : stamp;
    }

    public IAdminItem? GetAdminItem(IExtendedUserData nickChange) =>
        nickChange.Nodes.OfType<AdminItem>().SingleOrDefault()?.Map();
}
