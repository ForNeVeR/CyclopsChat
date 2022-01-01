using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;
using jabber.protocol.iq;

namespace Cyclops.Xmpp.JabberNet.Data;

internal static class BookmarkConferenceEx
{
    private class Bookmark : IBookmark
    {
        private readonly BookmarkConference bookmarkConference;
        public Bookmark(BookmarkConference bookmarkConference)
        {
            this.bookmarkConference = bookmarkConference;
        }

        public Jid ConferenceJid => bookmarkConference.JID.Map();
        public string Nick => bookmarkConference.Nick;
        public string Name => bookmarkConference.Name;
        public bool AutoJoin => bookmarkConference.AutoJoin;
    }

    public static IBookmark Wrap(this BookmarkConference bookmarkConference) => new Bookmark(bookmarkConference);
}
