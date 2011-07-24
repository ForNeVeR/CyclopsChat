using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using Cyclops.Core;

namespace Cyclops.MainApplication.MessageDecoration
{
    public class MessagePresenter
    {
        public static Paragraph Present(IConferenceMessage message)
        {
            if (!message.IsFromHistory)
                ChatObjectFactory.GetChatLogger().AddRecord(message.AuthorId, 
                    string.Format("[{0:s}] {1}: {2}", message.Timestamp, message.AuthorNick, message.Body),
                    message is PrivateMessage);

            var paragraph = new Paragraph();
            //paragraph.Inlines.Add(new Run(string.Format("[{0:s}] {1}: {2}", message.Timestamp, message.AuthorNick, message.Body)));
            //return paragraph;

            paragraph.SetResourceReference(FrameworkContentElement.StyleProperty, "parentRowStyle");

            // at first, textInlines will contain only one inline - raw text
            var textInlines = new List<Inline>();

            // applying custom decorators
            DecoratorsRegistry.Decorators.ForEach(i => textInlines = i.Decorate(message, textInlines));

            paragraph.Inlines.AddRange(textInlines);

            return paragraph;
        }
    }
}