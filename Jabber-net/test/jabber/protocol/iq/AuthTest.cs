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

namespace test.jabber.protocol.iq
{
    /// <summary>
    /// Summary description for AuthTest.
    /// </summary>
    [SVN(@"$Id: AuthTest.cs 724 2008-08-06 18:09:25Z hildjj $")]
    [TestFixture]
    public class AuthTest
    {
        XmlDocument doc = new XmlDocument();
        [SetUp]
        public void SetUp()
        {
            Element.ResetID();
        }
        [Test] public void Test_Create()
        {
            IQ iq = new AuthIQ(doc);
            Assert.AreEqual("<iq id=\""+iq.ID+"\" type=\"get\"><query xmlns=\"jabber:iq:auth\" /></iq>", iq.ToString());
        }
        [Test] public void Test_Hash()
        {
            IQ iq = new AuthIQ(doc);
            iq.Type = IQType.set;
            Auth a = (Auth) iq.Query;
            a.SetDigest("foo", "bar", "3B513636");
            a.Resource = "Home";
            Assert.AreEqual("<iq id=\""+iq.ID+"\" type=\"set\"><query xmlns=\"jabber:iq:auth\">" +
                "<username>foo</username>" +
                "<digest>37d9c887967a35d53b81f07916a309a5b8d7e8cc</digest>" +
                "<resource>Home</resource>" +
                "</query></iq>",
                iq.ToString());
        }
        /*
        SENT: <iq type="get" id="JCOM_14"><query xmlns="jabber:iq:auth"><username>zeroktest</username></query></iq>
        RECV: <iq id='JCOM_14' type='result'><query xmlns='jabber:iq:auth'><username>zeroktest</username><password/><digest/><sequence>499</sequence><token>3C7A6B0A</token><resource/></query></iq>
        SENT: <iq type="set" id="JCOM_15"><query xmlns="jabber:iq:auth"><username>zeroktest</username><hash>e00c83748492a3bc7e4831c9e973d117082c3abe</hash><resource>Winjab</resource></query></iq>
        */
        [Test] public void Test_ZeroK()
        {
            IQ iq = new AuthIQ(doc);
            iq.Type = IQType.set;
            Auth a = (Auth) iq.Query;
            a.SetZeroK("zeroktest", "test", "3C7A6B0A", 499);
            a.Resource = "Winjab";
            Assert.AreEqual("<iq id=\""+iq.ID+"\" type=\"set\"><query xmlns=\"jabber:iq:auth\">" +
                "<username>zeroktest</username>" +
                "<hash>e00c83748492a3bc7e4831c9e973d117082c3abe</hash>" +
                "<resource>Winjab</resource>" +
                "</query></iq>",
                iq.ToString());
        }
    }
}
