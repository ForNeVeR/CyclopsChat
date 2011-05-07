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

            if (msg.Body != null && msg.Body.StartsWith("/me", System.StringComparison.InvariantCultureIgnoreCase))
            {
                style = DecoratorsStyles.MeCommandNickStyle;
                message = msg.Body.Remove(0, 3);
            }

            var messageInline = new RunEx(message, MessagePartType.Body);
            messageInline.SetResourceReference(FrameworkContentElement.StyleProperty, style);

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

        #endregion
    }
}