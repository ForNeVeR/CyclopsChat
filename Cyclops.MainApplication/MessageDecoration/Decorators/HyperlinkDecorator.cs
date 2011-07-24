using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using Cyclops.Core;

namespace Cyclops.MainApplication.MessageDecoration.Decorators
{
    public class HyperlinkDecorator : IMessageDecorator
    {
        #region Implementation of IMessageDecorator

        /// <summary>
        /// Transform collection of inlines
        /// </summary>
        public List<Inline> Decorate(IConferenceMessage msg, List<Inline> inlines)
        {
            const string pattern =
                @"((https?|ftp|dchub|magnet|mailto|gopher|telnet|xmpp|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)";

            for (int i = 0; i < inlines.Count; i++)
            {
                // splite one Inline element (text (Run)) into several inlines (Runs and Hyperlinks)
                var inline = inlines[i] as Run;
                if (inline == null)
                    continue;

                string[] matches = Regex.Matches(inline.Text, pattern).OfType<Match>().Select(item => item.Value).ToArray();
                if (matches.Length < 1)
                    continue;

                inlines.RemoveAt(i);

                string[] parts = inline.Text.SplitAndIncludeDelimiters(matches).Select(p => p.String).ToArray();
                for (int j = i; j < parts.Length + i; j++)
                {
                    string part = parts[j - i];
                    if (matches.Contains(part) && Uri.IsWellFormedUriString(part, UriKind.RelativeOrAbsolute))
                        inlines.Insert(j, DecorateAsHyperlink(part));
                    else
                        inlines.Insert(j, CommonMessageDecorator.Decorate(msg, part));
                }
            }
            return inlines;
        }

        public static Inline DecorateAsHyperlink(string text)
        {
            var hyperlink = new Hyperlink(new Run(text)) {NavigateUri = new Uri(text)};
            hyperlink.Click += HyperlinkClickHandler;
            hyperlink.SetResourceReference(FrameworkContentElement.StyleProperty, DecoratorsStyles.HyperlinkStyle);
            return hyperlink;
        }

        private static void HyperlinkClickHandler(object sender, RoutedEventArgs e)
        {
            Process.Start(((Hyperlink) sender).NavigateUri.ToString());
        }

        #endregion
    }
}