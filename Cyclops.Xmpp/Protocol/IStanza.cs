namespace Cyclops.Xmpp.Protocol;

public interface IStanza
{
    Jid? From { get; }
    Jid? To { get; }
}
