using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Cyclops.Core;
using Cyclops.MainApplication.Localization;

namespace Cyclops.MainApplication.MessageDecoration.Decorators;

public class HyperlinkDecorator : IMessageDecorator
{
    public event EventHandler<UrlEventArgs> UrlRightMouseClick = delegate { };

    /// <summary>
    /// Transform collection of inlines
    /// </summary>
    public List<Inline> Decorate(IConferenceMessage msg, List<Inline> inlines)
    {
        const string pattern = "(http|https|ftp|dchub|mailto|xmpp)://([a-zA-Zа-яА-Я0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?";

        for (int i = 0; i < inlines.Count; i++)
        {
            // splite one Inline element (text (Run)) into several inlines (Runs and Hyperlinks)
            if (inlines[i] is not Run inline)
                continue;

            string[] matches = Regex.Matches(inline.Text, pattern).OfType<Match>().Select(item => item.Value).ToArray();
            if (matches.Length < 1)
                continue;

            inlines.RemoveAt(i);

            string[] parts = inline.Text.SplitAndIncludeDelimiters(matches).Select(p => p.String).ToArray();
            for (int j = i; j < parts.Length + i; j++)
            {
                string part = parts[j - i];
                if (matches.Contains(part) && IsWellFormedUriString(part))
                    inlines.Insert(j, DecorateAsHyperlink(part));
                else
                    inlines.Insert(j, CommonMessageDecorator.Decorate(msg, part));
            }
        }
        return inlines;
    }

    private static bool IsWellFormedUriString(string uri)
    {
        try
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }
        catch
        {
            return false;
        }
    }

    public Inline DecorateAsHyperlink(string text)
    {
        var hyperlink = new Hyperlink(new Run(text)) {NavigateUri = new Uri(text)};
        hyperlink.Click += HyperlinkClickHandler;
        hyperlink.MouseRightButtonUp += HyperlinkMouseRightButtonUp;
        hyperlink.SetResourceReference(FrameworkContentElement.StyleProperty, DecoratorsStyles.HyperlinkStyle);
        return hyperlink;
    }

    private void HyperlinkMouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is not Hyperlink hyperlink || hyperlink.NavigateUri == null || string.IsNullOrEmpty(hyperlink.NavigateUri.ToString()))
            return;

        hyperlink.ContextMenu = GenerateContextMenu(hyperlink.NavigateUri.ToString());
        UrlRightMouseClick(this, new UrlEventArgs(hyperlink.NavigateUri));
    }

    /// <summary>
    /// TODO: move to presentation layer
    /// </summary>
    private ContextMenu GenerateContextMenu(string uri)
    {
        ContextMenu menu = new ContextMenu();

        MenuItem copyItem = new MenuItem {Header = Conference.HyperlinkMenuCopy};
        copyItem.Click += (s, e) => Clipboard.SetText(uri);

        MenuItem browseItem = new MenuItem { Header = Conference.HyperlinkMenuOpen };
        browseItem.Click += (s, e) => Process.Start(uri);

        MenuItem browseInIeItem = new MenuItem { Header = Conference.HyperlinkMenuOpenInIe };
        browseInIeItem.Click += (s, e) => Process.Start("IEXPLORE.EXE", uri);

        menu.Items.Add(copyItem);
        menu.Items.Add(browseItem);
        menu.Items.Add(browseInIeItem);

        return menu;
    }

    private static void HyperlinkClickHandler(object sender, RoutedEventArgs e)
    {
        var uri = ((Hyperlink)sender).NavigateUri;
        var startInfo = new ProcessStartInfo(uri.ToString())
        {
            UseShellExecute = true
        };
        Process.Start(startInfo);
    }
}
