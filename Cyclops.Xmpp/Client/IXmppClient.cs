using System.Xml;
using Cyclops.Core;
using Cyclops.Xmpp.Data;

namespace Cyclops.Xmpp.Client;

public interface IXmppClient
{
    event EventHandler Connect;
    event EventHandler<string> ReadRawMessage;
    event EventHandler<string> WriteRawMessage;
    event EventHandler<Exception> Error;

    void SendElement(XmlElement element);

    Task<Vcard> GetVCard(IEntityIdentifier jid);
}
