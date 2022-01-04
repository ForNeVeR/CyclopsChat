using System.Globalization;
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
        var x = xmppPresence.Element(XNamespace.Get(Namespaces.MucUser) + Elements.X);
        return x?.WrapAsUserData();
    }

    public PhotoData? GetPhotoData(IPresence presence)
    {
        var xmlPresence = presence.Unwrap();
        var hash = xmlPresence.Element(XNamespace.Get(Namespaces.VCardTempXUpdate) + Elements.X)?
            .Element(XNamespace.Get(Namespaces.VCardTempXUpdate) + Elements.Photo)?
            .Value;
        return hash == null ? null : new PhotoData(hash);
    }

    public DateTime? GetDelayStamp(IMessage message)
    {
        var xmlMessage = message.Unwrap();
        var delay = xmlMessage.Element(XNamespace.Get(Namespaces.Delay) + Elements.Delay);
        var stamp = delay?.Attribute(Attributes.Stamp)?.Value;
        if (string.IsNullOrEmpty(stamp)) return null;
        return DateTime.TryParseExact(
            stamp,
            "yyyy-MM-ddTHH:mm:ssZ",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dateTime)
            ? dateTime
            : null;
    }

    public CaptchaRequest? GetCaptchaRequest(IMessage message)
    {
        var xmlMessage = message.Unwrap();
        var captcha = xmlMessage.Element(XNamespace.Get(Namespaces.Captcha) + Elements.Captcha);
        if (captcha == null) return null;

        var challenge = captcha.Element(XNamespace.Get(Namespaces.Data) + Elements.X)?
            .Elements(XNamespace.Get(Namespaces.Data) + Elements.Field)?
            .FirstOrDefault(f => f.Attribute(Attributes.Var)?.Value == "challenge");
        if (challenge == null) throw new Exception("Couldn't find a challenge field inside of a CAPTCHA form.");

        var data = xmlMessage.Element(XNamespace.Get(Namespaces.BitsOfBinary) + Elements.Data);
        if (data == null) throw new Exception("Couldn't find a data element inside of a CAPTCHA request message.");

        return new CaptchaRequest(challenge.Value, data.Value);
    }

    private class AdminItem : IAdminItem
    {
        public AdminItem(XElement item)
        {
            Nick = item.Attribute(Attributes.Nick)?.Value;
        }

        public string? Nick { get; }
    }

    public IAdminItem? GetAdminItem(IExtendedUserData nickChange)
    {
        var x = nickChange.Unwrap();
        var item = x.Element(XNamespace.Get(Namespaces.MucAdmin) + Elements.Item);
        return item == null ? null : new AdminItem(item);
    }
}
