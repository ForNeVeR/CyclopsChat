using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.Core.CustomEventArgs
{
    public class CaptchaEventArgs : EventArgs
    {
        public string CaptchaUrl { get; private set; }

        public CaptchaEventArgs(string url)
        {
            CaptchaUrl = url;
        }
    }
}
