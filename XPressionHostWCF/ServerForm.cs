using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XPressionHostWCF.Properties;
using XPressionService;

namespace XPressionHostWCF
{
    public partial class ServerForm : Form
    {

        XPressionWCFServer service = new XPressionWCFServer();
        public ServerForm()
        {
            InitializeComponent();
            //Console.SetOut(new ControlWriter(textBox1));
            tray.Icon = Resources.tray;
        }

        private void toggle_server_CheckedChanged(object sender, EventArgs e)
        {
            if (toggle_server.Checked)
            {
                service.Create(numericUpDown1.Value + "");
                service.Open();
                toggle_server.Text = "Server [ON]";
                Console.WriteLine("Service Started - " + DateTime.Now.ToString());
            }
            else
            {
                service.Close();
                toggle_server.Text = "Server [OFF]";
                Console.WriteLine("Service Stopped - " + DateTime.Now.ToString());
            }
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            try
            {
                Immutable.Create();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public class ControlWriter : TextWriter
        {
            private Control textbox;
            public ControlWriter(Control textbox)
            {
                this.textbox = textbox;
            }

            public override void Write(char value)
            {
                textbox.Text += value;
            }

            public override void Write(string value)
            {
                textbox.Text += value;
            }

            public override Encoding Encoding
            {
                get { return Encoding.ASCII; }
            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized || WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;
                tray.Visible = true;
                tray.ShowBalloonTip(1000, "Minimized To Tray!", "Form has been minimized to the system tray.", ToolTipIcon.Info);
                e.Cancel = true;
            }
        }

        private void tray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            tray.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tool_quit_Click(object sender, EventArgs e)
        {
            if(service.Running) service.Close();
            Environment.Exit(0);
        }

        private void tool_start_Click(object sender, EventArgs e)
        {
            if (!service.Running)
            {
                toggle_server.Checked = true;
            }
        }

        private void tool_stop_Click(object sender, EventArgs e)
        {
            if (service.Running)
            {
                toggle_server.Checked = false;
            }
        }

        private void tool_show_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            tray.Visible = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (service.Running) service.Close();
            Environment.Exit(0);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
