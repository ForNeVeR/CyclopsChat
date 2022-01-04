using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Data.Rooms;

public interface IRoom : IDisposable
{
    void Join(string? password = null);
    void Leave(string? reason);

    void SetNickname(string nickname);
    void SetSubject(string subject);

    event EventHandler Joined;
    event EventHandler<IPresence> Left;
    event EventHandler<IMessage> SubjectChange;
    event EventHandler<IPresence> PresenceError;
    event EventHandler<IMessage> SelfMessage;
    event EventHandler<IMessage> AdminMessage;
    event EventHandler<IMessage> RoomMessage;
    event EventHandler<IMucParticipant> ParticipantJoin;
    event EventHandler<IMucParticipant> ParticipantLeave;
    event EventHandler<IMucParticipant> ParticipantPresenceChange;

    Jid BareJid { get; }
    IReadOnlyList<IMucParticipant> Participants { get; }

    Task<IReadOnlyList<IMucParticipant>> GetParticipants(MucAffiliation? participantAffiliation);

    void SendPublicMessage(string body);
}
