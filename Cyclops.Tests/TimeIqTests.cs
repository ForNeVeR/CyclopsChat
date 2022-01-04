using System;
using System.Xml.Linq;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Protocol;
using SharpXMPP.XMPP.Client.Elements;
using Xunit;

namespace Cyclops.Tests;

public class TimeIqTests
{
    [Fact]
    public void TimeIqProperlyPresented()
    {
        var dateTime = new DateTime(2020, 1, 1);
        var timeZone = TimeZoneInfo.Utc;
        var sharpXmppIq = CreateSharpXmppTimeIq(dateTime, timeZone);

        var sharpXmppQuery = sharpXmppIq.Element(XNamespace.Get(Namespaces.Time) + Elements.Query)!;
        var utc = sharpXmppQuery.Element(sharpXmppQuery.Name.Namespace + "utc")!.Value;
        var display = sharpXmppQuery.Element(sharpXmppQuery.Name.Namespace + "display")!.Value;
        var tz = sharpXmppQuery.Element(sharpXmppQuery.Name.Namespace + "tz")!.Value;

        Assert.Equal("20191231T17:00:00", utc);
        Assert.Equal("2020-01-01T00:00:00", display);
        Assert.Equal(timeZone.StandardName, tz);
    }

    private static XMPPIq CreateSharpXmppTimeIq(DateTime dateTime, TimeZoneInfo timeZone)
    {
        var iq = new XMPPIq(XMPPIq.IqTypes.get);
        var wrapped = iq.WrapTime();
        wrapped.Time = (dateTime, timeZone);
        return wrapped.Unwrap();
    }
}
