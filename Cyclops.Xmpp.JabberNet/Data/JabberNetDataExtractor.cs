using System.Xml;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.JabberNet.Data.Rooms;
using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;
using jabber.protocol.iq;
using jabber.protocol.x;

namespace Cyclops.Xmpp.JabberNet.Data;

internal class JabberNetDataExtractor : IXmppDataExtractor
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

    public IExtendedUserData? GetExtendedUserData(IPresence presence) => (presence.Unwrap()["x"] as UserX)?.Wrap();

    public PhotoData? GetPhotoData(IPresence presence)
    {
        var photoTagParent = presence.Unwrap().Cast<XmlNode>()
            .FirstOrDefault(i => i.Name == "x" && i["photo"] != null);
        if (photoTagParent != null)
        {
            var sha1Hash = photoTagParent["photo"]?.InnerText;
            if (sha1Hash != null)
                return new PhotoData(sha1Hash);
        }

        return null;
    }

    public DateTime? GetDelayStamp(IMessage message)
    {
        var delay = message.Unwrap().Cast<XmlNode>().OfType<Delay>().SingleOrDefault();
        var stamp = delay?.Stamp;
        return stamp == DateTime.MinValue ? null : stamp;
    }

    public IAdminItem? GetAdminItem(IExtendedUserData nickChange) =>
        nickChange.Nodes.OfType<AdminItem>().SingleOrDefault()?.Map();
}
