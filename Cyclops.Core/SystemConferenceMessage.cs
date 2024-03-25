using System;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core
{
    public class SystemConferenceMessage : IConferenceMessage
    {
        public bool IsErrorMessage { get; set; }

        #region IConferenceMessage Members

        public IUserSession Session => null;

        public bool IsFromHistory { get; private set; }

        public IConference Conference => null;

        public Jid AuthorId { get; set; }

        public bool IsAuthorModer => false;

        public string AuthorNick => "System";

        public string Body { get; set; }

        public DateTime Timestamp => DateTime.Now;

        public bool IsCustom => true;

        public bool IsSelfMessage => false;

        #endregion
    }
}
