using System;
using System.Collections.Generic;
using Cyclops.MainApplication.MessageDecoration.Decorators;

namespace Cyclops.MainApplication.MessageDecoration
{
    public static class DecoratorsRegistry
    {
        private static readonly NickDecorator NickDecorator = new NickDecorator();
        static DecoratorsRegistry()
        {
            Decorators = new IMessageDecorator[]
                       {
                           //Order is important!
                           new CommonMessageDecorator(),
                           new HyperlinkDecorator(),
                           NickDecorator,
                           new TimestampDecorator(),
                           new SmilesDecorator(),
                       };
        }

        public static IEnumerable<IMessageDecorator> Decorators { get; private set; }

        public static event EventHandler<StringEventArgs> NickClick
        {
            add { NickDecorator.NickClick += value; }
            remove { NickDecorator.NickClick -= value; }
        }
    }
}