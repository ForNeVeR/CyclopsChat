using System.Xml.Linq;
using Cyclops.Core;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Data;
using SharpXMPP;
using SharpXMPP.XMPP;
using SharpXMPP.XMPP.Client.Elements;
using SharpXMPP.XMPP.Client.MUC.Bookmarks.Elements;
using Namespaces = Cyclops.Xmpp.Protocol.Namespaces;

namespace Cyclops.Xmpp.SharpXmpp.Client;

internal class SharpXmppBookmarkManager : IBookmarkManager
{
    private readonly ILogger logger;

    private readonly object bookmarksLock = new();
    private readonly List<BookmarkedConference> currentBookmarks = new();

    public SharpXmppBookmarkManager(ILogger logger)
    {
        this.logger = logger;
    }

    public XmppConnection? Connection { get; set; }

    public event EventHandler<IBookmark>? BookmarkAdded;

    public void RequestBookmarks()
    {
        var iq = new XMPPIq(XMPPIq.IqTypes.get);
        var storageQuery = new XElement(
            XNamespace.Get(Namespaces.Private) + Elements.Query,
            new XElement(XNamespace.Get(SharpXMPP.Namespaces.StorageBookmarks) + Elements.Storage));
        iq.Add(storageQuery);

        Connection!.Query(iq, response =>
        {
            try
            {
                lock (bookmarksLock)
                {
                    currentBookmarks.Clear();

                    var query = response.Element(XNamespace.Get(Namespaces.Private) + Elements.Query);
                    if (query == null)
                        throw new NotSupportedException("Cannot find the query element in the bookmark response.");
                    var storage =
                        query.Element(XNamespace.Get(SharpXMPP.Namespaces.StorageBookmarks) + Elements.Storage);
                    if (storage == null)
                        throw new NotSupportedException("Cannot find the storage element in the bookmark response.");
                    var conferences = storage.Elements(
                        XNamespace.Get(SharpXMPP.Namespaces.StorageBookmarks) + Elements.Conference);

                    foreach (var conference in conferences)
                    {
                        var bookmarkedConference = Stanza.Parse<BookmarkedConference>(conference);
                        currentBookmarks.Add(bookmarkedConference);
                    }
                }

                NotifyAllBookmarksAdded();
            }
            catch (Exception ex)
            {
                logger.LogError("Error during bookmark processing.", ex);
            }
        });
    }

    public IEnumerable<IBookmark> Bookmarks
    {
        get
        {
            lock (bookmarksLock)
                return currentBookmarks.Select(bc => bc.Wrap()).ToList();
        }
    }

    public void AddBookmark(Jid roomId, string name, bool autoJoin, string nickname)
    {
        throw new NotImplementedException();
    }

    public void RemoveBookmark(Jid roomId)
    {
        throw new NotImplementedException();
    }

    private void NotifyAllBookmarksAdded()
    {
        lock (currentBookmarks)
        {
            foreach (var bookmark in currentBookmarks)
                BookmarkAdded?.Invoke(this, bookmark.Wrap());
        }
    }
}
