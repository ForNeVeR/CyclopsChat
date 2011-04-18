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
using System.Text;
using System.IO;
using System.Threading;

using NUnit.Framework;
using bedrock.util;
using jabber.connection;

namespace test.bedrock.net
{
    /// <summary>
    /// TODO: This test is known to not work.  Add one that does, please.
    /// </summary>
    [TestFixture]
    [SVN(@"$Id: HttpUploadTest.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class HttpUploadTest
    {
        private object m_lock = new object();

        private void uploader_OnUpload(object sender)
        {
            lock(m_lock)
            {
                Monitor.Pulse(m_lock);
            }
        }

        [Test]
        public void UploadFile()
        {
            /*
            m_lock = new object();

            HttpUploader uploader = new HttpUploader();
            uploader.OnUpload += new global::bedrock.ObjectHandler(uploader_OnUpload);
            uploader.Upload("http://opodsadny.kiev.luxoft.com:7335/webclient", "les@primus.com/Bass", "upload.txt");
            lock (m_lock)
            {
                Monitor.Wait(m_lock);
            }
             */
        }
    }
}
