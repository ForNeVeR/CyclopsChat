using System;

namespace Cyclops.Core
{
    public class SystemConferenceMessage : IConferenceMessage
    {
        public bool IsErrorMessage { get; set; }

        #region IConferenceMessage Members

        public IUserSession Session
        {
            get { return null; }
        }

        public bool IsFromHistory { get; private set; }

        public IConference Conference
        {
            get { return null; }
        }

        public IEntityIdentifier AuthorId { get; set; }

        public bool IsAuthorModer
        {
            get { return false; }
        }

        public string AuthorNick
        {
            get { return "System"; }
        }

        public string Body { get; set; }

        public DateTime Timestamp
        {
            get { return DateTime.Now; }
        }

        public bool IsCustom
        {
            get { return true; }
        }

        public bool IsSelfMessage
        {
            get { return false; }
        }

        #endregion
    }
}