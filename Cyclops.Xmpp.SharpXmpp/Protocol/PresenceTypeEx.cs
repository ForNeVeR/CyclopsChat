using Cyclops.Xmpp.Data;

namespace Cyclops.Xmpp.SharpXmpp.Protocol;

internal static class PresenceTypeEx
{
    public static string Map(this PresenceType presenceType) => presenceType.ToString().ToLowerInvariant();
}
