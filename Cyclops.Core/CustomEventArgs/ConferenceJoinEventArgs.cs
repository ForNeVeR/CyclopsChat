using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.CustomEventArgs
{
    /// <summary>
    /// Disconnect event args
    /// </summary>
    public class ConferenceJoinEventArgs : OperationResult<ConferenceJoinErrorKind>
    {
        public IPresence? Presence { get; }

        public ConferenceJoinEventArgs()
        { }

        public ConferenceJoinEventArgs(
            ConferenceJoinErrorKind errorKind,
            string errorMessage,
            IPresence? presence = null)
            : base(errorKind, errorMessage)
            => Presence = presence;
    }
}
