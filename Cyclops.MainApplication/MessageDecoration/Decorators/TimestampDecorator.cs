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
            string style = "timestampStyle";
            var timestampInline = new RunEx(msg.Timestamp.ToString("[hh:mm:ss] "), MessagePartType.Timestamp);
            timestampInline.SetResourceReference(FrameworkContentElement.StyleProperty, style);

            inlines.Insert(0, timestampInline);
            return inlines;
        }

        #endregion
    }
}