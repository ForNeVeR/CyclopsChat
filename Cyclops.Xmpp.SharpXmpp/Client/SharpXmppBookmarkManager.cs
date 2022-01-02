using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Data;
using SharpXMPP.XMPP.Client.MUC.Bookmarks;

namespace Cyclops.Xmpp.SharpXmpp.Client;

internal class SharpXmppBookmarkManager : IBookmarkManager
{
    private BookmarksManager? currentBookmarkManager;
    public BookmarksManager BookmarkManager
    {
        set
        {
            if (currentBookmarkManager != null)
                throw new NotSupportedException(
                    $"Reinitialization of {nameof(SharpXmppIqQueryManager)} is not supported.");

            currentBookmarkManager = value;
            SubscribeToEvents(currentBookmarkManager);
        }
    }

    private void SubscribeToEvents(BookmarksManager bookmarkManager)
    {
        bookmarkManager.BookmarksSynced += _ =>
        {
            foreach (var bookmark in bookmarkManager.Rooms)
            {
                BookmarkAdded?.Invoke(this, bookmark.Wrap());
            }
        };
    }

    public event EventHandler<IBookmark>? BookmarkAdded;

    public void RequestBookmarks()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IBookmark> Bookmarks => throw new NotImplementedException();
    public void AddBookmark(Jid roomId, string name, bool autoJoin, string nickname)
    {
        throw new NotImplementedException();
    }

    public void RemoveBookmark(Jid roomId)
    {
        throw new NotImplementedException();
    }
}
