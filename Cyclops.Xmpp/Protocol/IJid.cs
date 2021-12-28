namespace Cyclops.Xmpp.Protocol;

public struct Jid : IEquatable<Jid>
{
    public Jid(string user, string server, string? resource = null)
    {
        User = user ?? throw new ArgumentNullException(nameof(user));
        Server = server ?? throw new ArgumentNullException(nameof(server));
        Resource = resource;
    }

    public string User { get; }
    public string Server { get; }
    public string? Resource { get; }

    public Jid Bare => new(Server, User);

    public override string ToString()
    {
        if (Resource == null)
            return $"{User}@{Server}";

        return $"{User}@{Server}/{Resource}";
    }

    public bool Equals(Jid other) => User == other.User && Server == other.Server && Resource == other.Resource;
    public override bool Equals(object? obj) => obj is Jid other && Equals(other);
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Server.GetHashCode();
            hashCode = (hashCode * 397) ^ User.GetHashCode();
            hashCode = (hashCode * 397) ^ (Resource != null ? Resource.GetHashCode() : 0);
            return hashCode;
        }
    }

    public static bool operator ==(Jid? left, Jid? right) => left.Equals(right);

    public static bool operator !=(Jid? left, Jid? right) => !left.Equals(right);
}
