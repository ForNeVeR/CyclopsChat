using System.Xml.Linq;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Protocol;

internal static class ErrorEx
{
    private class Error : IError
    {
        public int Code => throw new NotImplementedException();
        public string? Message => throw new NotImplementedException();
    }

    public static IError? WrapAsError(this XElement error) => new Error();
}
