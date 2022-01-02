using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.Protocol.IqQueries;
using SharpXMPP.XMPP.Client.Elements;

namespace Cyclops.Xmpp.SharpXmpp.Protocol;

internal static class IqEx
{
    private abstract class Iq : IIq
    {
        protected internal readonly XMPPIq Original;
        protected Iq(XMPPIq original)
        {
            Original = original;
        }

        public Jid? From => Original.From.Map();
        public Jid? To => Original.To.Map();
        public XmlElement? Error => throw new NotImplementedException();
    }

    private class TimeIq : Iq, ITimeIq
    {
        public TimeIq(XMPPIq original) : base(original)
        {
        }

        public ITimeIq CreateResponse() => Original.Reply().WrapTime();
        public (DateTime, TimeZoneInfo) Time
        {
            set
            {
                var query = Original.GetOrCreateChildElement(XNamespace.Get(Namespaces.Time) + Elements.Query);
                var utc = query.GetOrCreateChildElement(XNamespace.Get(Namespaces.Time) + Elements.Utc);
                var tz = query.GetOrCreateChildElement(XNamespace.Get(Namespaces.Time) + Elements.Tz);
                var display = query.GetOrCreateChildElement(XNamespace.Get(Namespaces.Time) + Elements.Display);

                var (dateTime, timeZone) = value;

                utc.Value = dateTime.ToUniversalTime().ToString("yyyyMMddTHH:mm:ss", CultureInfo.InvariantCulture);
                tz.Value = timeZone.IsDaylightSavingTime(dateTime) ? timeZone.DaylightName : timeZone.StandardName;
                display.Value = dateTime.ToString("s");
            }
        }
    }

    public static ITimeIq WrapTime(this XMPPIq iq) => new TimeIq(iq);
    public static XMPPIq Unwrap(this ITimeIq iq) => ((Iq)iq).Original;


    private class LastIq : Iq, ILastIq
    {
        public LastIq(XMPPIq iq) : base(iq)
        {
        }

        public ILastIq CreateResponse() => Original.Reply().WrapLast();

        public int? Seconds
        {
            set
            {
                var query = Original.GetOrCreateChildElement(XNamespace.Get(Namespaces.Last) + Elements.Query);
                var attribute = query.GetOrCreateAttribute(Attributes.Seconds);
                if (value == null) attribute.Remove();
                else attribute.Value = value.Value.ToString(CultureInfo.InvariantCulture);
            }
        }
    }

    public static ILastIq WrapLast(this XMPPIq iq) => new LastIq(iq);

    private class VersionIq : Iq, IVersionIq
    {
        public VersionIq(XMPPIq iq) : base(iq)
        {
        }

        public IVersionIq CreateResponse() => Original.Reply().WrapVersion();

        public ClientInfo ClientInfo
        {
            set
            {
                var query = Original.GetOrCreateChildElement(XNamespace.Get(Namespaces.Version) + Elements.Query);
                var name = query.GetOrCreateChildElement(XNamespace.Get(Namespaces.Version) + Elements.Name);
                var version = query.GetOrCreateChildElement(XNamespace.Get(Namespaces.Version) + Elements.Version);
                var os = query.GetOrCreateChildElement(XNamespace.Get(Namespaces.Version) + Elements.Os);

                name.Value = value.Client;
                version.Value = value.Version;
                os.Value = value.Os;
            }
        }
    }

    public static IVersionIq WrapVersion(this XMPPIq iq) => new VersionIq(iq);

    private static XElement GetOrCreateChildElement(this XContainer parent, XName name)
    {
        var child = parent.Element(name);
        if (child != null) return child;

        child = new XElement(name);
        parent.Add(child);

        return child;
    }

    private static XAttribute GetOrCreateAttribute(this XElement element, XName name)
    {
        var attribute = element.Attribute(name);
        if (attribute != null) return attribute;

        attribute = new XAttribute(name, "");
        element.Add(attribute);

        return attribute;
    }
}
