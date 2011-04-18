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
#if !NO_STRINGPREP

using System;
using NUnit.Framework;
using stringprep;
using stringprep.steps;
using bedrock.util;

namespace test.stringprep
{
    [SVN(@"$Id: TestResourceprep.cs 724 2008-08-06 18:09:25Z hildjj $")]
    [TestFixture]
    public class TestResourceprep
    {
        private static System.Text.Encoding ENC = System.Text.Encoding.UTF8;

        private Profile resourceprep = new XmppResource();

        private void TryOne(string input, string expected)
        {
            string output = resourceprep.Prepare(input);
            Assert.AreEqual(expected, output);
        }

        [Test] public void Test_Good()
        {
            TryOne("Test", "Test");
            TryOne("test", "test");
        }

        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_Bad()
        {
            TryOne("Test\x180E", null);
        }
    }
}
#endif
