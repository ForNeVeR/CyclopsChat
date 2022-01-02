using System;
using System.Xml;
using System.Xml.Linq;
using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Protocol;
using jabber.protocol.client;
using jabber.protocol.iq;
using SharpXMPP.XMPP.Client.Elements;
using Xunit;

namespace Cyclops.Tests;

public class TimeIqTests
{
    [Fact]
    public void TimeIqEquallyPresented()
    {
        var dateTime = new DateTime(2020, 1, 1);
        var timeZone = TimeZoneInfo.Utc;
        var jabberNetIq = CreateJabberNetTimeIq(dateTime, timeZone);
        var sharpXmppIq = CreateSharpXmppTimeIq(dateTime, timeZone);

        var jabberNetQuery = XDocument.Parse(jabberNetIq.Query.ToString()!).Root;
        var sharpXmppQuery = sharpXmppIq.Element(XNamespace.Get(Namespaces.Time) + Elements.Query);

        Assert.Equal(jabberNetQuery!.ToString(), sharpXmppQuery!.ToString());
    }

    private static IQ CreateJabberNetTimeIq(DateTime dateTime, TimeZoneInfo timeZone)
    {
        var iq = new TimeIQ(new XmlDocument());
        var wrapped = iq.WrapTime();
        wrapped.Time = (dateTime, timeZone);
        return Xmpp.JabberNet.Protocol.IqEx.Unwrap(wrapped);
    }

    private static XMPPIq CreateSharpXmppTimeIq(DateTime dateTime, TimeZoneInfo timeZone)
    {
        var iq = new XMPPIq(XMPPIq.IqTypes.get);
        var wrapped = iq.WrapTime();
        wrapped.Time = (dateTime, timeZone);
        return wrapped.Unwrap();
    }
}
