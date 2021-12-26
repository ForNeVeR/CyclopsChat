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
using System.Text;
using System.Text.RegularExpressions;
using bedrock.util;
using Cyclops.Xmpp.Data;

namespace jabber
{
    /// <summary>
    /// Informs the client that an invalid JID was entered.
    /// </summary>
    [SVN(@"$Id: JID.cs 748 2008-10-28 14:21:07Z hildjj $")]
    public class JIDFormatException : ApplicationException
    {
        /// <summary>
        /// Creates a new exception for an invalid JID.
        /// </summary>
        /// <param name="badJid">The invalid JID</param>
        public JIDFormatException(string badJid) : base("Bad JID: (" + badJid + ")")
        {
        }

        /// <summary>
        /// Creates a new exception instance.
        /// </summary>
        public JIDFormatException() : base()
        {
        }

        /// <summary>
        /// Creates a new exception instance, wrapping another exception.
        /// </summary>
        /// <param name="badJid">Invalid JID.</param>
        /// <param name="e">Inner exception.</param>
        public JIDFormatException(string badJid, Exception e) : base("Bad JID: (" + badJid + ")", e)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AsyncSocketConnectionException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="ctx">The contextual information about the source or destination.</param>
        protected JIDFormatException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext ctx) :
            base(info, ctx)
        {
        }
    }

    /// <summary>
    /// Provides simple JID management.
    /// </summary>
    [SVN(@"$Id: JID.cs 748 2008-10-28 14:21:07Z hildjj $")]
    [System.ComponentModel.TypeConverter(typeof(JIDTypeConverter))]
    public class JID : IComparable, IEntityIdentifier
    {
#if !NO_STRINGPREP
        private static readonly stringprep.Profile s_nodeprep     = new stringprep.XmppNode();
        private static readonly stringprep.Profile s_nameprep     = new stringprep.Nameprep();
        private static readonly stringprep.Profile s_resourceprep = new stringprep.XmppResource();
#endif

        private string m_user     = null;
        private string m_server   = null;
        private string m_resource = null;
        private string m_JID      = null;

        /// <summary>
        /// Creates a JID from a string.
        /// This will parse and perform the stringprep (RFC 3454) process.
        /// </summary>
        /// <param name="jid">Jabber ID, in string form</param>
        public JID(string jid)
        {
            Debug.Assert(jid != null, "jid must be non-null");
            m_JID = jid;
            parse();
        }

        /// <summary>
        /// Builds a new JID from the given components.
        /// This will parse and perform the stringprep (RFC 3454) process.
        /// </summary>
        /// <param name="user">The username value.</param>
        /// <param name="server">The XMPP server domain value.</param>
        /// <param name="resource">The current resource value.</param>
        public JID(string user, string server, string resource)
        {
            Debug.Assert(server != null, "server must be non-null");

#if !NO_STRINGPREP
            m_user     = (user == null) ? null : s_nodeprep.Prepare(user);
            m_server   = s_nameprep.Prepare(server);
            m_resource = (resource == null) ? null : s_resourceprep.Prepare(resource);
#else
            m_user     = (user == null) ? null : user.ToLower();
            m_server   = server.ToLower();
            m_resource = resource;
#endif
            m_JID      = build(m_user, m_server, m_resource);

        }

        /// <summary>
        /// Builds a new JID, from portions that are guaranteed to already be stringprep'd.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="server"></param>
        /// <param name="resource"></param>
        /// <param name="full">The full user@server/resource JID, so that it doesn't have to be recreated from the parts</param>
        private JID(string user, string server, string resource, string full)
        {
            m_user = user;
            m_server = server;
            m_resource = resource;
            m_JID = full;
        }

        private static string build(string user, string server, string resource)
        {
            Debug.Assert(server != null, "Server must be non-null");
            StringBuilder sb = new StringBuilder();
            if (user != null)
            {
                sb.Append(user);
                sb.Append("@");
            }
            sb.Append(server);
            if (resource != null)
            {
                sb.Append("/");
                sb.Append(resource);
            }
            return sb.ToString();
        }

        private void parse()
        {
            if (m_server != null)
                return; // already parsed

            string user = null;
            string server = null;
            string resource = null;

            int at = m_JID.IndexOf('@');
            int slash = m_JID.IndexOf('/');

            if (at == -1)
            {
                user = null;
                if (slash == -1)
                {
                    server = m_JID;
                    resource = null;
                }
                else
                {
                    server = m_JID.Substring(0, slash);
                    resource = m_JID.Substring(slash+1);
                }
            }
            else
            {
                if (slash == -1)
                {
                    user = m_JID.Substring(0, at);
                    server = m_JID.Substring(at + 1);
                }
                else
                {
                    if (at < slash)
                    { // normal case
                        user = m_JID.Substring(0, at);
                        server = m_JID.Substring(at+1, slash-at-1);
                        resource = m_JID.Substring(slash+1);
                    }
                    else
                    { // @ in a resource, with no user.  bastards.
                        user = null;
                        server = m_JID.Substring(0, slash);
                        resource = m_JID.Substring(slash+1);
                    }
                }
            }
            if (user != null)
            {
                if (user.IndexOf('@') != -1) throw new JIDFormatException(m_JID);
                if (user.IndexOf('/') != -1) throw new JIDFormatException(m_JID);
            }

            if ((server == null) || (server.Length == 0)) throw new JIDFormatException(m_JID);
            if (server.IndexOf('@') != -1) throw new JIDFormatException(m_JID);
            if (server.IndexOf('/') != -1) throw new JIDFormatException(m_JID);
            if ((resource != null) && (resource.Length == 0)) // null is ok, but "" is not.
                throw new JIDFormatException(m_JID);

#if !NO_STRINGPREP
            m_user = (user == null) ? null : s_nodeprep.Prepare(user);
            m_server = s_nameprep.Prepare(server);
            m_resource = (resource == null) ? null : s_resourceprep.Prepare(resource);
#else
            m_user = (user == null) ? null : user.ToLower();
            m_server = server.ToLower();
            m_resource = resource;
#endif
            // Make the case right, for fast equality comparisons
            m_JID = build(m_user, m_server, m_resource);
        }

        /// <summary>
        /// Gets the hash code on the string version of the JID.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return m_JID.GetHashCode();
        }

        /// <summary>
        /// Returns the string representation.
        /// </summary>
        /// <returns>String in the form of "[user]@[server]/[resource]</returns>
        public override string ToString()
        {
            return m_JID;
        }

        /// <summary>
        /// Equality of string representations.
        /// </summary>
        /// <param name="other">JID or string to compare against.</param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            if (other is string)
                return m_JID.Equals(other);
            if (! (other is JID))
                return false;

            return m_JID.Equals(((JID)other).m_JID);
        }

        /// <summary>
        /// Determines whether two JIDs have the same value.
        /// </summary>
        /// <param name="one">A JID to compare.</param>
        /// <param name="two">Another JID to compare to the first one.</param>
        /// <returns>True if everything (user, host and resource) are the same; otherwise false.</returns>
        public static bool operator==(JID one, JID two)
        {
            if ((object)one == null)
                return ((object)two == null);
            return one.Equals(two);
        }

        /// <summary>
        /// Determines whether the string representation of the specified JID is equal to the current JID.
        /// </summary>
        /// <param name="one">This string is converted to a JID than compared to the second parameter.</param>
        /// <param name="two">JID to compare to the first one.</param>
        /// <returns>True if everything (user, host and resource) are the same; otherwise false.</returns>
        public static bool operator==(string one, JID two)
        {
            if ((object)two == null)
                return ((object)one == null);
            return two.Equals(one);
        }

        /// <summary>
        /// Determines whether the string representation of the specified JID is not equal to the current JID.
        /// </summary>
        /// <param name="one">This string is converted to a JID than compared to the second parameter.</param>
        /// <param name="two">JID to compare to the first one.</param>
        /// <returns>True if one thing (user, host or resource) is different; otherwise false.</returns>
        public static bool operator!=(string one, JID two)
        {
            if ((object)two == null)
                return ((object)one != null);
            return !two.Equals(one);
        }

        /// <summary>
        /// Determines whether two JIDs have different values.
        /// </summary>
        /// <param name="one">A JID to compare.</param>
        /// <param name="two">Another JID to compare to the first one.</param>
        /// <returns>True if one thing (user, host and resource) is different; otherwise false.</returns>
        public static bool operator!=(JID one, JID two)
        {
            if ((object)one == null)
                return ((object)two != null);
            return !one.Equals(two);
        }

        /// <summary>
        /// Converts a string to a JID implicitly (no cast needed).
        /// </summary>
        /// <param name="jid">String containing a JID.</param>
        /// <returns>JID object representing the string passed in.</returns>
        public static implicit operator JID(string jid)
        {
            if (jid == null)
                return null;
            return new JID(jid);
        }

        /// <summary>
        /// Converts a JID to a string implicitly (no cast needed).
        /// </summary>
        /// <param name="jid">JID whos string representation we want.</param>
        /// <returns>String version of the jid.</returns>
        public static implicit operator string(JID jid)
        {
            if (jid == null)
                return null;
            return jid.m_JID;
        }

        /// <summary>
        /// Compares two JIDs.
        /// </summary>
        /// <param name="left">First JID.</param>
        /// <param name="right">Second JID.</param>
        /// <returns>True if the first JID is less to the second; otherwise false.</returns>
        public static bool operator<(JID left, JID right)
        {
            return left.CompareTo(right) == -1;
        }

        /// <summary>
        /// Compares two JIDs.
        /// </summary>
        /// <param name="left">First JID.</param>
        /// <param name="right">Second JID.</param>
        /// <returns>True if the first JID is greater than the second; otherwise false.</returns>
        public static bool operator>(JID left, JID right)
        {
            return left.CompareTo(right) == 1;
        }

        /// <summary>
        /// Compares two JIDs.
        /// </summary>
        /// <param name="left">First JID.</param>
        /// <param name="right">Second JID.</param>
        /// <returns>True if the first JID is less than or equal to the second; otherwise false.</returns>
        public static bool operator<=(JID left, JID right)
        {
            return left.CompareTo(right) != 1;
        }

        /// <summary>
        /// Compares two JIDs.
        /// </summary>
        /// <param name="left">First JID.</param>
        /// <param name="right">Second JID.</param>
        /// <returns>True if the first JID is greater than or equal to the second; otherwise false.</returns>
        public static bool operator>=(JID left, JID right)
        {
            return left.CompareTo(right) != -1;
        }

        /// <summary>
        /// Gets and sets the username value of the JID, and returns null if it does not exist.
        /// </summary>
        public string User
        {
            get
            {
                parse();
                return m_user;
            }
            set
            {
                parse();
                m_user = value;
                m_JID = build(m_user, m_server, m_resource);
            }
        }

        /// <summary>
        /// Gets and sets the XMPP server domain value.
        /// </summary>
        public string Server
        {
            get
            {
                parse();
                return m_server;
            }
            set
            {
                parse();
                m_server = value;
                m_JID = build(m_user, m_server, m_resource);
            }
        }

        /// <summary>
        /// Gets and sets the resource value and returns null if it does not exist.
        /// </summary>
        public string Resource
        {
            get
            {
                parse();
                return m_resource;
            }
            set
            {
                parse();
                m_resource = value;
                m_JID = build(m_user, m_server, m_resource);
            }
        }

        /// <summary>
        /// Gets the username and XMPP server domain values of the JID. For example: user@example.com
        /// </summary>
        public string Bare
        {
            get
            {
                parse();
                if (m_resource == null)
                    return m_JID;
                return build(m_user, m_server, null);
            }
        }

        /// <summary>
        /// Gets the user@server JID associated with this JID, as a JID.
        /// Slightly faster than building it yourself, since stringprep
        /// is avoided.
        /// </summary>
        public JID BareJID
        {
            get
            {
                parse();
                if (m_resource == null)
                    return this; // already bare
                return new JID(m_user, m_server, null, build(m_user, m_server, null));
            }
        }


        /// <summary>
        /// XEP-0106 escaping.
        /// </summary>
        /// <returns></returns>
        public static JID Escape(string user, string server, string resource)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (char c in user)
            {
                switch (c)
                {
                    case ' ':
                        if ((count == 0) || (count == (user.Length - 1)))
                            throw new JIDFormatException();
                        sb.Append("\\20");
                        break;
                    case '"':
                        sb.Append("\\22");
                        break;
                    case '&':
                        sb.Append("\\26");
                        break;
                    case '\'':
                        sb.Append("\\27");
                        break;
                    case '/':
                        sb.Append("\\2f");
                        break;
                    case ':':
                        sb.Append("\\3a");
                        break;
                    case '<':
                        sb.Append("\\3c");
                        break;
                    case '>':
                        sb.Append("\\3e");
                        break;
                    case '@':
                        sb.Append("\\40");
                        break;
                    case '\\':
                        sb.Append("\\5c");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
                count++;
            }
            string u = sb.ToString();
            return new JID(u, server, resource);
        }

        /// <summary>
        /// Unescape the username portion of a JID, as specified in XEP-106.
        /// </summary>
        /// <returns></returns>
        public string Unescape()
        {
            Regex re = new Regex(@"\\([2-5][0267face])");
            string u = re.Replace(m_user, new MatchEvaluator(delegate(Match m)
            {
                switch (m.Groups[1].Value)
                {
                    case "20":
                        return " ";
                    case "22":
                        return "\"";
                    case "26":
                        return "&";
                    case "27":
                        return "'";
                    case "2f":
                        return "/";
                    case "3a":
                        return ":";
                    case "3c":
                        return "<";
                    case "3e":
                        return ">";
                    case "40":
                        return "@";
                    case "5c":
                        return "\\";
                    default:
                        return m.Groups[0].Value;
                }
            }));
            return u;
        }

        #region Implementation of IComparable
        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the comparands. The return value has these meanings:
        /// Less than zero This instance is less than obj.
        /// Zero This instance is equal to obj.
        /// Greater than zero This instance is greater than obj.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj == (object)this)
                return 0;

            JID oj = obj as JID;
            if (oj == null)
                throw new ArgumentException("Comparison of JID to non-JID", "obj");

            // hm.  How tricky to get?
            // It could be that sorting by domain first is correct...
            //return this.m_JID.CompareTo(oj.m_JID);
            this.parse();
            oj.parse();

            int c = this.m_server.ToLower().CompareTo(oj.m_server.ToLower());
            if (c != 0) return c;

            if (this.m_user == null)
            {
                if (oj.m_user != null)
                    return -1;
            }
            else
            {
                if (oj.m_user == null)
                    return 1;

                c = this.m_user.ToLower().CompareTo(oj.m_user.ToLower());
                if (c != 0) return c;
            }

            if (this.m_resource == null)
            {
                return (oj.m_resource == null) ? 0 : -1;
            }
            return this.m_resource.CompareTo(oj.m_resource);
        }
        #endregion
    }

    /// <summary>
    /// Convert a JID to and from a string, so that JIDs can be used as properties for
    /// components, and have those properties set at design time.
    /// </summary>
    public class JIDTypeConverter : System.ComponentModel.TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of one type to the type of this converter.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == null)
            {
                throw new ArgumentNullException("sourceType");
            }
            return ((sourceType == typeof(string)) ||
                    (typeof(JID).IsAssignableFrom(sourceType) ||
                     base.CanConvertFrom(context, sourceType)));
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
        {
             return ((destinationType == typeof(string)) ||
                     ((destinationType == typeof(JID)) ||
                    base.CanConvertTo(context, destinationType)));
        }

        /// <summary>
        /// Returns whether the given value object is valid for this type.
        /// Empty strings are allowed, since they will map to null.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(System.ComponentModel.ITypeDescriptorContext context, object value)
        {
            string s = value as string;
            JID j;
            if (s != null)
            {
                if (s == "")
                    return true;

                try
                {
                    j = new JID(s);
                }
                catch (JIDFormatException)
                {
                    return false;
                }
                return true;
            }
            j = value as JID;
            return (j != null);
        }

        /// <summary>
        /// Converts the given value to the type of this converter.
        /// Empty strings are converted to null.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value == null)
                return null;

            string s = value as string;
            if (s != null)
            {
                if (s == "")
                    return null;
                return new JID(s);
            }
            JID j = value as JID;
            if (j != null)
                return j;
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the given value object to the specified type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
                return null;
            if (destinationType == typeof(string))
                return value.ToString();
            if (destinationType == typeof(JID))
                return value;
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
