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
            var paragraph = new Paragraph();
            paragraph.SetResourceReference(FrameworkContentElement.StyleProperty, "parentRowStyle");

            // at first, textInlines will contains only one inline - raw text
            var textInlines = new List<Inline>();

            // applying custom decorators
            DecoratorsRegistry.GetDecorators().ForEach(i => textInlines = i.Decorate(message, textInlines));

            paragraph.Inlines.AddRange(textInlines);

            return paragraph;
        }
    }
}