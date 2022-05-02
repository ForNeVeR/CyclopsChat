using System.Xml.Linq;
using Cyclops.Core;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Data;
using Cyclops.Xmpp.SharpXmpp.Protocol;
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
                        logger.LogInfo("Cannot find the query element in the bookmark response.");
                    var storage =
                        query?.Element(XNamespace.Get(SharpXMPP.Namespaces.StorageBookmarks) + Elements.Storage);
                    if (storage == null)
                        logger.LogInfo("Cannot find the storage element in the bookmark response.");
                    else
                    {
                        var conferences = storage.Elements(
                            XNamespace.Get(SharpXMPP.Namespaces.StorageBookmarks) + Elements.Conference);

                        foreach (var conference in conferences)
                        {
                            var bookmarkedConference = Stanza.Parse<BookmarkedConference>(conference);
                            currentBookmarks.Add(bookmarkedConference);
                        }
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
        var bookmark = new BookmarkedConference
        {
            JID = roomId.Map()
        };

        bookmark.SetAttributeValue(Attributes.Name, name);
        bookmark.GetOrCreateChildElement(XNamespace.Get(Namespaces.Bookmarks) + Elements.Nick).Value = nickname;
        bookmark.SetAttributeValue(Attributes.AutoJoin, autoJoin.ToString().ToLowerInvariant());

        List<BookmarkedConference> allBookmarks;
        lock (bookmarksLock)
        {
            currentBookmarks.Add(bookmark);
            allBookmarks = currentBookmarks.ToList();
        }

        StoreBookmarks(allBookmarks);
    }

    public void RemoveBookmark(Jid roomId)
    {
        List<BookmarkedConference> allBookmarks;
        lock (bookmarksLock)
        {
            currentBookmarks.RemoveAll(bc => bc.JID.Map() == roomId.Bare);
            allBookmarks = currentBookmarks.ToList();
        }

        StoreBookmarks(allBookmarks);
    }

    private void NotifyAllBookmarksAdded()
    {
        lock (bookmarksLock)
        {
            foreach (var bookmark in currentBookmarks)
                BookmarkAdded?.Invoke(this, bookmark.Wrap());
        }
    }

    private void StoreBookmarks(IEnumerable<BookmarkedConference> bookmarks)
    {
        var iq = new XMPPIq(XMPPIq.IqTypes.set);

        var storageBookmarks = new XElement(XNamespace.Get(SharpXMPP.Namespaces.StorageBookmarks) + Elements.Storage);
        foreach (var bc in bookmarks)
            storageBookmarks.Add(bc);
        var storageQuery = new XElement(XNamespace.Get(Namespaces.Private) + Elements.Query, storageBookmarks);

        iq.Add(storageQuery);

        Connection!.Send(iq);
    }
}
