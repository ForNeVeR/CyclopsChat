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
        private bool isSubscribed;

        internal ConferenceMember(UserSession session, RoomParticipant participant, Room room)
        {
            this.session = session;
            this.participant = participant;
            this.room = room;
            conferenceUserId = participant.NickJID;
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

        public bool IsSubscribed
        {
            get { return isSubscribed; }
            set
            {
                isSubscribed = value;
                OnPropertyChanged("IsSubscribed");
            }
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

        private IEntityIdentifier conferenceUserId;
        public IEntityIdentifier ConferenceUserId
        {
            get { return conferenceUserId; }
            set
            {
                conferenceUserId = value;
                OnPropertyChanged("ConferenceUserId");
            }
        }

        public IEntityIdentifier RealUserId
        {
            get { return participant.RealJID; }
        }

        #endregion

        private void room_OnParticipantPresenceChange(Room room, RoomParticipant participant)
        {
            if (this.participant == participant)
            {
                string[] props = {"StatusType", "StatusText", "IsModer"};
                foreach (var prop in props)
                    OnPropertyChanged(prop);
            }
        }
    }
}