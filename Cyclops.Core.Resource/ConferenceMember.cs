using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Media.Imaging;
using jabber.connection;
using jabber.protocol;
using jabber.protocol.iq;

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
        }

        // this is workaround (because room_OnParticipantPresenceChange does not fired when current user changed his status
        private void JabberClientOnPresence(object sender, jabber.protocol.client.Presence pres)
        {
            if (pres.From == pres.To && participant.NickJID.Equals(conference.ConferenceId))
            {
                var statusElement = pres.OfType<Element>().GetNodeByName<Element>("status");
                if (statusElement != null)
                {
                    StatusText = statusElement.InnerText;
                    UpdateProperties();
                }
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

        private void room_OnParticipantPresenceChange(Room room, RoomParticipant participant)
        {
            if (this.participant != participant)
                return;
            UpdateProperties();
            StatusText = participant.Presence.Status;
            StatusType = participant.Presence.Show;
        }

        private void UpdateProperties()
        {
            string[] props = { /*"StatusType", "StatusText",*/ "IsModer" };
            foreach (var prop in props)
                OnPropertyChanged(prop);
        }
    }
}