namespace Cyclops.Xmpp.SharpXmpp.Errors;

/// <summary>An exception produced when there are no other exception available.</summary>
public class MessageOnlyException : Exception
{
    public MessageOnlyException(string? message) : base(message ?? "Unknown error")
    {
    }
}
