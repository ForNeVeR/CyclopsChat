namespace Cyclops.Xmpp.Protocol;

public interface IPresence : IStanza
{
    string? Status { get; }
    string? Show { get; }
    IError? Error { get; }
}
