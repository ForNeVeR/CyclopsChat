namespace Cyclops.Core.Resource.Debug
{
    partial class RawXmppViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.xmppDebugger = new muzzle.XmppDebugger();
            this.SuspendLayout();
            // 
            // xmppDebugger
            // 
            this.xmppDebugger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xmppDebugger.ErrorColor = System.Drawing.Color.Red;
            this.xmppDebugger.Location = new System.Drawing.Point(0, 0);
            this.xmppDebugger.Name = "xmppDebugger";
            this.xmppDebugger.OtherColor = System.Drawing.Color.Green;
            this.xmppDebugger.OverrideFrom = null;
            this.xmppDebugger.ReceiveColor = System.Drawing.Color.Orange;
            this.xmppDebugger.SendColor = System.Drawing.Color.Blue;
            this.xmppDebugger.Size = new System.Drawing.Size(472, 437);
            this.xmppDebugger.Stream = null;
            this.xmppDebugger.TabIndex = 0;
            this.xmppDebugger.TextColor = System.Drawing.SystemColors.WindowText;
            // 
            // RawXmppViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 437);
            this.Controls.Add(this.xmppDebugger);
            this.Name = "RawXmppViewer";
            this.Text = "Raw Xmpp Viewer";
            this.ResumeLayout(false);

        }

        #endregion

        private muzzle.XmppDebugger xmppDebugger;
    }
}