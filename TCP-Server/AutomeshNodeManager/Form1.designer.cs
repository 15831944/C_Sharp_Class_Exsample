namespace AutomeshNodeManager
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Uninstall_btn = new System.Windows.Forms.Button();
            this.install_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.help_btn = new System.Windows.Forms.Button();
            this.set_btn = new System.Windows.Forms.Button();
            this.stop_btn = new System.Windows.Forms.Button();
            this.start_btn = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "AutomeshNodtServer";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 26);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Uninstall_btn
            // 
            this.Uninstall_btn.Location = new System.Drawing.Point(165, 41);
            this.Uninstall_btn.Name = "Uninstall_btn";
            this.Uninstall_btn.Size = new System.Drawing.Size(71, 23);
            this.Uninstall_btn.TabIndex = 1;
            this.Uninstall_btn.Text = "UnInstall";
            this.Uninstall_btn.UseVisualStyleBackColor = true;
            this.Uninstall_btn.Click += new System.EventHandler(this.Uninstall_btn_Click);
            // 
            // install_btn
            // 
            this.install_btn.Location = new System.Drawing.Point(165, 13);
            this.install_btn.Name = "install_btn";
            this.install_btn.Size = new System.Drawing.Size(71, 23);
            this.install_btn.TabIndex = 1;
            this.install_btn.Text = "Install";
            this.install_btn.UseVisualStyleBackColor = true;
            this.install_btn.Click += new System.EventHandler(this.install_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = ">>> 已启动...";
            // 
            // help_btn
            // 
            this.help_btn.Location = new System.Drawing.Point(242, 40);
            this.help_btn.Name = "help_btn";
            this.help_btn.Size = new System.Drawing.Size(38, 23);
            this.help_btn.TabIndex = 1;
            this.help_btn.Text = "help";
            this.help_btn.UseVisualStyleBackColor = true;
            this.help_btn.Click += new System.EventHandler(this.help_btn_Click);
            // 
            // set_btn
            // 
            this.set_btn.Location = new System.Drawing.Point(242, 12);
            this.set_btn.Name = "set_btn";
            this.set_btn.Size = new System.Drawing.Size(38, 23);
            this.set_btn.TabIndex = 1;
            this.set_btn.Text = "set";
            this.set_btn.UseVisualStyleBackColor = true;
            this.set_btn.Click += new System.EventHandler(this.set_btn_Click);
            // 
            // stop_btn
            // 
            this.stop_btn.Location = new System.Drawing.Point(88, 41);
            this.stop_btn.Name = "stop_btn";
            this.stop_btn.Size = new System.Drawing.Size(71, 23);
            this.stop_btn.TabIndex = 1;
            this.stop_btn.Text = "stop";
            this.stop_btn.UseVisualStyleBackColor = true;
            this.stop_btn.Click += new System.EventHandler(this.stop_btn_Click);
            // 
            // start_btn
            // 
            this.start_btn.Location = new System.Drawing.Point(88, 13);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(71, 23);
            this.start_btn.TabIndex = 1;
            this.start_btn.Text = "start";
            this.start_btn.UseVisualStyleBackColor = true;
            this.start_btn.Click += new System.EventHandler(this.start_btn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 102);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.set_btn);
            this.Controls.Add(this.start_btn);
            this.Controls.Add(this.install_btn);
            this.Controls.Add(this.help_btn);
            this.Controls.Add(this.stop_btn);
            this.Controls.Add(this.Uninstall_btn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(888, 0);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "AM_Node";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button Uninstall_btn;
        private System.Windows.Forms.Button install_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button help_btn;
        private System.Windows.Forms.Button set_btn;
        private System.Windows.Forms.Button stop_btn;
        private System.Windows.Forms.Button start_btn;
    }
}

