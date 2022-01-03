namespace Cyclops.Xmpp.Protocol;

public static class Attributes
{
    /// <remarks>XEP-0045: Multi-User Chat</remarks>
    public const string Affiliation = "affiliation";

    /// <remarks>XEP-0045: Multi-User Chat</remarks>
    public const string Code = "code";

    /// <remarks>
    /// Not found in XEP-0045: Multi-User Chat.
    /// TODO: clarify if such attribute exists; it was defined in Jabber-Net.
    /// </remarks>
    public const string Jid = "jid";

    /// <remarks>XEP-0045: Multi-User Chat</remarks>
    public const string From = "from";

    /// <remarks>XEP-0045: Multi-User Chat</remarks>
    public const string Role = "role";

    /// <remarks>XEP-0012: Last Activity</remarks>
    public const string Seconds = "seconds";
}
