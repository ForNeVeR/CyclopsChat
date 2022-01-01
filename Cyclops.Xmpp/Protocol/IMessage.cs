namespace Cyclops.Xmpp.Protocol;

public interface IMessage : IStanza
{
    string? Subject { get; }
    string? Body { get; }

    IError? Error { get; }
    MessageType Type { get; }
}
