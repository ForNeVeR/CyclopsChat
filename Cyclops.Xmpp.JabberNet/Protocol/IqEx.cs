using System.Xml;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;
using jabber.protocol.client;

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

    public static VCard ToVCard(this IQ iq)
    {
        var vCard = (jabber.protocol.iq.VCard)iq.Query;
        return new VCard
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
