using System;

namespace Cyclops.Core.CustomEventArgs
{
    /// <summary>
    /// Generic base type for EventArgs, that represents operation result
    /// </summary>
    /// <typeparam name="TErrorKindEnum">Enum type</typeparam>
    public class OperationResult<TErrorKindEnum> : EventArgs where TErrorKindEnum : struct
        //can't use 'enum' in the constraint :-(
    {
        /// <summary>
        /// </summary>
        public OperationResult()
        {
            ErrorMessage = string.Empty;
            Success = true;
            ErrorKind = default(TErrorKindEnum);
        }

        /// <summary>
        /// </summary>
        public OperationResult(TErrorKindEnum errorKind, string errorMessage)
        {
            Success = false;
            ErrorKind = errorKind;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; protected set; }

        /// <summary>
        /// Operation result, True if success
        /// </summary>
        public bool Success { get; protected set; }

        /// <summary>
        /// Error kind
        /// </summary>
        public TErrorKindEnum ErrorKind { get; protected set; }

        public override string ToString()
        {
            if (Success)
                return string.Format("Success");
            return string.Format("Fail (ErrorKind={0}, ErrorMessage={1}", ErrorKind, ErrorMessage);
        }
    }
}