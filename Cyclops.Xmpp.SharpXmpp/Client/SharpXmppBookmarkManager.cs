using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Client;

public class SharpXmppBookmarkManager : IBookmarkManager
{
    public event EventHandler<IBookmark>? BookmarkAdded
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }

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
