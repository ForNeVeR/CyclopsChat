using System.Drawing;

namespace Cyclops.Xmpp.Data;

public class VCard
{
    public Image? Photo { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public DateTime Birthday { get; set; }
    public string? Nick { get; set; }
    public string? Comments { get; set; }
}
