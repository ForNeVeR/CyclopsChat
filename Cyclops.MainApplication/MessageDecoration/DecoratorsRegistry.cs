using System;
using System.Collections.Generic;
using Cyclops.MainApplication.MessageDecoration.Decorators;

namespace Cyclops.MainApplication.MessageDecoration
{
    public static class DecoratorsRegistry
    {
        private static readonly NickDecorator nickDecorator = new NickDecorator();
        static DecoratorsRegistry()
        {
            Decorators = new IMessageDecorator[]
                       {
                           //Order is important!
                           new CommonMessageDecorator(),
                           new HyperlinkDecorator(),
                           nickDecorator,
                           new TimestampDecorator(),
                           new SmilesDecorator(),
                       };
        }

        public static IEnumerable<IMessageDecorator> Decorators { get; private set; }

        public static event EventHandler<StringEventArgs> NickClick
        {
            add { nickDecorator.NickClick += value; }
            remove { nickDecorator.NickClick -= value; }
        }
    }
}