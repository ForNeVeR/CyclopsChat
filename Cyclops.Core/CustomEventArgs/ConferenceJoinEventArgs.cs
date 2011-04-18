namespace Cyclops.Core.CustomEventArgs
{
    /// <summary>
    /// Disconnect event args
    /// </summary>
    public class ConferenceJoinEventArgs : OperationResult<ConferenceJoinErrorKind>
    {
        public ConferenceJoinEventArgs()
        {
        }

        public ConferenceJoinEventArgs(ConferenceJoinErrorKind errorKind, string errorMessage) :
            base(errorKind, errorMessage)
        {
        }
    }
}