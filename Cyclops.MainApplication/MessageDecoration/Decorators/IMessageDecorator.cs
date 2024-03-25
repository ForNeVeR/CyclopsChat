using System.Collections.Generic;
using System.Windows.Documents;
using Cyclops.Core;

namespace Cyclops.MainApplication.MessageDecoration.Decorators
{
    public interface IMessageDecorator
    {
        /// <summary>
        /// Transform collection of inlines
        /// </summary>
        List<Inline> Decorate(IConferenceMessage msg, List<Inline> inlines);
    }
}