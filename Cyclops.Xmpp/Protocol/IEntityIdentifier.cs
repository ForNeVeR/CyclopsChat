namespace Cyclops.Xmpp.Protocol
{
    public interface IEntityIdentifier
    {
        string Server { get; }
        string User { get; }
        string Resource { get; }
    }
}
