using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chat.Core;
using Chat.Core.Client;
using Chat.Core.Server;

namespace ChatClient
{
    public partial class frmMain : Form
    {
        bool start = false;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.ShowDialog();
            if (Session.Client == null)
            {
                Application.Exit();
                return;
            }
            Session.Client.NewMessgeReceived += new dgNewMessageReceived(newMessageReceived);
        }

        public void newMessageReceived(MessageReceivingArguments e)
        {
            Invoke(new dgNewMessageReceived(newMessage), e);
        }

        private void newMessage(MessageReceivingArguments e)
        {
            txtMessages.Text += @"\n" + e.Message;
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            Session.Client.SendMessage(txtMessage.Text);
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Session.Client != null)
                Session.Client.Disconnected();
        }
    }
}
