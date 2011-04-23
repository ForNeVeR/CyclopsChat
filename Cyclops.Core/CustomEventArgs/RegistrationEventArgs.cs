using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.Core.CustomEventArgs
{
    public class RegistrationEventArgs : EventArgs
    {
        public RegistrationResult Result { get; set; }
        public string ErrorMessage { get; set; }

        public RegistrationEventArgs()
        {
            Result = RegistrationResult.Success;
        }

        public RegistrationEventArgs(RegistrationResult result, string errorMessage)
        {
            Result = result;
            ErrorMessage = errorMessage;
        }
    }
}
