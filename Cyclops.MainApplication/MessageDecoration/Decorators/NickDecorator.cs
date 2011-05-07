using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using Cyclops.Core;

namespace Cyclops.MainApplication.MessageDecoration.Decorators
{
    public class NickDecorator : IMessageDecorator
    {
        public event EventHandler<StringEventArgs> NickClick = delegate { }; 

        #region Implementation of IMessageDecorator

        /// <summary>
        /// Transform collection of inlines
        /// </summary>
        public List<Inline> Decorate(IConferenceMessage msg, List<Inline> inlines)
        {
            string style = DecoratorsStyles.NickStyle;
            string format = "{0}: ";
            string nick = msg.AuthorNick;
            if (msg is SystemConferenceMessage)
            {
                style = DecoratorsStyles.SystemNickStyle;
                nick = msg.AuthorNick ?? "System";
            }

            if (msg.IsSelfMessage)
                style = DecoratorsStyles.MyNickStyle;

            if (msg.IsAuthorModer)
                style = DecoratorsStyles.ModerNickStyle;

            if (msg.Body != null && msg.Body.StartsWith("/me", System.StringComparison.InvariantCultureIgnoreCase))
            {
                style = DecoratorsStyles.MeCommandNickStyle;
                format = "{0}";
            }

            if (msg is SystemConferenceMessage && !((SystemConferenceMessage)msg).IsErrorMessage)
                return inlines;

            var nickInline = new RunEx(string.Format(format, nick), MessagePartType.Nick);
            nickInline.Tag = msg;
            nickInline.SetResourceReference(FrameworkContentElement.StyleProperty, style);
            nickInline.MouseLeftButtonDown += NickInlineMouseLeftButtonDown;

            inlines.Insert(0, nickInline);
            return inlines;
        }

        private void NickInlineMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var msg = ((IConferenceMessage) ((RunEx) sender).Tag);
            if (msg is SystemConferenceMessage)
                return;
            NickClick(this, new StringEventArgs(msg.AuthorNick));
        }

        #endregion
    }
}