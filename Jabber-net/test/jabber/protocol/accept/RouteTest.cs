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
using jabber.protocol.accept;

namespace test.jabber.protocol.accept
{
    /// <summary>
    /// Summary description for IQTest.
    /// </summary>
    [SVN(@"$Id: RouteTest.cs 724 2008-08-06 18:09:25Z hildjj $")]
    [TestFixture]
    public class RouteTest
    {
        XmlDocument doc = new XmlDocument();
        [Test] public void Test_Create()
        {
            Route r = new Route(doc);
            r.Contents = doc.CreateElement("foo");
            Assert.AreEqual("<route><foo /></route>", r.OuterXml);
            XmlElement foo = r.Contents;
            Assert.AreEqual("<foo />", foo.OuterXml);
        }
    }
}
