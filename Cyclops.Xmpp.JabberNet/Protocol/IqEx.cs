using System.Xml;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.Protocol.IqQueries;
using jabber.protocol.client;
using jabber.protocol.iq;
using VCard = Cyclops.Xmpp.Data.VCard;
using Version = jabber.protocol.iq.Version;

namespace Cyclops.Xmpp.JabberNet.Protocol;

internal static class IqEx
{
    private class Iq : Stanza, IIq
    {
        public readonly IQ IqValue;

        public Iq(IQ iq) : base(iq)
        {
            IqValue = iq;
        }

        public XmlElement? Error => IqValue.Error;
    }

    private class TimeIq : Iq, ITimeIq
    {
        public TimeIq(IQ iq) : base(iq)
        {
        }

        public ITimeIq CreateResponse() =>
            IqValue.GetResponse(IqValue.OwnerDocument!).WrapTime();

        public (DateTime, TimeZoneInfo) Time
        {
            set
            {
                var (dateTime, timeZone) = value;

                var query = (Time)IqValue.Query;
                query.UTC = dateTime.ToUniversalTime();
                query.TZ = timeZone.IsDaylightSavingTime(dateTime) ? timeZone.DaylightName : timeZone.StandardName;
                query.Display = dateTime.ToString("s");
            }
        }
    }

    private class LastIq : Iq, ILastIq
    {
        public LastIq(IQ iq) : base(iq)
        {
        }

        public ILastIq CreateResponse() =>
            IqValue.GetResponse(IqValue.OwnerDocument!).WrapLast();

        public int? Seconds
        {
            set => ((Last)IqValue.Query).Seconds = value ?? -1;
        }
    }

    private class VersionIq : Iq, IVersionIq
    {
        public VersionIq(IQ iq) : base(iq)
        {
        }

        public IVersionIq CreateResponse() =>
            IqValue.GetResponse(IqValue.OwnerDocument!).WrapVersion();

        public ClientInfo ClientInfo
        {
            set
            {
                var query = (Version)IqValue.Query;
                query.OS = value.Os;
                query.Ver = value.Version;
                query.EntityName = value.Client;
            }
        }
    }

    public static IIq Wrap(this IQ iq) => new Iq(iq);
    public static IQ Unwrap(this IIq iq) => ((Iq)iq).IqValue;

    public static ITimeIq WrapTime(this IQ iq) => new TimeIq(iq);
    public static ILastIq WrapLast(this IQ iq) => new LastIq(iq);
    public static IVersionIq WrapVersion(this IQ iq) => new VersionIq(iq);

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
