using Cyclops.Xmpp.Data;

namespace Cyclops.Xmpp.JabberNet.Data;

internal static class PresenceTypeEx
{
    public static jabber.protocol.client.PresenceType Map(this PresenceType presenceType) => presenceType switch
    {
        PresenceType.Available => jabber.protocol.client.PresenceType.available,
        _ => throw new ArgumentException($"Unsupported presence type: {presenceType}.", nameof(presenceType))
    };
}
