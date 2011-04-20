using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Media.Imaging;
using jabber.connection;
using jabber.protocol.iq;

namespace Cyclops.Core.Resource
{
    public class ConferenceMember : NotifyPropertyChangedBase, IConferenceMember
    {
        private readonly RoomParticipant participant;
        private readonly Room room;
        private readonly UserSession session;
        private BitmapImage avatarUrl;

        internal ConferenceMember(UserSession session, RoomParticipant participant, Room room)
        {
            this.session = session;
            this.participant = participant;
            this.room = room;
            room.OnParticipantPresenceChange += room_OnParticipantPresenceChange;
        }

        #region Implementation of ISessionHolder

        public IUserSession Session
        {
            get { return session; }
        }

        #endregion

        #region Implementation of IConferenceMember

        private readonly Random random = new Random(Environment.TickCount);

        public string Nick
        {
            get { return participant.Nick; }
        }

        public bool IsModer
        {
            get { return participant.IsModer(); }
        }

        public string StatusText
        {
            get { return participant.Presence.Status; }
        }

        public string StatusType
        {
            get { return participant.Presence.Show; }
        }

        public BitmapImage AvatarUrl
        {
            get { return avatarUrl; }
            internal set
            {
                avatarUrl = value;
                OnPropertyChanged("AvatarUrl");
            }
        }

        public IEntityIdentifier ConferenceUserId
        {
            get { return participant.NickJID; }
        }

        public IEntityIdentifier RealUserId
        {
            get { return participant.RealJID; }
        }

        #endregion

        private void room_OnParticipantPresenceChange(Room room, RoomParticipant participant)
        {
            if (this.participant == participant)
                foreach (PropertyInfo property in GetType().GetProperties())
                    OnPropertyChanged(property.Name); //Request for UI to update all member fields
        }
    }
}