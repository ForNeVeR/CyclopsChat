using System.Xml;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.JabberNet.Helpers;
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

    public ExtendedUserData? GetExtendedUserData(IPresence presence)
    {
        var userX = presence["x"] as UserX;
        if (userX == null) return null;

        var roomItem = userX.RoomItem;
        return new ExtendedUserData(
            roomItem?.Actor?.JID,
            userX.Status.Select(x => x.Map()).ToList(),
            roomItem?.Role.Map(),
            roomItem?.Affiliation.Map());
    }
}
