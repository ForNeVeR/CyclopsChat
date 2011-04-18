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
        Dispatcher Dispatcher { get; set; }
        IEntityIdentifier CurrentUserId { get; }
        ConnectionConfig ConnectionConfig { get; }
        IObservableCollection<IConference> Conferences { get; }
        IObservableCollection<IConferenceMessage> PrivateMessages { get; }

        void OpenConference(string name, string server, string nick);
        void AuthenticateAsync(ConnectionConfig config);
        void Close();
        void Reconnect();
        void SendPrivate(IEntityIdentifier target, string body);
        void GetConferenceListAsync();
        void StartPrivate(IEntityIdentifier conferenceUserId);

        event EventHandler<ConferencesListEventArgs> ConferencesListReceived;
        event EventHandler<AuthenticationEventArgs> Authenticated;
        event EventHandler<AuthenticationEventArgs> ConnectionDropped;
        event EventHandler PublicMessage;
    }
}