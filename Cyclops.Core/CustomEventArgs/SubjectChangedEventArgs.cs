using System;

namespace Cyclops.Core.CustomEventArgs
{
    public class SubjectChangedEventArgs : EventArgs
    {
        public string Author { get; private set; }
        public string NewSubject { get; private set; }

        public SubjectChangedEventArgs(string author, string subject)
        {
            Author = author;
            NewSubject = subject;
        }
    }
}