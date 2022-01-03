using System;
using System.Xml.Linq;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Data;
using Cyclops.Xmpp.SharpXmpp.Protocol;
using SharpXMPP.XMPP.Client.Elements;
using Xunit;

namespace Cyclops.Tests;

public class SharpXmppDataExtractorTests
{
    private readonly IXmppDataExtractor dataExtractor = new SharpXmppDataExtractor();

    [Fact]
    public void DateTimeParsesCorrectly()
    {
        const string stringData = "2002-09-10T23:41:07Z";
        var expectedDateTime = new DateTime(2002, 9, 10, 23, 41, 07, DateTimeKind.Utc);

        var message = new XMPPMessage();
        message.GetOrCreateChildElement(XNamespace.Get(Namespaces.Delay) + Elements.Delay)
            .GetOrCreateAttribute(Attributes.Stamp).Value = stringData;

        Assert.Equal(expectedDateTime, dataExtractor.GetDelayStamp(message.Wrap()));
    }
}
