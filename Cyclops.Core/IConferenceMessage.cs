using System;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core
{
    public interface IConferenceMessage : ISessionHolder
    {
        Jid AuthorId { get; }
        bool IsAuthorModer { get; }
        string AuthorNick { get; }
        string Body { get; }
        DateTime Timestamp { get; }
        bool IsCustom { get; }
        bool IsSelfMessage { get; }
        bool IsFromHistory { get; }

        IConference Conference { get; }
    }
}
