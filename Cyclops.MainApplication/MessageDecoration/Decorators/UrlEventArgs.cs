using System;

namespace Cyclops.MainApplication.MessageDecoration.Decorators
{
    public class UrlEventArgs : EventArgs
    {
        public Uri Uri { get; private set; }

        public UrlEventArgs(Uri uri)
        {
            Uri = uri;
        }
    }
}