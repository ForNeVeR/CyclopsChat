using System;
using System.Linq;
using Cyclops.Xmpp.Data;
using jabber.connection;
using jabber.protocol.client;
using jabber.protocol.x;

namespace Cyclops.Core.Resource
{
    public class ConferenceUserMessage : IConferenceMessage
    {
        private readonly Message msg;
        private readonly UserSession userSession;
        private readonly Room room;

        internal ConferenceUserMessage(UserSession userSession, Room room, Message msg)
        {
            this.userSession = userSession;
            this.room = room;
            this.msg = msg;
            Conference = userSession.Conferences.FirstOrDefault(i => i.ConferenceId.BaresEqual(room.JID));
            IsFromHistory = msg.OfType<Delay>().Any();
        }

        internal ConferenceUserMessage(UserSession userSession, Message msg, bool selfMessage)
        {
            this.userSession = userSession;
            this.msg = msg;
            this.IsSelfMessage = selfMessage;
            Conference = userSession.Conferences.FirstOrDefault(i => i.ConferenceId.BaresEqual(msg.From));
            IsFromHistory = msg.OfType<Delay>().Any();
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
                return room.Participants.OfType<RoomParticipant>().Any(i =>
                    i.NickJID != null && i.NickJID.Equals(AuthorId) && i.IsModer());
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