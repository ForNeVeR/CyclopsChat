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
using System.Diagnostics;
using System.Threading;
using System.Xml;

using bedrock.net;
using bedrock.util;
using jabber.protocol;
using System.Security.Cryptography;

namespace jabber.connection
{
    /// <summary>
    /// Manages the HTTP Polling XMPP stream.
    /// </summary>
    [SVN(@"$Id: PollingStanzaStream.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class PollingStanzaStream : HttpStanzaStream
    {
        ///<summary>
        /// Creates a PollingStanzaStream
        ///</summary>
        ///<param name="listener">Listener associated with PollingStanzaStream.</param>
        public PollingStanzaStream(IStanzaEventListener listener) : base(listener)
        {
        }

        /// <summary>
        /// Create a XEP25Socket.
        /// </summary>
        /// <returns></returns>
        protected override BaseSocket CreateSocket()
        {
            return new XEP25Socket(this);
        }
    }
}

