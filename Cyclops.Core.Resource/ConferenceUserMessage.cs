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
            Conference = userSession.Conferences.FirstOrDefault(i => i.ConferenceId.BaresEqual(room.BareJid));
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
            Conference = userSession.Conferences.FirstOrDefault(i => i.ConferenceId.Bare == msg.From?.Bare);
            IsFromHistory = dataExtractor.GetDelayStamp(msg) != null;
        }

        #region IConferenceMessage Members

        public IUserSession Session => userSession;

        public bool IsSelfMessage { get; private set; }

        public bool IsFromHistory { get; private set; }

        public IConference Conference { get; private set; }

        public Jid AuthorId => msg.From.Value;

        public bool IsAuthorModer
        {
            get
            {
                if (room == null || room.Participants == null)
                    return false;
                return room.Participants.Any(i =>
                    i.RoomParticipantJid == AuthorId && i.IsModer());
            }
        }

        public string AuthorNick => msg.From?.Resource ?? "";

        public string Body => msg.Body;

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

        public bool IsCustom => false;

        #endregion
    }
}
