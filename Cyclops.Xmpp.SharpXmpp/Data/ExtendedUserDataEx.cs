using System.Globalization;
using System.Xml.Linq;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Data;

internal static class ExtendedUserDataEx
{
    private class ExtendedUserData : IExtendedUserData
    {
        private readonly XElement x;
        public ExtendedUserData(XElement x)
        {
            this.x = x;
        }

        public IReadOnlyList<MucUserStatus?> Status => x.Elements(XNamespace.Get(Namespaces.Muc) + "status")
            .Select(e => MapStatusCode(e.Attribute(Attributes.Code)?.Value))
            .ToList();
        public IRoomItem? RoomItem => throw new NotImplementedException();
    }

    public static IExtendedUserData WrapUserData(this XElement x) => new ExtendedUserData(x);

    private static MucUserStatus? MapStatusCode(string? code)
    {
        if (code == null) return null;
        if (int.TryParse(code, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intCode))
        {
            return (MucUserStatus)intCode;
        }

        return null;
    }
}
