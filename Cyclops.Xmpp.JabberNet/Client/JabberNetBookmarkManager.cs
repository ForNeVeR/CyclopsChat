using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.JabberNet.Data;
using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;
using jabber.client;

namespace Cyclops.Xmpp.JabberNet.Client;

internal class JabberNetBookmarkManager : IBookmarkManager
{
    private readonly BookmarkManager bookmarkManager;
    public JabberNetBookmarkManager(BookmarkManager bookmarkManager)
    {
        this.bookmarkManager = bookmarkManager;
        bookmarkManager.OnConferenceAdd += (_, conference) => BookmarkAdded?.Invoke(this, conference.Wrap());
    }

    public event EventHandler<IBookmark>? BookmarkAdded;
    public void RequestBookmarks() => bookmarkManager.RequestBookmarks();
    public IEnumerable<IBookmark> Bookmarks => bookmarkManager.Bookmarks.Values.Select(b => b.Wrap());

    public void AddBookmark(Jid roomId, string name, bool autoJoin, string nickname) =>
        bookmarkManager.AddConference(roomId.Map(), name, autoJoin, nickname);

    public void RemoveBookmark(Jid roomId) =>
        bookmarkManager[roomId.Map()] = null;
}
