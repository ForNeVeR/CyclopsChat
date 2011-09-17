using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Media.Imaging;
using Cyclops.Core.Resource.JabberNetExtensions;
using jabber.connection;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;
using Version = System.Version;

namespace Cyclops.Core.Resource
{
    public class ConferenceMember : NotifyPropertyChangedBase, IConferenceMember
    {
        private readonly RoomParticipant participant;
        private readonly Room room;
        private readonly UserSession session;
        private readonly Conference conference;
        private string statusText;
        private string statusType;
        private BitmapImage avatarUrl;
        private bool isSubscribed;

        internal ConferenceMember(UserSession session, Conference conference, RoomParticipant participant, Room room)
        {
            this.session = session;
            this.conference = conference;
            this.participant = participant;
            this.room = room;
            conferenceUserId = participant.NickJID;
            room.OnParticipantPresenceChange += room_OnParticipantPresenceChange;
            session.JabberClient.OnPresence += JabberClientOnPresence;
            room_OnParticipantPresenceChange(room, participant); //force call
            Role = RoleConversion(participant);
            JabberCommonHelper.GetClientVersionAsnyc(session, participant.NickJID, OnClientInfoGot);
        }

        private void OnClientInfoGot(object sender, IQ iq, object data)
        {
            if (iq == null || iq.Error != null || !(iq.Query is jabber.protocol.iq.Version))
                return;
            var version = iq.Query as jabber.protocol.iq.Version;
            ClientInfo = new ClientInfo(version.OS, version.Ver, version.EntityName);
        }

        // this is workaround (because room_OnParticipantPresenceChange does not fired when current user changed his status
        private void JabberClientOnPresence(object sender, jabber.protocol.client.Presence pres)
        {
            if (pres.From == pres.To && participant.NickJID.Equals(conference.ConferenceId))
            {
                var statusElement = pres.OfType<Element>().GetNodeByName<Element>("status");
                if (statusElement != null)
                {
                    StatusText = pres.Status;
                    StatusType = pres.Show;
                }
            }
            if (participant.NickJID.Equals(conference.ConferenceId))
            {
                Role = RoleConversion(participant);
            }
        }

        #region Implementation of ISessionHolder

        public IUserSession Session
        {
            get { return session; }
        }

        #endregion

        #region Implementation of IConferenceMember

        public string Nick
        {
            get { return participant.Nick; }
        }

        public bool IsModer
        {
            get { return Role >= Role.Moder; }
        }

        public bool IsMe
        {
            get { return ConferenceUserId.Equals(conference.ConferenceId); }
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

        private Role role;
        public Role Role
        {
            get { return role; }
            set
            {
                role = value;
                OnPropertyChanged("Role");
                OnPropertyChanged("IsModer");
            }
        }

        private ClientInfo clientInfo = null;
        public ClientInfo ClientInfo
        {
            get
            {
                return clientInfo;
            }
            private set
            {
                clientInfo = value;
                OnPropertyChanged("ClientInfo");
            }
        }

        public string StatusText
        {
            get { return statusText; }
            set
            {
                statusText = value;
                OnPropertyChanged("StatusText");
            }
        }

        public string StatusType
        {
            get { return statusType; }
            set
            {
                statusType = value;
                OnPropertyChanged("StatusType");
            }
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

        private bool isFirstPresence = true;

        private void room_OnParticipantPresenceChange(Room room, RoomParticipant participant)
        {
            if (this.participant != participant)
                return;
            Role = RoleConversion(participant);
            StatusText = participant.Presence.Status;
            StatusType = participant.Presence.Show;
            if (!isFirstPresence)
                conference.RaiseSomebodyChangedHisStatusEvent(this);
            else isFirstPresence = false;
        }

        private Role RoleConversion(RoomParticipant roomParticipant)
        {
            return JabberCommonHelper.ConvertRole(roomParticipant.Role, roomParticipant.Affiliation);
        }
    }
}