namespace Cyclops.Xmpp.Protocol.IqQueries;

public interface ITimeIq : ITypedIq<ITimeIq>
{
    (DateTime, TimeZoneInfo) Time { set; }
}
