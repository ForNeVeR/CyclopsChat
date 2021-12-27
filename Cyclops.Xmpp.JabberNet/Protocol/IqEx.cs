using System.Xml;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Cyclops.Xmpp.JabberNet.Protocol;

internal static class IqEx
{
    private class Iq : Stanza, IIq
    {
        private readonly IQ iq;
        public Iq(IQ iq) : base(iq)
        {
            this.iq = iq;
        }

        public XmlElement? Error => iq.Error;
    }

    public static IIq Wrap(this IQ iq) => new Iq(iq);

    public static Vcard ToVCard(this IQ iq)
    {
        var vCard = (VCard)iq.Query;
        return new Vcard
        {
            Photo = vCard.Photo?.Image,
            Email = vCard.Email,
            FullName = vCard.FullName,
            Birthday = vCard.Birthday,
            Nick = vCard.Nickname ?? iq.From?.Resource,
            Comments = vCard.Description
        };
    }
}
