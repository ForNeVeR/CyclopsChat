using System.Globalization;
using System.Xml.Linq;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Protocol;
using SharpXMPP.XMPP.Client.Elements;

namespace Cyclops.TestFramework;

public static class MockPresences
{
    public static XMPPPresence Join(string roomJid, string nickname)
    {
        return MakeSelfPresence(roomJid, nickname);
    }

    public static XMPPPresence Leave(string roomJid, string nickname)
    {
        var presence = MakeSelfPresence(roomJid, nickname);
        presence.Add(
            new XAttribute(Attributes.Type, PresenceTypes.Unavailable));
        return presence;
    }

    private static XMPPPresence MakeSelfPresence(string roomJid, string nickname)
    {
        var presence = new XMPPPresence();
        presence.Add(
            new XAttribute(Attributes.From, $"{roomJid}/${nickname}"),
            new XElement(XNamespace.Get(Namespaces.MucUser) + Elements.X,
                new XElement(XNamespace.Get(Namespaces.MucUser) + Elements.Status,
                    new XAttribute(Attributes.Code, ((int)MucUserStatus.SelfReferringPresence).ToString(CultureInfo.InvariantCulture)))));
        return presence;
    }
}
