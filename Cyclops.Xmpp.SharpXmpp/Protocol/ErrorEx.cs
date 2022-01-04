using System.Globalization;
using System.Xml.Linq;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Protocol;

internal static class ErrorEx
{
    private class Error : IError
    {
        private XElement error;
        public Error(XElement error)
        {
            this.error = error;
        }

        public int? Code
        {
            get
            {
                var code = error.Attribute(Attributes.Code)?.Value;
                if (code == null) return null;
                return int.TryParse(code, NumberStyles.Integer, CultureInfo.InvariantCulture, out var c) ? c : null;
            }
        }

        public string? Message => error.Element(error.Name.Namespace + Elements.Text)?.Value;
    }

    public static IError WrapAsError(this XElement error) => new Error(error);
}
