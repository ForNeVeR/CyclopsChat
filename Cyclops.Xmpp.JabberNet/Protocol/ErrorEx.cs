using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.JabberNet.Protocol;

internal static class ErrorEx
{
    private class Error : IError
    {
        private jabber.protocol.client.Error error;
        public Error(jabber.protocol.client.Error error)
        {
            this.error = error;
        }

        public int? Code => error.Code;
        public string? Message => error.Message;
    }

    public static IError Wrap(this jabber.protocol.client.Error error) => new Error(error);
}
