namespace Cyclops.Xmpp.Protocol.IqQueries;

public interface ILastIq : ITypedIq<ILastIq>
{
    /// <summary>Seconds since the last activity.</summary>
    int? Seconds { set; }
}
