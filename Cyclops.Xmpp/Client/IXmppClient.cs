using System.Xml;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Client;

public interface IXmppClient
{
    event EventHandler Connect;
    event EventHandler<string> ReadRawMessage;
    event EventHandler<string> WriteRawMessage;
    event EventHandler<Exception> Error;

    event EventHandler<IPresence> Presence;

    void SendElement(XmlElement element);

    Task<Vcard> GetVCard(IEntityIdentifier jid);
}
