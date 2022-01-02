using System.Globalization;
using System.Xml;
using System.Xml.Linq;
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
                var query = Original.Element(XNamespace.Get(Namespaces.Time) + Elements.Query);
                if (query == null)
                    throw new NotSupportedException("Could not able to find query element inside of iq");

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

    private static XElement GetOrCreateChildElement(this XElement parent, XName name)
    {
        var child = parent.Element(name);
        if (child == null)
        {
            child = new XElement(name);
            parent.Add(child);
        }

        return child;
    }
}
