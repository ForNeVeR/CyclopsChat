using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.Core.CustomEventArgs
{
    public class RegistrationEventArgs : EventArgs
    {
        //public RegistrationResult Result { get; set; }
        public string ErrorMessage { get; set; }

        public bool HasError { get { return !string.IsNullOrEmpty(ErrorMessage); } }

        public RegistrationEventArgs()
        {
            //Result = RegistrationResult.Success;
        }

        public RegistrationEventArgs(string errorMessage)
        {
            //Result = result;
            ErrorMessage = errorMessage;
        }
    }
}
