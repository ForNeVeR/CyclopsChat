using System;
using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;
using jabber;
using Xunit;

namespace Cyclops.Tests;

public class JidTests
{
    [Theory]
    [InlineData("foo@bar/baz")]
    [InlineData("@bar/baz")]
    [InlineData("foo@bar")]
    [InlineData("foo")]
    [InlineData("@bar")]
    [InlineData("foo/baz")]
    [InlineData("foo/bar@baz")]
    public void CheckParsesCorrectly(string jid)
    {
        var jabberNetJid = new JID(jid);
        var cyclopsJid = Jid.Parse(jid);

        Assert.Equal(jabberNetJid.Map(), cyclopsJid);
        Assert.Equal(jabberNetJid.ToString(), cyclopsJid.ToString());
    }

    [Theory]
    [InlineData("/")]
    [InlineData("@")]
    [InlineData("/baz")]
    [InlineData("foo@/baz")]
    [InlineData("foo@bar/")]
    public void CheckParsesWithException(string jid)
    {
        Assert.Throws<JIDFormatException>(() => new JID(jid));
        Assert.Throws<ArgumentException>(() => Jid.Parse(jid));
    }
}
