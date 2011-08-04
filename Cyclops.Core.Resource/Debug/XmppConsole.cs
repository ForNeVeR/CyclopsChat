using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using muzzle;

namespace Cyclops.Core.Resource.Debug
{
    public partial class XmppConsole : Form, IDebugWindow
    {
        private XmppDebugger xmppDebugger = null;

        public XmppConsole()
        {
            InitializeComponent();
            panel.Controls.Add(xmppDebugger = new XmppDebugger());
            xmppDebugger.Dock = DockStyle.Fill;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            xmppDebugger.Clear();
        }

        private void hideButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        public void ShowConsole(IUserSession session)
        {
            xmppDebugger.Stream = ((UserSession)session).JabberClient;
            Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
