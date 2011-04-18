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
using jabber;
using jabber.protocol;
using jabber.protocol.iq;

namespace test.jabber.protocol.iq
{
    /// <summary>
    /// Summary description for RosterTest.
    /// </summary>
    [SVN(@"$Id: RegisterTest.cs 724 2008-08-06 18:09:25Z hildjj $")]
    [TestFixture]
    public class RegisterTest
    {
        XmlDocument doc = new XmlDocument();
        [SetUp]
        public void SetUp()
        {
            Element.ResetID();
        }
        [Test] public void Test_Create()
        {
            Register r = new Register(doc);
            Assert.AreEqual("<query xmlns=\"jabber:iq:register\" />", r.ToString());
        }
        [Test] public void Test_Registered()
        {
            Register r = new Register(doc);
            r.Registered = true;
            Assert.AreEqual("<query xmlns=\"jabber:iq:register\"><registered /></query>", r.ToString());
            Assert.IsTrue(r.Registered);
        }
    }
}
