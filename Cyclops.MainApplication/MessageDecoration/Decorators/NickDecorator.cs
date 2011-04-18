using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using Cyclops.Core;

namespace Cyclops.MainApplication.MessageDecoration.Decorators
{
    public class NickDecorator : IMessageDecorator
    {
        #region Implementation of IMessageDecorator

        /// <summary>
        /// Transform collection of inlines
        /// </summary>
        public List<Inline> Decorate(IConferenceMessage msg, List<Inline> inlines)
        {
            string style = "nickStyle";
            string nick = msg.AuthorNick;
            if (msg is SystemConferenceMessage)
            {
                style = "systemNickStyle";
                nick = msg.AuthorNick ?? "System";
            }

            if (msg.IsSelfMessage)
            {
                style = "myNickStyle";
            }

            if (msg.IsAuthorModer)
            {
                style = "moderNickStyle";
            }

            if (msg is SystemConferenceMessage && !((SystemConferenceMessage)msg).IsErrorMessage)
                return inlines;

            var nickInline = new RunEx(string.Format("{0}: ", nick), MessagePartType.Nick);
            nickInline.SetResourceReference(FrameworkContentElement.StyleProperty, style);

            inlines.Insert(0, nickInline);
            return inlines;
        }

        #endregion
    }
}