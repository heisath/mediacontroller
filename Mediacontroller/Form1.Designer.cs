namespace Mediacontroller
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Button5 = new System.Windows.Forms.Button();
            this.StartUpTimer = new System.Windows.Forms.Timer(this.components);
            this.grpNixie = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkMisc7 = new System.Windows.Forms.CheckBox();
            this.chkMisc0 = new System.Windows.Forms.CheckBox();
            this.chkMisc6 = new System.Windows.Forms.CheckBox();
            this.chkMisc1 = new System.Windows.Forms.CheckBox();
            this.chkMisc5 = new System.Windows.Forms.CheckBox();
            this.chkMisc2 = new System.Windows.Forms.CheckBox();
            this.chkMisc4 = new System.Windows.Forms.CheckBox();
            this.chkMisc3 = new System.Windows.Forms.CheckBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btnSetNx = new System.Windows.Forms.Button();
            this.btnNxClock = new System.Windows.Forms.Button();
            this.nupNx2 = new System.Windows.Forms.NumericUpDown();
            this.nupNxStatus = new System.Windows.Forms.NumericUpDown();
            this.nupNx1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nupNx3 = new System.Windows.Forms.NumericUpDown();
            this.nupNx4 = new System.Windows.Forms.NumericUpDown();
            this.btnConnectSerial = new System.Windows.Forms.Button();
            this.txtComName = new System.Windows.Forms.TextBox();
            this.NixieStartUpTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notify_btnAnzeigen = new System.Windows.Forms.ToolStripMenuItem();
            this.anzeigenToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.notify_btnTonLautsprecher = new System.Windows.Forms.ToolStripMenuItem();
            this.notify_btnTonGemischt = new System.Windows.Forms.ToolStripMenuItem();
            this.notify_btnTonKopfhoererH600 = new System.Windows.Forms.ToolStripMenuItem();
            this.notify_btnTonKopfhoererG935 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.notify_btnNixieAn = new System.Windows.Forms.ToolStripMenuItem();
            this.notify_btnNixieAus = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.notify_btnBeenden = new System.Windows.Forms.ToolStripMenuItem();
            this.grpNixie.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupNx2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNxStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNx1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNx3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNx4)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button5
            // 
            this.Button5.Location = new System.Drawing.Point(16, 236);
            this.Button5.Margin = new System.Windows.Forms.Padding(4);
            this.Button5.Name = "Button5";
            this.Button5.Size = new System.Drawing.Size(316, 28);
            this.Button5.TabIndex = 9;
            this.Button5.Text = "Hide";
            this.Button5.UseVisualStyleBackColor = true;
            this.Button5.Click += new System.EventHandler(this.ButtonHide_Click);
            // 
            // StartUpTimer
            // 
            this.StartUpTimer.Enabled = true;
            this.StartUpTimer.Interval = 10;
            this.StartUpTimer.Tick += new System.EventHandler(this.StartUpTimer_Tick);
            // 
            // grpNixie
            // 
            this.grpNixie.Controls.Add(this.panel1);
            this.grpNixie.Controls.Add(this.btnConnectSerial);
            this.grpNixie.Controls.Add(this.txtComName);
            this.grpNixie.Location = new System.Drawing.Point(16, 13);
            this.grpNixie.Margin = new System.Windows.Forms.Padding(4);
            this.grpNixie.Name = "grpNixie";
            this.grpNixie.Padding = new System.Windows.Forms.Padding(4);
            this.grpNixie.Size = new System.Drawing.Size(316, 201);
            this.grpNixie.TabIndex = 12;
            this.grpNixie.TabStop = false;
            this.grpNixie.Text = "Nixie-Control";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.btnSetNx);
            this.panel1.Controls.Add(this.btnNxClock);
            this.panel1.Controls.Add(this.nupNx2);
            this.panel1.Controls.Add(this.nupNxStatus);
            this.panel1.Controls.Add(this.nupNx1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.nupNx3);
            this.panel1.Controls.Add(this.nupNx4);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(0, 63);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(316, 138);
            this.panel1.TabIndex = 12;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkMisc7);
            this.panel2.Controls.Add(this.chkMisc0);
            this.panel2.Controls.Add(this.chkMisc6);
            this.panel2.Controls.Add(this.chkMisc1);
            this.panel2.Controls.Add(this.chkMisc5);
            this.panel2.Controls.Add(this.chkMisc2);
            this.panel2.Controls.Add(this.chkMisc4);
            this.panel2.Controls.Add(this.chkMisc3);
            this.panel2.Location = new System.Drawing.Point(16, 68);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(184, 28);
            this.panel2.TabIndex = 22;
            // 
            // chkMisc7
            // 
            this.chkMisc7.AutoSize = true;
            this.chkMisc7.Location = new System.Drawing.Point(0, 0);
            this.chkMisc7.Margin = new System.Windows.Forms.Padding(4);
            this.chkMisc7.Name = "chkMisc7";
            this.chkMisc7.Size = new System.Drawing.Size(18, 17);
            this.chkMisc7.TabIndex = 14;
            this.chkMisc7.UseVisualStyleBackColor = true;
            // 
            // chkMisc0
            // 
            this.chkMisc0.AutoSize = true;
            this.chkMisc0.Location = new System.Drawing.Point(140, 0);
            this.chkMisc0.Margin = new System.Windows.Forms.Padding(4);
            this.chkMisc0.Name = "chkMisc0";
            this.chkMisc0.Size = new System.Drawing.Size(18, 17);
            this.chkMisc0.TabIndex = 21;
            this.chkMisc0.UseVisualStyleBackColor = true;
            // 
            // chkMisc6
            // 
            this.chkMisc6.AutoSize = true;
            this.chkMisc6.Location = new System.Drawing.Point(20, 0);
            this.chkMisc6.Margin = new System.Windows.Forms.Padding(4);
            this.chkMisc6.Name = "chkMisc6";
            this.chkMisc6.Size = new System.Drawing.Size(18, 17);
            this.chkMisc6.TabIndex = 15;
            this.chkMisc6.UseVisualStyleBackColor = true;
            // 
            // chkMisc1
            // 
            this.chkMisc1.AutoSize = true;
            this.chkMisc1.Location = new System.Drawing.Point(120, 0);
            this.chkMisc1.Margin = new System.Windows.Forms.Padding(4);
            this.chkMisc1.Name = "chkMisc1";
            this.chkMisc1.Size = new System.Drawing.Size(18, 17);
            this.chkMisc1.TabIndex = 20;
            this.chkMisc1.UseVisualStyleBackColor = true;
            // 
            // chkMisc5
            // 
            this.chkMisc5.AutoSize = true;
            this.chkMisc5.Location = new System.Drawing.Point(40, 0);
            this.chkMisc5.Margin = new System.Windows.Forms.Padding(4);
            this.chkMisc5.Name = "chkMisc5";
            this.chkMisc5.Size = new System.Drawing.Size(18, 17);
            this.chkMisc5.TabIndex = 16;
            this.chkMisc5.UseVisualStyleBackColor = true;
            // 
            // chkMisc2
            // 
            this.chkMisc2.AutoSize = true;
            this.chkMisc2.Location = new System.Drawing.Point(100, 0);
            this.chkMisc2.Margin = new System.Windows.Forms.Padding(4);
            this.chkMisc2.Name = "chkMisc2";
            this.chkMisc2.Size = new System.Drawing.Size(18, 17);
            this.chkMisc2.TabIndex = 19;
            this.chkMisc2.UseVisualStyleBackColor = true;
            // 
            // chkMisc4
            // 
            this.chkMisc4.AutoSize = true;
            this.chkMisc4.Location = new System.Drawing.Point(60, 0);
            this.chkMisc4.Margin = new System.Windows.Forms.Padding(4);
            this.chkMisc4.Name = "chkMisc4";
            this.chkMisc4.Size = new System.Drawing.Size(18, 17);
            this.chkMisc4.TabIndex = 17;
            this.chkMisc4.UseVisualStyleBackColor = true;
            // 
            // chkMisc3
            // 
            this.chkMisc3.AutoSize = true;
            this.chkMisc3.Location = new System.Drawing.Point(80, 0);
            this.chkMisc3.Margin = new System.Windows.Forms.Padding(4);
            this.chkMisc3.Name = "chkMisc3";
            this.chkMisc3.Size = new System.Drawing.Size(18, 17);
            this.chkMisc3.TabIndex = 18;
            this.chkMisc3.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(212, 100);
            this.button7.Margin = new System.Windows.Forms.Padding(4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(96, 28);
            this.button7.TabIndex = 13;
            this.button7.Text = "C Down";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(112, 100);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(92, 28);
            this.button6.TabIndex = 12;
            this.button6.Text = "C Up";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // btnSetNx
            // 
            this.btnSetNx.Location = new System.Drawing.Point(208, 64);
            this.btnSetNx.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetNx.Name = "btnSetNx";
            this.btnSetNx.Size = new System.Drawing.Size(100, 28);
            this.btnSetNx.TabIndex = 4;
            this.btnSetNx.Text = "Set NX";
            this.btnSetNx.UseVisualStyleBackColor = true;
            this.btnSetNx.Click += new System.EventHandler(this.btnSetNx_Click);
            // 
            // btnNxClock
            // 
            this.btnNxClock.Location = new System.Drawing.Point(8, 100);
            this.btnNxClock.Margin = new System.Windows.Forms.Padding(4);
            this.btnNxClock.Name = "btnNxClock";
            this.btnNxClock.Size = new System.Drawing.Size(96, 28);
            this.btnNxClock.TabIndex = 11;
            this.btnNxClock.Text = "NX Clock";
            this.btnNxClock.UseVisualStyleBackColor = true;
            this.btnNxClock.Click += new System.EventHandler(this.btnNxClock_Click);
            // 
            // nupNx2
            // 
            this.nupNx2.Location = new System.Drawing.Point(64, 36);
            this.nupNx2.Margin = new System.Windows.Forms.Padding(4);
            this.nupNx2.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nupNx2.Name = "nupNx2";
            this.nupNx2.Size = new System.Drawing.Size(40, 22);
            this.nupNx2.TabIndex = 0;
            // 
            // nupNxStatus
            // 
            this.nupNxStatus.Location = new System.Drawing.Point(160, 4);
            this.nupNxStatus.Margin = new System.Windows.Forms.Padding(4);
            this.nupNxStatus.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nupNxStatus.Name = "nupNxStatus";
            this.nupNxStatus.Size = new System.Drawing.Size(40, 22);
            this.nupNxStatus.TabIndex = 10;
            // 
            // nupNx1
            // 
            this.nupNx1.Location = new System.Drawing.Point(16, 36);
            this.nupNx1.Margin = new System.Windows.Forms.Padding(4);
            this.nupNx1.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nupNx1.Name = "nupNx1";
            this.nupNx1.Size = new System.Drawing.Size(40, 22);
            this.nupNx1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 6);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "Status";
            // 
            // nupNx3
            // 
            this.nupNx3.Location = new System.Drawing.Point(112, 36);
            this.nupNx3.Margin = new System.Windows.Forms.Padding(4);
            this.nupNx3.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nupNx3.Name = "nupNx3";
            this.nupNx3.Size = new System.Drawing.Size(40, 22);
            this.nupNx3.TabIndex = 2;
            // 
            // nupNx4
            // 
            this.nupNx4.Location = new System.Drawing.Point(160, 36);
            this.nupNx4.Margin = new System.Windows.Forms.Padding(4);
            this.nupNx4.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nupNx4.Name = "nupNx4";
            this.nupNx4.Size = new System.Drawing.Size(40, 22);
            this.nupNx4.TabIndex = 3;
            // 
            // btnConnectSerial
            // 
            this.btnConnectSerial.Location = new System.Drawing.Point(208, 23);
            this.btnConnectSerial.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnectSerial.Name = "btnConnectSerial";
            this.btnConnectSerial.Size = new System.Drawing.Size(100, 28);
            this.btnConnectSerial.TabIndex = 6;
            this.btnConnectSerial.Text = "Verbinden";
            this.btnConnectSerial.UseVisualStyleBackColor = true;
            this.btnConnectSerial.Click += new System.EventHandler(this.btnConnectSerial_Click);
            // 
            // txtComName
            // 
            this.txtComName.Location = new System.Drawing.Point(16, 26);
            this.txtComName.Margin = new System.Windows.Forms.Padding(4);
            this.txtComName.Name = "txtComName";
            this.txtComName.Size = new System.Drawing.Size(132, 22);
            this.txtComName.TabIndex = 5;
            this.txtComName.Text = "COM4";
            // 
            // NixieStartUpTimer
            // 
            this.NixieStartUpTimer.Interval = 10000;
            this.NixieStartUpTimer.Tick += new System.EventHandler(this.NixieStartUpTimer_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Mediacontroller";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notify_btnAnzeigen,
            this.anzeigenToolStripMenuItem,
            this.notify_btnTonLautsprecher,
            this.notify_btnTonGemischt,
            this.notify_btnTonKopfhoererH600,
            this.notify_btnTonKopfhoererG935,
            this.toolStripSeparator2,
            this.notify_btnNixieAn,
            this.notify_btnNixieAus,
            this.toolStripSeparator1,
            this.notify_btnBeenden});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(213, 214);
            // 
            // notify_btnAnzeigen
            // 
            this.notify_btnAnzeigen.Name = "notify_btnAnzeigen";
            this.notify_btnAnzeigen.Size = new System.Drawing.Size(212, 24);
            this.notify_btnAnzeigen.Text = "Anzeigen";
            // 
            // anzeigenToolStripMenuItem
            // 
            this.anzeigenToolStripMenuItem.Name = "anzeigenToolStripMenuItem";
            this.anzeigenToolStripMenuItem.Size = new System.Drawing.Size(209, 6);
            // 
            // notify_btnTonLautsprecher
            // 
            this.notify_btnTonLautsprecher.Name = "notify_btnTonLautsprecher";
            this.notify_btnTonLautsprecher.Size = new System.Drawing.Size(212, 24);
            this.notify_btnTonLautsprecher.Text = "Ton Lautsprecher";
            this.notify_btnTonLautsprecher.Click += new System.EventHandler(this.notify_btnTonLautsprecher_Click);
            // 
            // notify_btnTonGemischt
            // 
            this.notify_btnTonGemischt.Name = "notify_btnTonGemischt";
            this.notify_btnTonGemischt.Size = new System.Drawing.Size(212, 24);
            this.notify_btnTonGemischt.Text = "Ton Gemischt";
            this.notify_btnTonGemischt.Click += new System.EventHandler(this.notify_btnTonGemischt_Click);
            // 
            // notify_btnTonKopfhoererH600
            // 
            this.notify_btnTonKopfhoererH600.Name = "notify_btnTonKopfhoererH600";
            this.notify_btnTonKopfhoererH600.Size = new System.Drawing.Size(212, 24);
            this.notify_btnTonKopfhoererH600.Text = "Ton Kopfhörer H600";
            this.notify_btnTonKopfhoererH600.Click += new System.EventHandler(this.notify_btnTonH600_Click);
            // 
            // notify_btnTonKopfhoererG935
            // 
            this.notify_btnTonKopfhoererG935.Name = "notify_btnTonKopfhoererG935";
            this.notify_btnTonKopfhoererG935.Size = new System.Drawing.Size(212, 24);
            this.notify_btnTonKopfhoererG935.Text = "Ton Kopfhörer G935";
            this.notify_btnTonKopfhoererG935.Click += new System.EventHandler(this.notify_btnTonG935_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(209, 6);
            // 
            // notify_btnNixieAn
            // 
            this.notify_btnNixieAn.Name = "notify_btnNixieAn";
            this.notify_btnNixieAn.Size = new System.Drawing.Size(212, 24);
            this.notify_btnNixieAn.Text = "Nixie an";
            this.notify_btnNixieAn.Click += new System.EventHandler(this.notify_btnNixieAn_Click);
            // 
            // notify_btnNixieAus
            // 
            this.notify_btnNixieAus.Name = "notify_btnNixieAus";
            this.notify_btnNixieAus.Size = new System.Drawing.Size(212, 24);
            this.notify_btnNixieAus.Text = "Nixie aus";
            this.notify_btnNixieAus.Click += new System.EventHandler(this.notify_btnNixieAus_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(209, 6);
            // 
            // notify_btnBeenden
            // 
            this.notify_btnBeenden.Name = "notify_btnBeenden";
            this.notify_btnBeenden.Size = new System.Drawing.Size(212, 24);
            this.notify_btnBeenden.Text = "Beenden";
            this.notify_btnBeenden.Click += new System.EventHandler(this.notify_btnBeenden_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 277);
            this.Controls.Add(this.grpNixie);
            this.Controls.Add(this.Button5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Mediacontroller";
            this.grpNixie.ResumeLayout(false);
            this.grpNixie.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupNx2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNxStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNx1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNx3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNx4)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.Button Button5;
        internal System.Windows.Forms.Timer StartUpTimer;
        private System.Windows.Forms.GroupBox grpNixie;
        private System.Windows.Forms.Button btnSetNx;
        private System.Windows.Forms.NumericUpDown nupNx4;
        private System.Windows.Forms.NumericUpDown nupNx3;
        private System.Windows.Forms.NumericUpDown nupNx1;
        private System.Windows.Forms.NumericUpDown nupNx2;
        private System.Windows.Forms.Button btnConnectSerial;
        private System.Windows.Forms.TextBox txtComName;
        private System.Windows.Forms.NumericUpDown nupNxStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnNxClock;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkMisc7;
        private System.Windows.Forms.CheckBox chkMisc0;
        private System.Windows.Forms.CheckBox chkMisc6;
        private System.Windows.Forms.CheckBox chkMisc1;
        private System.Windows.Forms.CheckBox chkMisc5;
        private System.Windows.Forms.CheckBox chkMisc2;
        private System.Windows.Forms.CheckBox chkMisc4;
        private System.Windows.Forms.CheckBox chkMisc3;
        private System.Windows.Forms.Timer NixieStartUpTimer;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem notify_btnAnzeigen;
        private System.Windows.Forms.ToolStripSeparator anzeigenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notify_btnTonLautsprecher;
        private System.Windows.Forms.ToolStripMenuItem notify_btnTonGemischt;
        private System.Windows.Forms.ToolStripMenuItem notify_btnTonKopfhoererG935;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem notify_btnNixieAn;
        private System.Windows.Forms.ToolStripMenuItem notify_btnNixieAus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem notify_btnBeenden;
        private System.Windows.Forms.ToolStripMenuItem notify_btnTonKopfhoererH600;
    }
}

