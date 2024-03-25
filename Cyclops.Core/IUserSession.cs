using System;
using System.Threading.Tasks;
using Cyclops.Configuration;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core
{
    public interface IUserSession : IDisposable
    {
        bool IsAuthenticating { get; }
        bool IsAuthenticated { get; }
        Jid CurrentUserId { get; }
        Jid ConferenceServiceId { get; }
        ConnectionConfig ConnectionConfig { get; }
        IObservableCollection<IConference> Conferences { get; }
        IObservableCollection<IConferenceMessage> PrivateMessages { get; }
        bool AutoReconnect { get; set; }
        string Status { get; set; }
        StatusType StatusType { get; set; }

        void SendPresence(PresenceDetails presenceDetails);
        void SendIq(IIq iq);

        void ChangeStatus(StatusType type, string status);
        void OpenConference(Jid id);
        void AuthenticateAsync(ConnectionConfig config);
        void Close();
        void Reconnect();
        void SendPrivate(Jid target, string body);
        void RefreshConferenceList(string? conferenceService);
        void RaiseBookmarksReceived();
        void StartPrivate(Jid conferenceUserId);

        Task<ClientInfo?> GetClientInfo(Jid jid);
        Task<VCard> GetVCard(Jid target);
        Task UpdateVCard(VCard vCard);

        event EventHandler<ConferencesListEventArgs> ConferencesListReceived;
        event EventHandler<AuthenticationEventArgs> Authenticated;
        event EventHandler<AuthenticationEventArgs> ConnectionDropped;
        event EventHandler<IPresence> Presence;
        event EventHandler PublicMessage;
        event EventHandler<ErrorEventArgs> ErrorMessageRecieved;
        void RemoveFromBookmarks(Jid conferenceId);
        void AddToBookmarks(Jid conferenceId);
    }
}
