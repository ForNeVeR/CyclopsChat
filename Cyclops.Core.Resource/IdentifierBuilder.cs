using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jabber;

namespace Cyclops.Core.Resource
{
    public static class IdentifierBuilder
    {
        public static IEntityIdentifier Create(string user, string server, string resource)
        {
            return new JID(user, server, resource);
        }

        public static IEntityIdentifier Create(string str)
        {
            return new JID(str);
        }
    }
}
