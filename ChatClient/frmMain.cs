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
        bool hasConnection = false;
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
            Session.Client.ClientListRefresh += new dgClientListRefresh(clientListRefresh);
        }

        public void newMessageReceived(MessageReceivingArguments e)
        {
            Invoke(new dgNewMessageReceived(newMessage), e);
        }

        public void clientListRefresh(List<ClientItem> clients)
        {
            Invoke(new dgClientListRefresh(refreshClientList), clients);
        }

        private void refreshClientList(List<ClientItem> clients)
        {
            lvClients.Items.Clear();
            foreach (var client in clients)
            {
                ListViewItem item = new ListViewItem();
                item.Text = client.ClientId.ToString();
                item.SubItems.Add(client.Nick);
                item.SubItems.Add("");
                lvClients.Items.Add(item);
            }
        }

        private void newMessage(MessageReceivingArguments e)
        {
            txtMessages.Text += $@"\n{e.Message.From}: {e.Message.Content}";
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            Session.Client.SendMessage(txtMessage.Text);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Session.HasConnection)
            {
                if (Session.Client != null)
                    Session.Client.Disconnected();
            }
        }
    }
}
