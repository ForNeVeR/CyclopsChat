using System;
using System.Collections.Generic;
using Cyclops.MainApplication.MessageDecoration.Decorators;

namespace Cyclops.MainApplication.MessageDecoration
{
    public static class DecoratorsRegistry
    {
        private static readonly NickDecorator NickDecorator = new NickDecorator();
        private static readonly HyperlinkDecorator HyperlinkDecorator = new HyperlinkDecorator();

        static DecoratorsRegistry()
        {
            Decorators = new IMessageDecorator[]
                       {
                           //Order is important!
                           new CommonMessageDecorator(),
                           HyperlinkDecorator,
                           NickDecorator,
                           new TimestampDecorator(),
                           new SmilesDecorator(),
                       };
        }

        public static IEnumerable<IMessageDecorator> Decorators { get; private set; }

        public static event EventHandler<NickEventArgs> NickClick
        {
            add { NickDecorator.NickClick += value; }
            remove { NickDecorator.NickClick -= value; }
        }

        public static event EventHandler<UrlEventArgs> HyperlinkRightMouseClick
        {
            add { HyperlinkDecorator.UrlRightMouseClick += value; }
            remove { HyperlinkDecorator.UrlRightMouseClick -= value; }
        }
    }
}