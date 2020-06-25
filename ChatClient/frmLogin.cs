using Chat.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        int portNo = 0;

        private bool validation()
        {
            if (!int.TryParse(txtPortNo.Text, out portNo))
            {
                errorProvider1.SetError(txtPortNo, "Not valid Port No");
                return false;
            }
            return true;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                Session.Client = new Chat.Core.Client.ChatClient(txtIPAddress.Text, portNo, txtNick.Text);
                Session.Client.Connect();
                Close();
            }
        }
    }
}
