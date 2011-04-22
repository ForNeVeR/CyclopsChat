using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using muzzle;

namespace Cyclops.Core.Resource.Debug
{
    public partial class RawXmppViewer : Form, IDebugWindow
    {

        public RawXmppViewer()
        {
            InitializeComponent();
        }

        #region Implementation of IDebugWindow

        public void ShowWindow(IUserSession session)
        {
            xmppDebugger.Stream = ((UserSession)session).JabberClient;
            Show();
        }

        #endregion

        private void ButtonClearClick(object sender, EventArgs e)
        {
            xmppDebugger.Clear();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            base.OnClosing(e);
        }
    }
}
