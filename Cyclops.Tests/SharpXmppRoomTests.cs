using System;
using System.Threading;
using Cyclops.Core;
using Cyclops.TestFramework;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Client;
using Cyclops.Xmpp.SharpXmpp.Data.Rooms;
using JetBrains.Lifetimes;
using Xunit;
using Xunit.Abstractions;

namespace Cyclops.Tests;

public class SharpXmppRoomTests
{
    private readonly ILogger _logger;
    public SharpXmppRoomTests(ITestOutputHelper helper)
    {
        _logger = new XUnitLoggerAdapter(helper);
    }

    [Fact]
    public void NicknameChangeShouldProduceRejoinEvent()
    {
        const string roomJid = "room@conference.example.com";
        using var client = new MockXmppClient(_logger);
        var room = new SharpXmppRoom(
            _logger,
            client,
            new SharpXmppConferenceManager(_logger, client),
            Jid.Parse(roomJid));

        using var ld = new LifetimeDefinition();
        var lifetime = ld.Lifetime;

        var joined = 0;
        EventHandler onJoin = (_, _) => Interlocked.Increment(ref joined);
        EventHandler<IPresence> onLeave = (_, _) => Interlocked.Decrement(ref joined);
        lifetime.Bracket(
            () => room.Joined += onJoin,
            () => room.Joined -= onJoin);
        lifetime.Bracket(
            () => room.Left += onLeave,
            () => room.Left -= onLeave);

        Assert.Equal(0, joined);
        client.FirePresence(MockPresences.Join(roomJid, "nick1"));
        Assert.Equal(1, joined);

        client.FirePresence(MockPresences.Leave(roomJid, "nick1"));
        Assert.Equal(0, joined);

        client.FirePresence(MockPresences.Join(roomJid, "nick2"));
        Assert.Equal(1, joined);
    }
}
