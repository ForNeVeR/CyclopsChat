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
        void SendPublicMessage(string body);
        void SendPrivateMessage(IEntityIdentifier target, string body);

        event EventHandler<DisconnectEventArgs> Disconnected;
        event EventHandler<ConferenceJoinEventArgs> Joined;
        event EventHandler<CaptchaEventArgs> CaptchaRequirment;
        event EventHandler<KickedEventArgs> Kicked;
        event EventHandler<BannedEventArgs> Banned;
        event EventHandler InvalidCaptchaCode;
    }
}