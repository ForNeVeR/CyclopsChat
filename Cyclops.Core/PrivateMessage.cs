using System;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core
{
    public class PrivateMessage : IConferenceMessage
    {
        public IConference Conference { get; set; }

        #region IConferenceMessage Members

        public IUserSession Session => null;

        public Jid AuthorId { get; set; }

        public bool IsAuthorModer => false;

        public bool IsFromHistory { get; private set; }

        public string AuthorNick { get; set; }

        public string Body { get; set; }

        public DateTime Timestamp => DateTime.Now;

        public bool IsCustom => true;

        public bool IsSelfMessage { get; set; }

        #endregion
    }
}
