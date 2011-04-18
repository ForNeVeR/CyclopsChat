using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
            string style = "commonMessageStyle";
            if (msg is SystemConferenceMessage)
                style = "systemMessageStyle";
            if (msg is SystemConferenceMessage && ((SystemConferenceMessage) msg).IsErrorMessage)
                style = "errorMessageStyle";


            var messageInline = new RunEx(message, MessagePartType.Body);
            messageInline.SetResourceReference(FrameworkContentElement.StyleProperty, style);

            if (msg is CaptchaSystemMessage)
            {
                var web = new WebBrowser();
                web.Navigate(((CaptchaSystemMessage) msg).Url);
                web.MaxHeight = 150;
                web.MaxWidth = 230;
                Span span = new Span();
                span.Inlines.Add(messageInline);
                span.Inlines.Add(new Figure(new BlockUIContainer(web)));
                return span;
            }

            return messageInline;
        }

        #endregion
    }
}