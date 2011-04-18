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

using bedrock.util;

namespace jabber.protocol.client
{
    /// <summary>
    /// IQ type attribute
    /// </summary>
    [SVN(@"$Id: IQ.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public enum IQType
    {
        /// <summary>
        /// type='get'
        /// </summary>
        get,
        /// <summary>
        /// type='set'
        /// </summary>
        set,
        /// <summary>
        /// type='result'
        /// </summary>
        result,
        /// <summary>
        /// type='error'
        /// </summary>
        error
    }

    /// <summary>
    /// All IQ packets start here.  The Query property holds the interesting part.
    /// There should usually be a convenience class next to the Query type, which
    /// creates an IQ with the appropriate type of query inside.
    /// </summary>
    [SVN(@"$Id: IQ.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class IQ : Packet
    {
        private bool m_handled = false;

        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public IQ(XmlDocument doc) : base("iq", doc)
        {
            ID   = NextID();
            Type = IQType.get;  // get better errors than when there is no type specified.
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public IQ(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(qname.Name, doc) // Note:  *NOT* base(prefix, qname, doc), so that xpath matches are easier
        {
        }

        /// <summary>
        /// Has this IQ been handled?  Set automatically by GetResponse and GetErrorResponse.  If this is not
        /// set to true, Jabber-Net will respond automatically with a 501 error.
        /// </summary>
        public bool Handled
        {
            get { return m_handled; }
            set { m_handled = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public IQType Type
        {
            get { return GetEnumAttr<IQType>("type"); }
            set 
            {
                IQType cur = this.Type;
                if (cur == value)
                    return;

                if (value == IQType.error)
                {
                    this.InnerXml = "";
                    this.GetOrCreateElement<Error>();
                }
                SetEnumAttr("type", value);
            }
        }

        /// <summary>
        /// IQ error.
        /// </summary>
        public Error Error
        {
            get { return GetChildElement<Error>(); }
            set
            {
                this.Type = IQType.error;
                ReplaceChild<Error>(value);
            }
        }

        /// <summary>
        /// The query tag inside, regardless of namespace.
        /// If the iq contains something other than query,
        /// use normal XmlElement routines.
        /// </summary>
        public XmlElement Query
        {
            get { return this.GetFirstChildElement(); }
            set { this.InnerXml = ""; this.AddChild(value); }
        }

#if __MonoCS__
#pragma warning disable 0809
#endif
        /// <summary>
        /// Swap the To and the From addresses.
        /// Obsolete: Use GetResponse or GetErrorResponse, now, for IQs.
        /// </summary>
        [Obsolete("Use GetResponse or GetErrorResponse, now.")]
        public override void Swap()
        {
            base.Swap();
        }
#if __MonoCS__
#pragma warning restore 0809
#endif

        /// <summary>
        /// Swap the to and from, set the type to result.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public IQ GetResponse(XmlDocument doc)
        {
            IQ resp = new IQ(doc);
            resp.From = this.To;
            resp.To = this.From;
            resp.ID = this.ID;
            resp.Type = IQType.result;

            XmlElement q = this.Query;
            if (q != null)
            {
                if (q is Element)
                    resp.AppendChild((XmlElement)((Element)q).CloneNode(true, doc));
                else
                    resp.AppendChild(doc.ImportNode(q, true));
            }

            this.Handled = true;
            return resp;
        }

        /// <summary>
        /// Respond to this IQ with an error.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IQ GetErrorResponse(XmlDocument doc, string condition)
        {
            IQ resp = new IQError(doc, condition);
            resp.From = this.To;
            resp.To = this.From;
            resp.ID = this.ID;
            resp.Type = IQType.error;

            XmlElement q = this.Query;
            if (q != null)
            {
                if (q is Element)
                    resp.AppendChild((XmlElement)((Element)q).CloneNode(true, doc));
                else
                    resp.AppendChild(doc.ImportNode(q, true));
            }

            this.Handled = true;
            return resp;
        }
    }

    /// <summary>
    /// An IQ subclass that allows typed access to its first child,
    /// through the Instruction property.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [SVN(@"$Id: IQ.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class TypedIQ<T> : IQ
        where T : Element
    {
        /// <summary>
        /// Create an IQ to send out, with an instance of the specified
        /// type as a child.
        /// </summary>
        /// <param name="doc"></param>
        public TypedIQ(XmlDocument doc) : base(doc)
        {
            CreateChildElement<T>();
        }

        /// <summary>
        /// The child element (often "query") with the command for this IQ.
        /// </summary>
        public T Instruction
        {
            get { return GetChildElement<T>(); }
            set { ReplaceChild<T>(value); }
        }
    }

}
