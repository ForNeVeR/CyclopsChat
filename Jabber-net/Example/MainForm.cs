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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using bedrock.util;
using jabber;
using jabber.connection;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Example
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    [SVN(@"$Id: MainForm.cs 733 2008-09-07 23:03:44Z hildjj $")]
    public class MainForm : Form
    {
        #region Private Members

        private StatusBar sb;
        private jabber.client.JabberClient jc;
        private jabber.client.RosterManager rm;
        private jabber.client.PresenceManager pm;
        private TabControl tabControl1;
        private TabPage tpDebug;
        private TabPage tpRoster;
        private StatusBarPanel pnlCon;
        private StatusBarPanel pnlPresence;
        private ContextMenu mnuPresence;
        private MenuItem mnuAvailable;
        private MenuItem mnuAway;
        private IContainer components;
        private muzzle.RosterTree roster;
        private StatusBarPanel pnlSSL;
        private DiscoManager dm;
        private TabPage tpServices;
        private CapsManager cm;
        private muzzle.XmppDebugger debug;
        private PubSubManager psm;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem connectToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem servicesToolStripMenuItem;
        private ToolStripMenuItem debugToolStripMenuItem;
        private ToolStripMenuItem rosterToolStripMenuItem;
        private ToolStripMenuItem addContactToolStripMenuItem;
        private ToolStripMenuItem removeContactToolStripMenuItem;
        private ToolStripMenuItem addGroupToolStripMenuItem;
        private IdleTime idler;
        private ServiceDisplay services;
        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem closeTabToolStripMenuItem;
        private ToolStripMenuItem deletePubSubToolStripMenuItem;
        private ToolStripMenuItem subscribePubSubToolStripMenuItem;
        private ToolStripMenuItem pubSubToolStripMenuItem;
        private ConferenceManager muc;
        private ToolStripMenuItem joinConferenceToolStripMenuItem;

        private bool m_err = false;
        private jabber.client.BookmarkManager bmm;
        private TabPage tpBookmarks;
        private ListView lvBookmarks;
        private ColumnHeader chName;
        private ColumnHeader chNick;
        private ColumnHeader chAutoJoin;
        private ToolStripMenuItem bookmarkToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private bool m_connected = false;

        #endregion

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            services.ImageList = roster.ImageList;
            cm.AddFeature(URI.TIME);
            cm.AddFeature(URI.VERSION);
            cm.AddFeature(URI.LAST);
            cm.AddFeature(URI.DISCO_INFO);

            tabControl1.TabPages.Remove(tpServices);
            tabControl1.TabPages.Remove(tpDebug);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }


        void idler_OnUnIdle(object sender, TimeSpan span)
        {
            jc.Presence(PresenceType.available, "Available", null, 0);
            pnlPresence.Text = "Available";
        }

        private void idler_OnIdle(object sender, TimeSpan span)
        {
            jc.Presence(PresenceType.available, "Auto-away", "away", 0);
            pnlPresence.Text = "Away";
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            idler.Enabled = false;

            if( disposing )
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

#region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            jabber.connection.Ident ident2 = new jabber.connection.Ident();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.sb = new System.Windows.Forms.StatusBar();
            this.pnlCon = new System.Windows.Forms.StatusBarPanel();
            this.pnlSSL = new System.Windows.Forms.StatusBarPanel();
            this.pnlPresence = new System.Windows.Forms.StatusBarPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpRoster = new System.Windows.Forms.TabPage();
            this.roster = new muzzle.RosterTree();
            this.jc = new jabber.client.JabberClient(this.components);
            this.pm = new jabber.client.PresenceManager(this.components);
            this.cm = new jabber.connection.CapsManager(this.components);
            this.dm = new jabber.connection.DiscoManager(this.components);
            this.rm = new jabber.client.RosterManager(this.components);
            this.tpServices = new System.Windows.Forms.TabPage();
            this.services = new Example.ServiceDisplay();
            this.tpBookmarks = new System.Windows.Forms.TabPage();
            this.lvBookmarks = new System.Windows.Forms.ListView();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.chNick = new System.Windows.Forms.ColumnHeader();
            this.chAutoJoin = new System.Windows.Forms.ColumnHeader();
            this.tpDebug = new System.Windows.Forms.TabPage();
            this.debug = new muzzle.XmppDebugger();
            this.mnuPresence = new System.Windows.Forms.ContextMenu();
            this.mnuAvailable = new System.Windows.Forms.MenuItem();
            this.mnuAway = new System.Windows.Forms.MenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joinConferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.servicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rosterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addContactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeContactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pubSubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subscribePubSubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePubSubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.psm = new jabber.connection.PubSubManager(this.components);
            this.idler = new bedrock.util.IdleTime();
            this.muc = new jabber.connection.ConferenceManager(this.components);
            this.bmm = new jabber.client.BookmarkManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pnlCon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSSL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlPresence)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpRoster.SuspendLayout();
            this.tpServices.SuspendLayout();
            this.tpBookmarks.SuspendLayout();
            this.tpDebug.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sb
            // 
            this.sb.Location = new System.Drawing.Point(0, 416);
            this.sb.Name = "sb";
            this.sb.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.pnlCon,
            this.pnlSSL,
            this.pnlPresence});
            this.sb.ShowPanels = true;
            this.sb.Size = new System.Drawing.Size(632, 22);
            this.sb.TabIndex = 0;
            this.sb.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.sb_PanelClick);
            // 
            // pnlCon
            // 
            this.pnlCon.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.pnlCon.Name = "pnlCon";
            this.pnlCon.Text = "Click on \"Offline\", and select a presence to log in.";
            this.pnlCon.Width = 538;
            // 
            // pnlSSL
            // 
            this.pnlSSL.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.pnlSSL.Name = "pnlSSL";
            this.pnlSSL.Width = 30;
            // 
            // pnlPresence
            // 
            this.pnlPresence.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.pnlPresence.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.pnlPresence.Name = "pnlPresence";
            this.pnlPresence.Text = "Offline";
            this.pnlPresence.Width = 47;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpRoster);
            this.tabControl1.Controls.Add(this.tpServices);
            this.tabControl1.Controls.Add(this.tpBookmarks);
            this.tabControl1.Controls.Add(this.tpDebug);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(632, 392);
            this.tabControl1.TabIndex = 2;
            // 
            // tpRoster
            // 
            this.tpRoster.Controls.Add(this.roster);
            this.tpRoster.Location = new System.Drawing.Point(4, 22);
            this.tpRoster.Name = "tpRoster";
            this.tpRoster.Size = new System.Drawing.Size(624, 366);
            this.tpRoster.TabIndex = 1;
            this.tpRoster.Text = "Roster";
            this.tpRoster.UseVisualStyleBackColor = true;
            // 
            // roster
            // 
            this.roster.AllowDrop = true;
            this.roster.Client = this.jc;
            this.roster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.roster.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.roster.ImageIndex = 1;
            this.roster.Location = new System.Drawing.Point(0, 0);
            this.roster.Name = "roster";
            this.roster.PresenceManager = this.pm;
            this.roster.RosterManager = this.rm;
            this.roster.SelectedImageIndex = 0;
            this.roster.ShowLines = false;
            this.roster.ShowRootLines = false;
            this.roster.Size = new System.Drawing.Size(624, 366);
            this.roster.Sorted = true;
            this.roster.StatusColor = System.Drawing.Color.Teal;
            this.roster.TabIndex = 0;
            this.roster.DoubleClick += new System.EventHandler(this.roster_DoubleClick);
            // 
            // jc
            // 
            this.jc.AutoReconnect = 3F;
            this.jc.AutoStartCompression = true;
            this.jc.AutoStartTLS = true;
            this.jc.InvokeControl = this;
            this.jc.KeepAlive = 30F;
            this.jc.LocalCertificate = null;
            this.jc.Password = null;
            this.jc.User = null;
            this.jc.OnRegisterInfo += new jabber.client.RegisterInfoHandler(this.jc_OnRegisterInfo);
            this.jc.OnError += new bedrock.ExceptionHandler(this.jc_OnError);
            this.jc.OnIQ += new jabber.client.IQHandler(this.jc_OnIQ);
            this.jc.OnAuthenticate += new bedrock.ObjectHandler(this.jc_OnAuthenticate);
            this.jc.OnStreamError += new jabber.protocol.ProtocolHandler(this.jc_OnStreamError);
            this.jc.OnConnect += new jabber.connection.StanzaStreamHandler(this.jc_OnConnect);
            this.jc.OnDisconnect += new bedrock.ObjectHandler(this.jc_OnDisconnect);
            this.jc.OnAuthError += new jabber.protocol.ProtocolHandler(this.jc_OnAuthError);
            this.jc.OnRegistered += new jabber.client.IQHandler(this.jc_OnRegistered);
            this.jc.OnMessage += new jabber.client.MessageHandler(this.jc_OnMessage);
            // 
            // pm
            // 
            this.pm.CapsManager = this.cm;
            this.pm.OverrideFrom = null;
            this.pm.Stream = this.jc;
            // 
            // cm
            // 
            this.cm.DiscoManager = this.dm;
            this.cm.Features = new string[0];
            this.cm.FileName = "caps.xml";
            ident2.Category = "client";
            ident2.Lang = "en";
            ident2.Name = "Jabber-Net Test Client";
            ident2.Type = "pc";
            this.cm.Identities = new jabber.connection.Ident[] {
        ident2};
            this.cm.Node = "http://cursive.net/clients/csharp-example";
            this.cm.OverrideFrom = null;
            this.cm.Stream = this.jc;
            // 
            // dm
            // 
            this.dm.OverrideFrom = null;
            this.dm.Stream = this.jc;
            // 
            // rm
            // 
            this.rm.AutoAllow = jabber.client.AutoSubscriptionHanding.AllowIfSubscribed;
            this.rm.AutoSubscribe = true;
            this.rm.OverrideFrom = null;
            this.rm.Stream = this.jc;
            this.rm.OnRosterEnd += new bedrock.ObjectHandler(this.rm_OnRosterEnd);
            this.rm.OnSubscription += new jabber.client.SubscriptionHandler(this.rm_OnSubscription);
            this.rm.OnUnsubscription += new jabber.client.UnsubscriptionHandler(this.rm_OnUnsubscription);
            // 
            // tpServices
            // 
            this.tpServices.Controls.Add(this.services);
            this.tpServices.Location = new System.Drawing.Point(4, 22);
            this.tpServices.Name = "tpServices";
            this.tpServices.Size = new System.Drawing.Size(624, 366);
            this.tpServices.TabIndex = 2;
            this.tpServices.Text = "Services";
            this.tpServices.UseVisualStyleBackColor = true;
            // 
            // services
            // 
            this.services.DiscoManager = this.dm;
            this.services.Dock = System.Windows.Forms.DockStyle.Fill;
            this.services.ImageList = null;
            this.services.Location = new System.Drawing.Point(0, 0);
            this.services.Name = "services";
            this.services.Size = new System.Drawing.Size(624, 366);
            this.services.Stream = this.jc;
            this.services.TabIndex = 0;
            // 
            // tpBookmarks
            // 
            this.tpBookmarks.Controls.Add(this.lvBookmarks);
            this.tpBookmarks.Location = new System.Drawing.Point(4, 22);
            this.tpBookmarks.Name = "tpBookmarks";
            this.tpBookmarks.Size = new System.Drawing.Size(624, 366);
            this.tpBookmarks.TabIndex = 3;
            this.tpBookmarks.Text = "Bookmarks";
            this.tpBookmarks.UseVisualStyleBackColor = true;
            // 
            // lvBookmarks
            // 
            this.lvBookmarks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chNick,
            this.chAutoJoin});
            this.lvBookmarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvBookmarks.Location = new System.Drawing.Point(0, 0);
            this.lvBookmarks.Name = "lvBookmarks";
            this.lvBookmarks.Size = new System.Drawing.Size(624, 366);
            this.lvBookmarks.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvBookmarks.TabIndex = 0;
            this.lvBookmarks.UseCompatibleStateImageBehavior = false;
            this.lvBookmarks.View = System.Windows.Forms.View.Details;
            this.lvBookmarks.DoubleClick += new System.EventHandler(this.lvBookmarks_DoubleClick);
            this.lvBookmarks.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvBookmarks_KeyUp);
            // 
            // chName
            // 
            this.chName.Text = "Room";
            this.chName.Width = 198;
            // 
            // chNick
            // 
            this.chNick.Text = "Nick";
            this.chNick.Width = 88;
            // 
            // chAutoJoin
            // 
            this.chAutoJoin.Text = "AutoJoin";
            // 
            // tpDebug
            // 
            this.tpDebug.Controls.Add(this.debug);
            this.tpDebug.Location = new System.Drawing.Point(4, 22);
            this.tpDebug.Name = "tpDebug";
            this.tpDebug.Size = new System.Drawing.Size(624, 366);
            this.tpDebug.TabIndex = 0;
            this.tpDebug.Text = "Debug";
            this.tpDebug.UseVisualStyleBackColor = true;
            // 
            // debug
            // 
            this.debug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debug.ErrorColor = System.Drawing.Color.Red;
            this.debug.Location = new System.Drawing.Point(0, 0);
            this.debug.Name = "debug";
            this.debug.OtherColor = System.Drawing.Color.Green;
            this.debug.OverrideFrom = null;
            this.debug.ReceiveColor = System.Drawing.Color.Orange;
            this.debug.SendColor = System.Drawing.Color.Blue;
            this.debug.Size = new System.Drawing.Size(624, 366);
            this.debug.Stream = this.jc;
            this.debug.TabIndex = 0;
            this.debug.TextColor = System.Drawing.Color.Black;
            // 
            // mnuPresence
            // 
            this.mnuPresence.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuAvailable,
            this.mnuAway});
            // 
            // mnuAvailable
            // 
            this.mnuAvailable.Enabled = false;
            this.mnuAvailable.Index = 0;
            this.mnuAvailable.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mnuAvailable.Text = "&Available";
            this.mnuAvailable.Click += new System.EventHandler(this.mnuAvailable_Click);
            // 
            // mnuAway
            // 
            this.mnuAway.Enabled = false;
            this.mnuAway.Index = 1;
            this.mnuAway.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.mnuAway.Text = "A&way";
            this.mnuAway.Click += new System.EventHandler(this.mnuAway_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.rosterToolStripMenuItem,
            this.bookmarkToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.pubSubToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(632, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.joinConferenceToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.connectToolStripMenuItem.Text = "&Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // joinConferenceToolStripMenuItem
            // 
            this.joinConferenceToolStripMenuItem.Name = "joinConferenceToolStripMenuItem";
            this.joinConferenceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
            this.joinConferenceToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.joinConferenceToolStripMenuItem.Text = "&Join Conference";
            this.joinConferenceToolStripMenuItem.Click += new System.EventHandler(this.joinConferenceToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(197, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.servicesToolStripMenuItem,
            this.debugToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // servicesToolStripMenuItem
            // 
            this.servicesToolStripMenuItem.Name = "servicesToolStripMenuItem";
            this.servicesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.servicesToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.servicesToolStripMenuItem.Text = "&Services";
            this.servicesToolStripMenuItem.Click += new System.EventHandler(this.servicesToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.debugToolStripMenuItem.Text = "&Debug";
            this.debugToolStripMenuItem.Click += new System.EventHandler(this.debugToolStripMenuItem_Click);
            // 
            // rosterToolStripMenuItem
            // 
            this.rosterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addContactToolStripMenuItem,
            this.removeContactToolStripMenuItem,
            this.addGroupToolStripMenuItem});
            this.rosterToolStripMenuItem.Name = "rosterToolStripMenuItem";
            this.rosterToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.rosterToolStripMenuItem.Text = "&Roster";
            // 
            // addContactToolStripMenuItem
            // 
            this.addContactToolStripMenuItem.Name = "addContactToolStripMenuItem";
            this.addContactToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Insert;
            this.addContactToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.addContactToolStripMenuItem.Text = "&Add Contact";
            this.addContactToolStripMenuItem.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // removeContactToolStripMenuItem
            // 
            this.removeContactToolStripMenuItem.Name = "removeContactToolStripMenuItem";
            this.removeContactToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.removeContactToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.removeContactToolStripMenuItem.Text = "&Remove Contact";
            this.removeContactToolStripMenuItem.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // addGroupToolStripMenuItem
            // 
            this.addGroupToolStripMenuItem.Name = "addGroupToolStripMenuItem";
            this.addGroupToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.addGroupToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.addGroupToolStripMenuItem.Text = "&Add Group";
            this.addGroupToolStripMenuItem.Click += new System.EventHandler(this.addGroupToolStripMenuItem_Click);
            // 
            // bookmarkToolStripMenuItem
            // 
            this.bookmarkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.bookmarkToolStripMenuItem.Name = "bookmarkToolStripMenuItem";
            this.bookmarkToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.bookmarkToolStripMenuItem.Text = "Bookmark";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.addToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.B)));
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeTabToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.windowToolStripMenuItem.Text = "&Window";
            // 
            // closeTabToolStripMenuItem
            // 
            this.closeTabToolStripMenuItem.Name = "closeTabToolStripMenuItem";
            this.closeTabToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.closeTabToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.closeTabToolStripMenuItem.Text = "&Close Tab";
            this.closeTabToolStripMenuItem.Click += new System.EventHandler(this.closeTabToolStripMenuItem_Click);
            // 
            // pubSubToolStripMenuItem
            // 
            this.pubSubToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subscribePubSubToolStripMenuItem,
            this.deletePubSubToolStripMenuItem});
            this.pubSubToolStripMenuItem.Name = "pubSubToolStripMenuItem";
            this.pubSubToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.pubSubToolStripMenuItem.Text = "PubSub";
            // 
            // subscribePubSubToolStripMenuItem
            // 
            this.subscribePubSubToolStripMenuItem.Name = "subscribePubSubToolStripMenuItem";
            this.subscribePubSubToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.subscribePubSubToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.subscribePubSubToolStripMenuItem.Text = "&Subscribe";
            this.subscribePubSubToolStripMenuItem.Click += new System.EventHandler(this.subscribeToPubSubToolStripMenuItem_Click);
            // 
            // deletePubSubToolStripMenuItem
            // 
            this.deletePubSubToolStripMenuItem.Name = "deletePubSubToolStripMenuItem";
            this.deletePubSubToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.deletePubSubToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.deletePubSubToolStripMenuItem.Text = "&Delete";
            this.deletePubSubToolStripMenuItem.Click += new System.EventHandler(this.deletePubSubToolStripMenuItem_Click);
            // 
            // psm
            // 
            this.psm.OverrideFrom = null;
            this.psm.Stream = this.jc;
            // 
            // idler
            // 
            this.idler.InvokeControl = this;
            this.idler.OnIdle += new bedrock.util.SpanEventHandler(this.idler_OnIdle);
            this.idler.OnUnIdle += new bedrock.util.SpanEventHandler(this.idler_OnUnIdle);
            // 
            // muc
            // 
            this.muc.OverrideFrom = null;
            this.muc.Stream = this.jc;
            // 
            // bmm
            // 
            this.bmm.ConferenceManager = this.muc;
            this.bmm.OverrideFrom = null;
            this.bmm.Stream = this.jc;
            this.bmm.OnConferenceAdd += new jabber.client.BookmarkConferenceDelegate(this.bmm_OnConferenceAdd);
            this.bmm.OnConferenceRemove += new jabber.client.BookmarkConferenceDelegate(this.bmm_OnConferenceRemove);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(632, 438);
            this.ContextMenu = this.mnuPresence;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.sb);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.pnlCon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSSL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlPresence)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tpRoster.ResumeLayout(false);
            this.tpServices.ResumeLayout(false);
            this.tpBookmarks.ResumeLayout(false);
            this.tpDebug.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion

        /// <summary>
        /// The MainForm entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }

        private void Connect()
        {
            muzzle.ClientLogin.Login(jc, "login.xml");
        }

        private void jc_OnAuthenticate(object sender)
        {
            pnlPresence.Text = "Available";
            pnlCon.Text = "Connected";
            mnuAway.Enabled = mnuAvailable.Enabled = true;

            if (jc.SSLon)
            {

                pnlSSL.Text = "SSL";
                System.Security.Cryptography.X509Certificates.X509Certificate cert2 =
                    (System.Security.Cryptography.X509Certificates.X509Certificate)
                    jc[Options.REMOTE_CERTIFICATE];

                string cert_str = cert2.ToString(true);
                debug.Write("CERT:", cert_str);
                pnlSSL.ToolTipText = cert_str;
            }
            idler.Enabled = true;
        }

        private void jc_OnDisconnect(object sender)
        {
            m_connected = false;
            mnuAway.Enabled = mnuAvailable.Enabled = false;
            idler.Enabled = false;
            pnlPresence.Text = "Offline";
            pnlSSL.Text = "";
            pnlSSL.ToolTipText = "";
            connectToolStripMenuItem.Text = "&Connect";
            lvBookmarks.Items.Clear();

            if (!m_err)
                pnlCon.Text = "Disconnected";
        }

        private void jc_OnError(object sender, Exception ex)
        {
            m_connected = false;
            mnuAway.Enabled = mnuAvailable.Enabled = false;
            connectToolStripMenuItem.Text = "&Connect";
            idler.Enabled = false;
            lvBookmarks.Items.Clear();

            pnlCon.Text = "Error: " + ex.Message;
        }

        private void jc_OnAuthError(object sender, XmlElement elem)
        {
            if (MessageBox.Show(this,
                "Create new account?",
                "Authentication error",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (!m_connected)
                {
                    MessageBox.Show("You have been disconnected by the server.  Registration is not enabled.", "Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                jc.Register(new JID(jc.User, jc.Server, null));
            }
            else
            {
                jc.Close(false);
            }
        }

        private void jc_OnRegistered(object sender, IQ iq)
        {
            if (iq.Type == IQType.result)
                jc.Login();
            else
                pnlCon.Text = "Registration error";
        }

        private bool jc_OnRegisterInfo(object sender, Register r)
        {
            if (r.Form == null)
                return true;
            muzzle.XDataForm f = new muzzle.XDataForm(r.Form);
            if (f.ShowDialog() != DialogResult.OK)
                return false;
            f.FillInResponse(r.Form);
            return true;
        }

        private void jc_OnMessage(object sender, jabber.protocol.client.Message msg)
        {
            jabber.protocol.x.Data x = msg["x", URI.XDATA] as jabber.protocol.x.Data;
            if (x != null)
            {
                muzzle.XDataForm f = new muzzle.XDataForm(msg);
                f.ShowDialog(this);
                jc.Write(f.GetResponse());
            }
            else
                MessageBox.Show(this, msg.Body, msg.From, MessageBoxButtons.OK);
        }

        private void jc_OnIQ(object sender, IQ iq)
        {
            if (iq.Type != IQType.get)
                return;

            XmlElement query = iq.Query;
            if (query == null)
                return;

            // <iq id="jcl_8" to="me" from="you" type="get"><query xmlns="jabber:iq:version"/></iq>
            if (query is jabber.protocol.iq.Version)
            {
                iq = iq.GetResponse(jc.Document);
                jabber.protocol.iq.Version ver = iq.Query as jabber.protocol.iq.Version;
                if (ver != null)
                {
                    ver.OS = Environment.OSVersion.ToString();
                    ver.EntityName = Application.ProductName;
                    ver.Ver = Application.ProductVersion;
                }
                jc.Write(iq);
                return;
            }

            if (query is Time)
            {
                iq = iq.GetResponse(jc.Document);
                Time tim = iq.Query as Time;
                if (tim != null) tim.SetCurrentTime();
                jc.Write(iq);
                return;
            }

            if (query is Last)
            {
                iq = iq.GetResponse(jc.Document);
                Last last = iq.Query as Last;
                if (last != null) last.Seconds = (int)IdleTime.GetIdleTime();
                jc.Write(iq);
                return;
            }
        }

        private void roster_DoubleClick(object sender, EventArgs e)
        {
            muzzle.RosterTree.ItemNode n = roster.SelectedNode as muzzle.RosterTree.ItemNode;
            if (n == null)
                return;
            new SendMessage(jc, n.JID).Show();
        }

        private void sb_PanelClick(object sender, StatusBarPanelClickEventArgs e)
        {
            if (e.StatusBarPanel != pnlPresence)
                return;
            mnuPresence.Show(sb, new Point(e.X, e.Y));
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (jc.IsAuthenticated)
            {
                jc.Close(true);
                connectToolStripMenuItem.Text = "&Connect";
            }
            else
            {
                Connect();
                connectToolStripMenuItem.Text = "Dis&connect";
            }
        }

        private void mnuAvailable_Click(object sender, EventArgs e)
        {
            if (jc.IsAuthenticated)
            {
                jc.Presence(PresenceType.available, "Available", null, 0);
                pnlPresence.Text = "Available";
            }
            else
                Connect();
        }

        private void mnuAway_Click(object sender, EventArgs e)
        {
            if (jc.IsAuthenticated)
            {
                jc.Presence(PresenceType.available, "Away", "away", 0);
                pnlPresence.Text = "Away";
            }
            else
                Connect();
        }

        /*
        private void mnuOffline_Click(object sender, EventArgs e)
        {
            if (jc.IsAuthenticated)
                jc.Close();
        }
         */

        void jc_OnConnect(object sender, StanzaStream stream)
        {
            m_err = false;
            m_connected = true;
        }

        private void jc_OnStreamError(object sender, XmlElement rp)
        {
            m_err = true;
            pnlCon.Text = "Stream error: " + rp.InnerText;
        }

        /*
        private void txtDebugInput_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && e.Control)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(txtDebugInput.Text);
                    XmlElement elem = doc.DocumentElement;
                    if (elem != null)
                        jc.Write(elem);
                    txtDebugInput.Clear();
                }
                catch (XmlException ex)
                {
                    MessageBox.Show("Invalid XML: " + ex.Message);
                }
            }

        }
*/
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString(), "Unhandled exception: " + e.GetType());
        }

        private void rm_OnRosterEnd(object sender)
        {
            roster.ExpandAll();
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            if (jc.IsAuthenticated)
                jc.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jc.Close();
            Close();
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            AddContact ac = new AddContact();
            ac.AllGroups = roster.Groups;
            ac.DefaultDomain = jc.Server;
            if (ac.ShowDialog() != DialogResult.OK)
                return;

            jc.Subscribe(ac.JID, ac.Nickname, ac.SelectedGroups);
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            muzzle.RosterTree.ItemNode n = roster.SelectedNode as muzzle.RosterTree.ItemNode;
            if (n == null)
                return;
            jc.RemoveRosterItem(n.JID);
       }


        // add group
        private void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGroup ag = new AddGroup();
            if (ag.ShowDialog() == DialogResult.Cancel)
                return;

            if (ag.GroupName == "")
                return;

            roster.AddGroup(ag.GroupName).EnsureVisible();
        }

        private void servicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Contains(tpServices))
            {
                tabControl1.TabPages.Remove(tpServices);
                servicesToolStripMenuItem.Checked = false;
            }
            else
            {
                tabControl1.TabPages.Add(tpServices);
                tabControl1.SelectedTab = tpServices;
                servicesToolStripMenuItem.Checked = true;
            }
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Contains(tpDebug))
            {
                tabControl1.TabPages.Remove(tpDebug);
                debugToolStripMenuItem.Checked = false;
            }
            else
            {
                tabControl1.TabPages.Add(tpDebug);
                tabControl1.SelectedTab = tpDebug;
                debugToolStripMenuItem.Checked = true;
            }
        }

        private void subscribeToPubSubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PubSubSubcribeForm ps = new PubSubSubcribeForm();
            // this is a small race.  to do it right, I should call dm.BeginFindServiceWithFeature,
            // and modify that to call back on all of the found services.  The idea is that
            // by the the time the user has a chance to click on the menu item, the DiscoManager
            // will be populated.
            ps.DiscoManager = dm;
            if (ps.ShowDialog() != DialogResult.OK)
                return;
            JID jid = ps.JID;
            string node = ps.Node;
            string text = string.Format("{0}/{1}", jid, node);

            TabPage tp = new TabPage(text);
            tp.Name = text;

            PubSubDisplay disp = new PubSubDisplay();
            disp.Node = psm.GetNode(jid, node, 10);
            tp.Controls.Add(disp);
            disp.Dock = DockStyle.Fill;

            tabControl1.TabPages.Add(tp);
            tabControl1.SelectedTab = tp;
        }

        private void rm_OnSubscription(jabber.client.RosterManager manager, Item ri, Presence pres)
        {
            DialogResult res = MessageBox.Show("Allow incoming presence subscription request from: " + pres.From,
                "Subscription Request",
                MessageBoxButtons.YesNoCancel);
            switch (res)
            {
            case DialogResult.Yes:
                manager.ReplyAllow(pres);
                break;
            case DialogResult.No:
                manager.ReplyDeny(pres);
                break;
            case DialogResult.Cancel:
                // do nothing;
                break;
            }
        }

        private void rm_OnUnsubscription(jabber.client.RosterManager manager, Presence pres, ref bool remove)
        {
            MessageBox.Show(pres.From + " has removed you from their roster.", "Unsubscription notification", MessageBoxButtons.OK);
        }

        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tp = tabControl1.SelectedTab;
            if (tp == tpRoster)
                return;
            else if (tp == tpDebug)
                debugToolStripMenuItem.Checked = false;
            else if (tp == tpServices)
                servicesToolStripMenuItem.Checked = false;
            tabControl1.TabPages.Remove(tp);
        }

        private void deletePubSubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PubSubSubcribeForm fm = setupPubSubForm();
            if (fm.ShowDialog() != DialogResult.OK)
                return;

            JID jid = fm.JID;
            string node = fm.Node;

            psm.RemoveNode(jid, node,
                delegate {
                    MessageBox.Show("Remove Node unsuccessful.");
                });

            tabControl1.TabPages.RemoveByKey(string.Format("{0}/{1}", jid, node));
        }

        private PubSubSubcribeForm setupPubSubForm()
        {
            string JID = null;
            string node = null;
            if (tabControl1.SelectedTab.Name != null && tabControl1.SelectedTab.Name.Contains("/"))
            {
                string value = tabControl1.SelectedTab.Name;
                int index = value.IndexOf("/");

                JID = value.Substring(0, index);
                node = value.Substring(index + 1);
            }

            PubSubSubcribeForm fm = new PubSubSubcribeForm();
            fm.Text = "Delete PubSub";
            if (JID != null) fm.JID = JID;
            if (node != null) fm.Node = node;
            fm.DiscoManager = dm;

            return fm;
        }

        private void joinConferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConferenceForm cf = new ConferenceForm();
            cf.DiscoManager = dm;
            cf.Nick = muc.DefaultNick;
            if (cf.ShowDialog() != DialogResult.OK)
                return;

            muc.GetRoom(cf.RoomAndNick).Join();
        }

        private IQ muc_OnRoomConfig(Room room, IQ parent)
        {
            muzzle.XDataForm form = new muzzle.XDataForm(parent);
            if (form.ShowDialog() != DialogResult.OK)
                return null;

            return (IQ)form.GetResponse();
        }

        private void muc_OnPresenceError(Room room, Presence pres)
        {
            m_err = true;
            pnlCon.Text = "Groupchat error: " + pres.Error.OuterXml;
        }

        private void muc_OnInvite(object sender, jabber.protocol.client.Message msg)
        {
            Room r = sender as Room;
            r.Join();
        }

        private void bmm_OnConferenceAdd(jabber.client.BookmarkManager manager, BookmarkConference conference)
        {
            string jid = conference.JID;
            string name = conference.ConferenceName;
            if (name == null)
                name = jid;
            if (lvBookmarks.Items.ContainsKey(jid))
                lvBookmarks.Items.RemoveByKey(jid);
            ListViewItem item = lvBookmarks.Items.Add(jid, name, -1);
            item.SubItems.Add(conference.Nick);
            item.SubItems.Add(conference.AutoJoin.ToString());
            item.Tag = conference.JID;
        }

        private void bmm_OnConferenceRemove(jabber.client.BookmarkManager manager, BookmarkConference conference)
        {
            string jid = conference.JID;
            if (lvBookmarks.Items.ContainsKey(jid))
                lvBookmarks.Items.RemoveByKey(jid);
        }

        private void lvBookmarks_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                removeToolStripMenuItem_Click(null, null);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // pop up AddBookmark dialog
            ConferenceForm cf = new ConferenceForm();
            cf.DiscoManager = dm;
            cf.Nick = muc.DefaultNick;
            if (cf.ShowDialog() != DialogResult.OK)
                return;
            // TODO: add autojoin and name.
            bmm.AddConference(cf.RoomJID, null, false, cf.Nick);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lvBookmarks.SelectedItems)
            {
                bmm[(JID)lvi.Tag] = null;
            }
        }

        private void lvBookmarks_DoubleClick(object sender, EventArgs e)
        {
            if (lvBookmarks.SelectedItems.Count == 0)
                return;
            ListViewItem lvi = lvBookmarks.SelectedItems[0];

            JID jid = (JID)lvi.Tag;
            BookmarkConference conf = bmm[jid];
            Debug.Assert(conf != null);

            ConferenceForm cf = new ConferenceForm();
            cf.DiscoManager = dm;
            cf.RoomAndNick = new JID(jid.User, jid.Server, conf.Nick);

            if (cf.ShowDialog() != DialogResult.OK)
                return;
            bmm.AddConference(cf.RoomJID, null, false, cf.Nick);
        }
    }
}
