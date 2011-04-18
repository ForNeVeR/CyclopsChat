/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2008 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net is licensed under the LGPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using bedrock.util;
using jabber.protocol;

namespace jabber.protocol.client
{
    /// <summary>
    /// ElementFactory for the jabber:client namespace.
    /// </summary>
    [SVN(@"$Id: Factory.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Factory : IPacketTypes
    {
        private static QnameType[] s_qnt = new QnameType[]
        {
            new QnameType("presence", URI.CLIENT, typeof(jabber.protocol.client.Presence)),
            new QnameType("message",  URI.CLIENT, typeof(jabber.protocol.client.Message)),
            new QnameType("iq",       URI.CLIENT, typeof(jabber.protocol.client.IQ)),
            new QnameType("error",    URI.CLIENT, typeof(jabber.protocol.client.Error)),
            // meh.  jabber protocol really isn't right WRT to namespaces.
            new QnameType("presence", URI.ACCEPT, typeof(jabber.protocol.client.Presence)),
            new QnameType("message",  URI.ACCEPT, typeof(jabber.protocol.client.Message)),
            new QnameType("iq",       URI.ACCEPT, typeof(jabber.protocol.client.IQ)),
            new QnameType("error",    URI.ACCEPT, typeof(jabber.protocol.client.Error))
        };
        QnameType[] IPacketTypes.Types { get { return s_qnt; } }
    }
}
