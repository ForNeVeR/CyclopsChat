using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Cyclops.Core.Helpers;
using Cyclops.Core.Resource.JabberNetExtensions;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.Resource
{
    public class ConferenceMember : NotifyPropertyChangedBase, IConferenceMember
    {
        private readonly IMucParticipant participant;
        private readonly IRoom room;
        private readonly UserSession session;
        private readonly Conference conference;
        private string? statusText;
        private string? statusType;
        private BitmapImage avatarUrl;
        private bool isSubscribed;

        internal ConferenceMember(
            ILogger logger,
            UserSession session,
            Conference conference,
            IMucParticipant participant,
            IRoom room)
        {
            this.session = session;
            this.conference = conference;
            this.participant = participant;
            this.room = room;
            conferenceUserId = participant.RoomParticipantJid;
            room.ParticipantPresenceChange += room_OnParticipantPresenceChange;
            session.Presence += OnPresence;
            room_OnParticipantPresenceChange(room, participant); //force call
            Role = RoleConversion(participant);
            ProcessClientVersion().NoAwait(logger);
        }

        private async Task ProcessClientVersion()
        {
            ClientInfo = await session.GetClientInfo(participant.RoomParticipantJid);
        }

        // this is workaround (because room_OnParticipantPresenceChange does not fired when current user changed his status
        private void OnPresence(object sender, IPresence presence)
        {
            if (presence.From?.Equals(presence.To) == true && participant.RoomParticipantJid.Equals(conference.ConferenceId))
            {
                StatusText = presence.Status;
                StatusType = presence.Show;
            }
            if (participant.RoomParticipantJid.Equals(conference.ConferenceId))
            {
                Role = RoleConversion(participant);
            }
        }

        #region Implementation of ISessionHolder

        public IUserSession Session => session;

        #endregion

        #region Implementation of IConferenceMember

        public string Nick => participant.Nick;

        public bool IsModer => Role >= Role.Moder;

        public bool IsMe => ConferenceUserId.Equals(conference.ConferenceId);

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

        private ClientInfo? clientInfo;
        public ClientInfo? ClientInfo
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

        public string? StatusText
        {
            get { return statusText; }
            set
            {
                statusText = value;
                OnPropertyChanged("StatusText");
            }
        }

        public string? StatusType
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

        private Jid conferenceUserId;
        public Jid ConferenceUserId
        {
            get { return conferenceUserId; }
            set
            {
                conferenceUserId = value;
                OnPropertyChanged("ConferenceUserId");
            }
        }

        public Jid? RealUserId => participant.RealJid;

        #endregion

        private bool isFirstPresence = true;

        private void room_OnParticipantPresenceChange(object _, IMucParticipant participant)
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

        private Role RoleConversion(IMucParticipant roomParticipant)
        {
            return JabberCommonHelper.ConvertRole(roomParticipant.Role, roomParticipant.Affiliation);
        }
    }
}
