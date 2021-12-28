using System;
using System.Linq;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.Resource
{
    public class ConferenceUserMessage : IConferenceMessage
    {
        private readonly IMessage msg;
        private readonly UserSession userSession;
        private readonly IRoom room;

        internal ConferenceUserMessage(
            IXmppDataExtractor dataExtractor,
            UserSession userSession,
            IRoom room,
            IMessage msg)
        {
            this.userSession = userSession;
            this.room = room;
            this.msg = msg;
            Conference = userSession.Conferences.FirstOrDefault(i => i.ConferenceId.BaresEqual(room.Jid));
            IsFromHistory = dataExtractor.GetDelayStamp(msg) != null;
        }

        internal ConferenceUserMessage(
            IXmppDataExtractor dataExtractor,
            UserSession userSession,
            IMessage msg,
            bool selfMessage)
        {
            this.userSession = userSession;
            this.msg = msg;
            this.IsSelfMessage = selfMessage;
            Conference = userSession.Conferences.FirstOrDefault(i => i.ConferenceId.BaresEqual(msg.From));
            IsFromHistory = dataExtractor.GetDelayStamp(msg) != null;
        }

        #region IConferenceMessage Members

        public IUserSession Session
        {
            get { return userSession; }
        }

        public bool IsSelfMessage { get; private set; }

        public bool IsFromHistory { get; private set; }

        public IConference Conference { get; private set; }

        public IEntityIdentifier AuthorId
        {
            get { return msg.From; }
        }

        public bool IsAuthorModer
        {
            get
            {
                if (room == null || room.Participants == null)
                    return false;
                return room.Participants.Any(i =>
                    i.RoomParticipantJid?.Equals(AuthorId) == true && i.IsModer());
            }
        }

        public string AuthorNick
        {
            get { return msg.From.Resource; }
        }

        public string Body
        {
            get { return msg.Body; }
        }

        private DateTime timestamp = DateTime.MinValue;
        public DateTime Timestamp
        {
            get
            {
                if (timestamp == DateTime.MinValue)
                    return DateTime.Now;
                return timestamp;
            }
            set { timestamp = value; }
        }

        public bool IsCustom
        {
            get { return false; }
        }

        #endregion
    }
}
