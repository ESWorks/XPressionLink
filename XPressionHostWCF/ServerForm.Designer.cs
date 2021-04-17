namespace XPressionHostWCF
{
    partial class ServerForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.tray = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tool_quit = new System.Windows.Forms.ToolStripMenuItem();
            this.tool_start = new System.Windows.Forms.ToolStripMenuItem();
            this.tool_stop = new System.Windows.Forms.ToolStripMenuItem();
            this.tool_show = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.toggle_server = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(12, 30);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(125, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // tray
            // 
            this.tray.ContextMenuStrip = this.contextMenuStrip;
            this.tray.Text = "Tray";
            this.tray.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tray_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tool_quit,
            this.tool_start,
            this.tool_stop,
            this.tool_show});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(153, 92);
            // 
            // tool_quit
            // 
            this.tool_quit.Name = "tool_quit";
            this.tool_quit.Size = new System.Drawing.Size(152, 22);
            this.tool_quit.Text = "Close Program";
            this.tool_quit.Click += new System.EventHandler(this.tool_quit_Click);
            // 
            // tool_start
            // 
            this.tool_start.Name = "tool_start";
            this.tool_start.Size = new System.Drawing.Size(152, 22);
            this.tool_start.Text = "Start Server";
            this.tool_start.Click += new System.EventHandler(this.tool_start_Click);
            // 
            // tool_stop
            // 
            this.tool_stop.Name = "tool_stop";
            this.tool_stop.Size = new System.Drawing.Size(152, 22);
            this.tool_stop.Text = "Stop Server";
            this.tool_stop.Click += new System.EventHandler(this.tool_stop_Click);
            // 
            // tool_show
            // 
            this.tool_show.Name = "tool_show";
            this.tool_show.Size = new System.Drawing.Size(152, 22);
            this.tool_show.Text = "Show Window";
            this.tool_show.Click += new System.EventHandler(this.tool_show_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(147, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Log";
            this.label2.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(143, 29);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(321, 221);
            this.textBox1.TabIndex = 3;
            this.textBox1.Visible = false;
            // 
            // toggle_server
            // 
            this.toggle_server.Appearance = System.Windows.Forms.Appearance.Button;
            this.toggle_server.Location = new System.Drawing.Point(12, 56);
            this.toggle_server.Name = "toggle_server";
            this.toggle_server.Size = new System.Drawing.Size(125, 23);
            this.toggle_server.TabIndex = 4;
            this.toggle_server.Text = "Server [OFF]";
            this.toggle_server.UseVisualStyleBackColor = true;
            this.toggle_server.CheckedChanged += new System.EventHandler(this.toggle_server_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox1.Location = new System.Drawing.Point(12, 85);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(125, 23);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Close";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 253);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.toggle_server);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ServerForm";
            this.Text = "ServerForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
            this.Load += new System.EventHandler(this.ServerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NotifyIcon tray;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox toggle_server;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tool_quit;
        private System.Windows.Forms.ToolStripMenuItem tool_start;
        private System.Windows.Forms.ToolStripMenuItem tool_stop;
        private System.Windows.Forms.ToolStripMenuItem tool_show;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}