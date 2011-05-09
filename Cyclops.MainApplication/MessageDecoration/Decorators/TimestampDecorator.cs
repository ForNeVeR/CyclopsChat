using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using Cyclops.Core;

namespace Cyclops.MainApplication.MessageDecoration.Decorators
{
    public class TimestampDecorator : IMessageDecorator
    {
        #region Implementation of IMessageDecorator

        /// <summary>
        /// Transform collection of inlines
        /// </summary>
        public List<Inline> Decorate(IConferenceMessage msg, List<Inline> inlines)
        {
            string style = DecoratorsStyles.TimestampStyle;
            var timestampInline = new RunEx(msg.Timestamp.ToString("[HH:mm:ss] "), MessagePartType.Timestamp);
            timestampInline.SetResourceReference(FrameworkContentElement.StyleProperty, style);

            inlines.Insert(0, timestampInline);
            return inlines;
        }

        #endregion
    }
}