using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeManager
{
    public partial class FrmChat : Form
    {
        Chatter chatter;
        private StringBuilder outputs;
        Thread th;
        string currentInput;
        bool inProgress;
        public String Command
        {
            get
            {
                return txtInput.Text;
            }
            set
            {
                txtInput.Text = value;
                try
                {
                    txtInput.Select(value.Length, 0);
                    txtInput.ScrollToCaret();
                }
                catch { }
            }
        }
        public FrmChat()
        {
            InitializeComponent();
            chatter = new Chatter();
            outputs = new StringBuilder();
            inProgress = false;
        }

        private void appendOutput(String s)
        {
            if (s != "")
            {
                outputs.Append(s + "\n");
                txtHistory.Text = outputs.ToString().Replace("\r", "").Replace("\n", "\r\n");
            }
            txtHistory.Select(txtHistory.Text.Length, 0);
            txtHistory.ScrollToCaret();
        }
        void chat()
        {
            inProgress = true;
            String r = chatter.chat(currentInput.ToString());
            Invoke(new Action(()=> appendOutput("ChatGLM: " + r+"\n")));
            inProgress = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape: Hide(); return true;
                case Keys.Enter:
                    if (inProgress) return false;
                    if (txtInput.Focused)
                    {
                        try
                        {
                            appendOutput("User: " + Command);
                            currentInput = Command;
                            Command = "";
                            th = new Thread(new ThreadStart(chat));
                            th.Start();
                        }
                        catch(Exception ex)
                        {
                            appendOutput("Error!\n" + ex.ToString());
                        }
                        return true;
                    }
                    return false;
                case Keys.Up:
                    if (txtInput.Focused)
                    {
                        Command = chatter.last();
                        return true;
                    }
                    return false;
                case Keys.Down:
                    if (txtInput.Focused)
                    {
                        Command = chatter.last();
                        return true;
                    }
                    return false;
                default: break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void startChat()
        {
            Show();
        }
    }
}
