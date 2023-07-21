using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeManager
{
    public partial class FrmEdit : Form
    {
        public bool confirmed;
        public String Title
        {
            get
            {
                return txtTitle.Text;
            }
            set
            {
                txtTitle.Text = value;
            }
        }
        public String Desc
        {
            get
            {
                return txtDesc.Text;
            }
            set
            {
                txtDesc.Text = value;
            }
        }

        public String Code
        {
            get
            {
                return txtCode.Text;
            }
            set
            {
                txtCode.Text = value;
            }
        }
        public FrmEdit()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            confirmed = true;
            Hide();
        }

        public void startEdit(String title = null, String desc = null, String code = null)
        {
            if (title != null) Title = title;
            if (desc != null) Desc = desc;
            if (code != null) Code = code;
            confirmed = false;
            ShowDialog();
        }
        public Dictionary<string,object> ToDictionary(String[] fields)
        {
            Dictionary<string, object> d = new Dictionary<string, object>(3);
            d.Add(fields[0], Title);
            d.Add(fields[1], Desc);
            d.Add(fields[2], Code);
            return d;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    if (txtDesc.Focused || txtCode.Focused) return false;
                    confirmed = true; Hide(); return true;
                case Keys.Escape:
                    confirmed = false; Hide(); return true;
                default: break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
