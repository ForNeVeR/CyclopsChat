using System;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core
{
    public class PrivateMessage : IConferenceMessage
    {
        public IConference Conference { get; set; }

        #region IConferenceMessage Members

        public IUserSession Session
        {
            get { return null; }
        }

        public IEntityIdentifier AuthorId { get; set; }

        public bool IsAuthorModer
        {
            get { return false; }
        }
        
        public bool IsFromHistory { get; private set; }

        public string AuthorNick { get; set; }

        public string Body { get; set; }

        public DateTime Timestamp
        {
            get { return DateTime.Now; }
        }

        public bool IsCustom
        {
            get { return true; }
        }

        public bool IsSelfMessage { get; set; }

        #endregion
    }
}