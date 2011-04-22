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
            this.buttonClear = new System.Windows.Forms.Button();
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
            this.xmppDebugger.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.xmppDebugger.ReceiveColor = System.Drawing.Color.Orange;
            this.xmppDebugger.SendColor = System.Drawing.Color.Blue;
            this.xmppDebugger.Size = new System.Drawing.Size(567, 413);
            this.xmppDebugger.Stream = null;
            this.xmppDebugger.TabIndex = 0;
            this.xmppDebugger.TextColor = System.Drawing.SystemColors.WindowText;
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.Location = new System.Drawing.Point(489, 387);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 1;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.ButtonClearClick);
            // 
            // RawXmppViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 413);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.xmppDebugger);
            this.Name = "RawXmppViewer";
            this.Text = "Raw Xmpp Viewer";
            this.ResumeLayout(false);

        }

        #endregion

        private muzzle.XmppDebugger xmppDebugger;
        private System.Windows.Forms.Button buttonClear;


    }
}