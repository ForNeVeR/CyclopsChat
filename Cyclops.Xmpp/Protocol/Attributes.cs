namespace Cyclops.Xmpp.Protocol;

public static class Attributes
{
    /// <remarks>XEP-0045: Multi-User Chat</remarks>
    public const string Affiliation = "affiliation";

    /// <remarks>XEP-0048: Bookmarks</remarks>
    public const string AutoJoin = "autojoin";

    /// <remarks>XEP-0045: Multi-User Chat</remarks>
    public const string Code = "code";

    /// <remarks>
    /// Not found in XEP-0045: Multi-User Chat.
    /// TODO: clarify if such attribute exists; it was defined in Jabber-Net.
    /// XEP-0030: Service Discovery
    /// </remarks>
    public const string Jid = "jid";

    /// <remarks>XEP-0045: Multi-User Chat</remarks>
    public const string From = "from";

    /// <remarks>XEP-0030: Service Discovery</remarks>
    public const string Name = "name";

    /// <remarks>XEP-0045: Multi-User Chat</remarks>
    public const string Nick = "nick";

    /// <remarks>XEP-0030: Service Discovery</remarks>
    public const string Node = "node";

    /// <remarks>XEP-0045: Multi-User Chat</remarks>
    public const string Role = "role";

    /// <remarks>XEP-0012: Last Activity</remarks>
    public const string Seconds = "seconds";

    /// <remarks>XEP-0203: Delayed Delivery</remarks>
    public const string Stamp = "stamp";

    /// <remarks>XEP-0045: Multi-User Chat</remarks>
    public const string Type = "type";

    /// <remarks>
    /// XEP-0030: Service Discovery
    /// XEP-0158: CAPTCHA Forms
    /// </remarks>
    public const string Var = "var";
}
