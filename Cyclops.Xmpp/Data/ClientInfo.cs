namespace Cyclops.Xmpp.Data;

public class ClientInfo
{
    public ClientInfo(string? os, string? version, string? client)
    {
        Os = os;
        Version = version;
        Client = client;
    }

    public string? Os { get; }
    public string? Version { get; }
    public string? Client { get; }

    public override string ToString() => $"{Client} {Version} ({Os})";
}
