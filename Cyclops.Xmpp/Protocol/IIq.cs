using System.Xml;

namespace Cyclops.Xmpp.Protocol;

public interface IIq : IStanza
{
    public XmlElement? Error { get; }
}

public interface ITypedIq<T> : IIq where T : IIq
{
    public T CreateResponse();
}
