using System;
using Cyclops.Xmpp.Protocol;
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
        var cyclopsJid = Jid.Parse(jid);

        Assert.Equal(jid, cyclopsJid.ToString());
    }

    [Theory]
    [InlineData("/")]
    [InlineData("@")]
    [InlineData("/baz")]
    [InlineData("foo@/baz")]
    [InlineData("foo@bar/")]
    public void CheckParsesWithException(string jid)
    {
        Assert.Throws<ArgumentException>(() => Jid.Parse(jid));
    }
}
