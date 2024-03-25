using System;
using System.Threading.Tasks;
using Cyclops.Core.Avatars;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core
{
    public interface IConference : ISessionHolder
    {
        string Subject { get; }
        bool IsInConference { get; }

        IAvatarsManager AvatarsManager { get; }
        Jid ConferenceId { get; }
        IObservableCollection<IConferenceMember> Members { get; }
        IObservableCollection<IConferenceMessage> Messages { get; }

        void Leave(string reason = "");
        void LeaveAndClose(string reason = "");
        void ChangeSubject(string s);
        Task SendPublicMessage(string body);
        bool ChangeNick(string value);
        void RejoinWithNewNick(string nick);
        void ChangeNickAndStatus(string nick, StatusType statusType, string status);

        event EventHandler NotFound;
        event EventHandler<RoleChangedEventArgs> RoleChanged;
        event EventHandler<ConferenceMemberEventArgs> SomebodyChangedHisStatus;
        event EventHandler<DisconnectEventArgs> Disconnected;
        event EventHandler<ConferenceJoinEventArgs> Joined;
        event EventHandler<CaptchaEventArgs> CaptchaRequirment;
        event EventHandler<KickedEventArgs> Kicked;
        event EventHandler<BannedEventArgs> Banned;
        event EventHandler InvalidCaptchaCode;
        event EventHandler AccessDenied;
        event EventHandler<NickChangeEventArgs> NickChange;
        event EventHandler BeginJoin;
        event EventHandler StartReconnectTimer;
        event EventHandler ServiceUnavailable;
        event EventHandler CantChangeSubject;
        event EventHandler MethodNotAllowedError;
        event EventHandler<SubjectChangedEventArgs> SubjectChanged;
        event EventHandler<ConferenceMemberEventArgs> ParticipantLeave;
        event EventHandler<ConferenceMemberEventArgs> ParticipantJoin;
    }
}
