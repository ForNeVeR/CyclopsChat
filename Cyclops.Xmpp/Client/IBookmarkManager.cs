using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Client;

public interface IBookmarkManager
{
    event EventHandler<IBookmark> BookmarkAdded;

    /// <summary>Sends a bookmark request, and fires BookmarkAdded event on each bookmark received.</summary>
    void RequestBookmarks();

    IEnumerable<IBookmark> Bookmarks { get; }

    void AddBookmark(Jid roomId, string name, bool autoJoin, string nickname);
    void RemoveBookmark(Jid roomId);
}
