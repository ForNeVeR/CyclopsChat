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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Xml;

using bedrock.util;
using jabber;
using jabber.connection;
using jabber.protocol.client;

namespace muzzle
{
    /// <summary>
    /// A UserControl that references an XmppStream.
    /// </summary>
    [SVN(@"$Id: StreamControl.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class StreamControl : System.Windows.Forms.UserControl
    {
        /// <summary>
        /// The XmppStream for this control.  Set at design time when a subclass control is dragged onto a form.
        /// </summary>
        protected XmppStream m_stream = null;

        /// <summary>
        /// The XmppStream was changed.  Often at design time.  The object will be this StreamControl.
        /// </summary>
        public event bedrock.ObjectHandler OnStreamChanged;

        /// <summary>
        /// The JabberClient or JabberService to hook up to.
        /// </summary>
        [Description("The JabberClient or JabberService to hook up to.")]
        [Category("Jabber")]
        public virtual XmppStream Stream
        {
            get
            {
                // If we are running in the designer, let's try to get an XmppStream control
                // from the environment.
                if ((this.m_stream == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost)base.GetService(typeof(IDesignerHost));
                    this.Stream = StreamComponent.GetStreamFromHost(host);
                }
                return m_stream;
            }
            set
            {
                if ((object)m_stream != (object)value)
                {
                    m_stream = value;
                    if (OnStreamChanged != null)
                        OnStreamChanged(this);
                }
            }
        }

        private JID m_overrideFrom = null;

        /// <summary>
        /// Override the from address that will be stamped on outbound packets.
        /// Unless your server implemets XEP-193, you shouldn't use this for 
        /// client connections.
        /// </summary>
        public JID OverrideFrom
        {
            get { return m_overrideFrom; }
            set { m_overrideFrom = value; }
        }

        /// <summary>
        /// Write the specified stanza to the stream.
        /// </summary>
        /// <param name="elem"></param>
        public void Write(XmlElement elem)
        {
            if ((m_overrideFrom != null) && (elem.GetAttribute("from") == ""))
                elem.SetAttribute("from", m_overrideFrom);
            m_stream.Write(elem);
        }

        ///<summary>
        /// Does an asynchronous IQ call.
        /// If the from address hasn't been set, and an OverrideFrom has been set,
        /// the from address will be set to the value of OverrideFrom.
        ///</summary>
        ///<param name="iq">IQ packet to send.</param>
        ///<param name="cb">Callback to execute when the result comes back.</param>
        ///<param name="cbArg">Arguments to pass to the callback.</param>
        public void BeginIQ(IQ iq, IqCB cb, object cbArg)
        {
            if ((m_overrideFrom != null) && (iq.From == null))
                iq.From = m_overrideFrom;
            m_stream.Tracker.BeginIQ(iq, cb, cbArg);
        }
    }
}
