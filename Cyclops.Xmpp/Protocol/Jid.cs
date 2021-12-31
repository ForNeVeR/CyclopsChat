using System.Text;

namespace Cyclops.Xmpp.Protocol;

public readonly struct Jid : IEquatable<Jid>
{
    public static Jid Parse(string jid)
    {
        string localAndDomain;
        string? resource;

        var indexOfSlash = jid.IndexOf('/');
        if (indexOfSlash == -1)
        {
            localAndDomain = jid;
            resource = null;
        }
        else
        {
            localAndDomain = jid.Substring(0, indexOfSlash);
            resource = jid.Substring(indexOfSlash + 1);
            if (string.IsNullOrEmpty(resource))
                throw new ArgumentException($"Resource part of a JID {jid} is empty.", nameof(jid));
        }

        string? local;
        string domain;
        var indexOfAt = localAndDomain.IndexOf('@');
        if (indexOfAt == -1)
        {
            local = null;
            domain = localAndDomain;
        }
        else
        {
            local = localAndDomain.Substring(0, indexOfAt);
            domain = localAndDomain.Substring(indexOfAt + 1);
        }

        if (string.IsNullOrEmpty(domain))
            throw new ArgumentException($"Domain part of a JID {jid} is empty.", nameof(jid));

        return new Jid(local, domain, resource);
    }

    public Jid(string? local, string domain, string? resource = null)
    {
        Local = local;
        Domain = domain ?? throw new ArgumentNullException(nameof(domain));
        Resource = resource;
    }

    public string? Local { get; }
    public string Domain { get; }
    public string? Resource { get; }

    public Jid WithResource(string? resource) => new(Local, Domain, resource);
    public Jid Bare => WithResource(null);

    public override string ToString()
    {
        var result = new StringBuilder();
        if (Local != null)
            result.Append($"{Local}@");
        result.Append(Domain);
        if (Resource != null)
            result.Append($"/{Resource}");

        return result.ToString();
    }

    public bool Equals(Jid other) => Local == other.Local && Domain == other.Domain && Resource == other.Resource;
    public override bool Equals(object? obj) => obj is Jid other && Equals(other);
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Domain.GetHashCode();
            hashCode = (hashCode * 397) ^ (Local?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (Resource?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public static bool operator ==(Jid? left, Jid? right) => left.Equals(right);

    public static bool operator !=(Jid? left, Jid? right) => !left.Equals(right);
}
