namespace Cyclops.Xmpp.Protocol;

public interface IPresence : IStanza
{
    public string? Status { get; }
    public string? Show { get; }
}
