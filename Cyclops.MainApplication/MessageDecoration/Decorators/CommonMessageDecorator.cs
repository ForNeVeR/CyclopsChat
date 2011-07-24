using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using Cyclops.Core;

namespace Cyclops.MainApplication.MessageDecoration.Decorators
{
    public class CommonMessageDecorator : IMessageDecorator
    {
        #region Implementation of IMessageDecorator

        /// <summary>
        /// Transform collection of inlines
        /// </summary>
        public List<Inline> Decorate(IConferenceMessage msg, List<Inline> inlines)
        {
            inlines.Add(Decorate(msg, msg.Body));
            return inlines;
        }

        public static Inline Decorate(IConferenceMessage msg, string message)
        {
            string style = DecoratorsStyles.CommonMessageStyle;
            if (msg is SystemConferenceMessage)
                style = DecoratorsStyles.SystemMessageStyle;
            if (msg is SystemConferenceMessage && ((SystemConferenceMessage) msg).IsErrorMessage)
                style = DecoratorsStyles.ErrorMessageStyle;

            //TODO: needed refactor

            if (IsPublicMessageToMe(msg))
            {
                string nick = msg.Conference.ConferenceId.Resource + ":";
                var nickInline = new RunEx(nick, MessagePartType.Nick);
                nickInline.SetResourceReference(FrameworkContentElement.StyleProperty, DecoratorsStyles.PublicMessageToMeStyle);

                Span span = new Span();
                span.Inlines.Add(nickInline);
                span.Inlines.Add(CreateMessageInline(message.Remove(0, nick.Length), style));

                return span;
            }
            
            if (msg.Body != null && msg.Body.StartsWith("/me", System.StringComparison.InvariantCultureIgnoreCase))
            {
                style = DecoratorsStyles.MeCommandNickStyle;
                message = msg.Body.Remove(0, 3);
            }

            var messageInline = CreateMessageInline(message, style);

            if (msg is CaptchaSystemMessage)
            {
                var imageControl = new Image();
                BitmapImage image;
                imageControl.Source = image =(((CaptchaSystemMessage)msg).Bitmap);
                imageControl.Width = image.Width;
                imageControl.Height = image.Height;
                Span span = new Span();
                span.Inlines.Add(messageInline);
                span.Inlines.Add(imageControl);
                return span;
            }

            return messageInline;
        }

        private static Inline CreateMessageInline(string message, string style)
        {
            var messageInline = new RunEx(message, MessagePartType.Body);
            messageInline.SetResourceReference(FrameworkContentElement.StyleProperty, style);
            return messageInline;
        }

        private static bool IsPublicMessageToMe(IConferenceMessage msg)
        {
            if (msg == null || msg.Conference == null || msg.Conference.ConferenceId == null || msg.Body == null || string.IsNullOrEmpty(msg.Conference.ConferenceId.Resource))
                return false;
            return msg.Body.StartsWith(msg.Conference.ConferenceId.Resource + ":");
        }

        #endregion
    }
}