namespace Cyclops.Xmpp.Protocol;

public interface IMessage : IStanza
{
    public string? Subject { get; }
    public string? Body { get; }

    IError? Error { get; }
}
