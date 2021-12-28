namespace Cyclops.Xmpp.Protocol;

public interface IError
{
    int Code { get; }
    string? Message { get; }
}
