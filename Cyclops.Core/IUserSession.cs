using System;
using System.Windows.Threading;
using Cyclops.Core.Configuration;
using Cyclops.Core.CustomEventArgs;

namespace Cyclops.Core
{
    public interface IUserSession : IDisposable
    {
        bool IsAuthenticating { get; }
        bool IsAuthenticated { get; }
        IEntityIdentifier CurrentUserId { get; }
        IEntityIdentifier ConferenceServiceId { get; }
        ConnectionConfig ConnectionConfig { get; }
        IObservableCollection<IConference> Conferences { get; }
        IObservableCollection<IConferenceMessage> PrivateMessages { get; }
        bool AutoReconnect { get; set; }
        string Status { get; set; }
        StatusType StatusType { get; set; }

        void ChangeStatus(StatusType type, string status);
        void OpenConference(IEntityIdentifier id);
        void AuthenticateAsync(ConnectionConfig config);
        void Close();
        void Reconnect();
        void SendPrivate(IEntityIdentifier target, string body);
        void GetConferenceListAsync(string conferenceService = null);
        void GetBookmarks();
        void StartPrivate(IEntityIdentifier conferenceUserId);
        void RequestVcard(IEntityIdentifier target, Action<Vcard> callback);
        void UpdateVcard(Vcard vcard, Action<bool> callback);

        event EventHandler<ConferencesListEventArgs> ConferencesListReceived;
        event EventHandler<AuthenticationEventArgs> Authenticated;
        event EventHandler<AuthenticationEventArgs> ConnectionDropped;
        event EventHandler PublicMessage;
        event EventHandler<ErrorEventArgs> ErrorMessageRecieved;
    }
}