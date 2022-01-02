using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Protocol;
using SharpXMPP.XMPP.Client.MUC.Bookmarks.Elements;

namespace Cyclops.Xmpp.SharpXmpp.Data;

internal static class BookmarkedConferenceEx
{
    private class Bookmark : IBookmark
    {
        private readonly BookmarkedConference bookmarkedConference;
        public Bookmark(BookmarkedConference bookmarkedConference)
        {
            this.bookmarkedConference = bookmarkedConference;
        }

        public Jid ConferenceJid => bookmarkedConference.JID.Map();
        public string Nick => bookmarkedConference.Nick;
        public string Name => bookmarkedConference.Name;
        public bool AutoJoin => bookmarkedConference.IsAutojoin;
    }

    public static IBookmark Wrap(this BookmarkedConference bookmarkedConference) => new Bookmark(bookmarkedConference);
}
