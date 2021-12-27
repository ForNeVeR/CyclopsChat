namespace Cyclops.Xmpp.Client;

public interface IPresence : IStanza
{
    public string? Status { get; }
    public string? Show { get; }
}
