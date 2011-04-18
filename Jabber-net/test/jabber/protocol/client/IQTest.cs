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

using System.Xml;
using NUnit.Framework;

using bedrock.util;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace test.jabber.protocol.client
{
    /// <summary>
    /// Summary description for IQTest.
    /// </summary>
    [SVN(@"$Id: IQTest.cs 724 2008-08-06 18:09:25Z hildjj $")]
    [TestFixture]
    public class IQTest
    {
        XmlDocument doc = new XmlDocument();

        [Test]
        public void Create()
        {
            Element.ResetID();

            IQ iq = new IQ(doc);
            Assert.AreEqual("<iq id=\"JN_1\" type=\"get\" />", iq.ToString());
            iq = new IQ(doc);
            Assert.AreEqual("<iq id=\"JN_2\" type=\"get\" />", iq.ToString());
            iq.Query = new Auth(doc);
            Assert.AreEqual("<iq id=\"JN_2\" type=\"get\"><query xmlns=\"jabber:iq:auth\" /></iq>", iq.ToString());
        }
    }
}
