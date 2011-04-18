namespace Cyclops.Core.CustomEventArgs
{
    /// <summary>
    /// Authentication event arguments
    /// </summary>
    public class AuthenticationEventArgs : OperationResult<ConnectionErrorKind>
    {
        public AuthenticationEventArgs()
        {
        }

        public AuthenticationEventArgs(ConnectionErrorKind errorKind, string errorMessage) :
            base(errorKind, errorMessage)
        {
        }
    }
}