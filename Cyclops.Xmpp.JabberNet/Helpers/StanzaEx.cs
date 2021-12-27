using System.Xml;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.JabberNet.Helpers;

internal static class StanzaEx
{
    public static T? GetNodeByName<T>(this IStanza stanza, string name) where T : XmlNode =>
        stanza.Elements.FirstOrDefault(i => string.Equals(i.Name, name, StringComparison.InvariantCultureIgnoreCase)) as T;
}
