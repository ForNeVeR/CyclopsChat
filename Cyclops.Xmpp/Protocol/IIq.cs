namespace Cyclops.Xmpp.Protocol;

public interface IIq : IStanza
{
    IError? Error { get; }
}

public interface ITypedIq<T> : IIq where T : IIq
{
    T CreateResponse();
}
