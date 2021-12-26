using Cyclops.Core;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Cyclops.Xmpp.JabberNet;

internal static class IqEx
{
    public static Vcard ToVCard(this IQ? iq)
    {
        var result = new Vcard();
        if (iq == null) return result;

        var vcard = (VCard)iq.Query;
        result.Photo = vcard.Photo?.Image;
        result.Email = vcard.Email;
        result.FullName = vcard.FullName;
        result.Birthday = vcard.Birthday;
        result.Nick = vcard.Nickname ?? iq.From.Resource;
        result.Comments = vcard.Description;
        return result;
    }
}
