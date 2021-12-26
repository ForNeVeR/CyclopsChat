namespace Cyclops.Xmpp.Data
{
    public interface IEntityIdentifier
    {
        string Server { get; }
        string User { get; }
        string Resource { get; }
    }
}
