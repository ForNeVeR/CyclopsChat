using System.ComponentModel.DataAnnotations;

namespace Cyclops.Configuration;

/// <summary>
/// Connection configuration
/// </summary>
public class ConnectionConfig
{
    /// <summary>Server address.</summary>
    public string? Server { get; set; }

    /// <summary>Physical server address.</summary>
    public string? NetworkHost { get; set; }

    [Range(0, 65535)] public int Port { get; set; } = 5222;

    public string? User { get; set; }

    [Display(AutoGenerateField = false)]
    public string? EncodedPassword { get; set; }

    /*
     * TODO: Proxy
     */

    public override string ToString() =>
        $"Server: {Server}; NetworkHost: {NetworkHost}; User: {User}; Port: {Port}; Is password set: {!string.IsNullOrEmpty(EncodedPassword)}";
}
