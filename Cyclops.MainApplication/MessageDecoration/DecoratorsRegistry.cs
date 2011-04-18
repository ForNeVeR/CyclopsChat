using System.Collections.Generic;
using Cyclops.MainApplication.MessageDecoration.Decorators;

namespace Cyclops.MainApplication.MessageDecoration
{
    public static class DecoratorsRegistry
    {
        public static IEnumerable<IMessageDecorator> GetDecorators()
        {
            return new IMessageDecorator[]
                       {
                           //Order is important!
                           new CommonMessageDecorator(),
                           new HyperlinkDecorator(),
                           new NickDecorator(),
                           new TimestampDecorator(),
                           new SmilesDecorator(),
                       };
        }
    }
}