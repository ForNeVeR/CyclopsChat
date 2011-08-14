using System;
using Cyclops.Core.Avatars;
using Cyclops.Core.CustomEventArgs;

namespace Cyclops.Core
{
    public interface IConference : ISessionHolder
    {
        string Subject { get; }
        bool IsInConference { get; }

        IAvatarsManager AvatarsManager { get; }
        IEntityIdentifier ConferenceId { get; }
        IObservableCollection<IConferenceMember> Members { get; }
        IObservableCollection<IConferenceMessage> Messages { get; }

        void Leave(string reason = "");
        void LeaveAndClose(string reason = "");
        void ChangeSubject(string s);
        void SendPublicMessage(string body);
        bool ChangeNick(string value);
        void SendPrivateMessage(IEntityIdentifier target, string body);
        void RejoinWithNewNick(string nick);

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