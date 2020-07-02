using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
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
        Dictionary<long, frmMessage> privateMessageFormList = new Dictionary<long, frmMessage>();
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
            Text = $"Chat | {Session.Client.Nick}";
            Session.Client.NewMessgeReceived += new dgNewMessageReceived(newMessageReceived);
            Session.Client.ClientListRefresh += new dgClientListRefresh(clientListRefresh);
            Session.Client.ServerStopped += new dgServerStopped(() => Invoke(new dgServerStopped(serverStopped)));
        }

        private void newMessageReceived(MessageReceivingArguments e)
        {
            Invoke(new dgNewMessageReceived(newMessage), e);
        }

        private void clientListRefresh(ClientListResponse response)
        {
            Invoke(new dgClientListRefresh(refreshClientList), response);
        }

        private void serverStopped()
        {
            txtMessage.Clear();
            txtMessage.ReadOnly = true;
            lvClients.Items.Clear();
            btnSendMessage.Enabled = false;
        }

        private void refreshClientList(ClientListResponse response)
        {
            Session.Clients = response.Clients;
            lvClients.Items.Clear();
            Text = response.Client.Nick;
            foreach (var client in response.Clients)
            {
                ListViewItem item = new ListViewItem();
                item.Text = client.ClientId.ToString();
                item.SubItems.Add(client.Nick);
                lvClients.Items.Add(item);
            }
        }

        private void newMessage(MessageReceivingArguments e)
        {
            if (e.Message.To == 0)
            {
                var to = Session.Clients.First(c => c.ClientId == e.Message.From);
                txtMessages.Text += $@"{to.Nick}: {e.Message.Content} [{e.Date.ToShortTimeString()}]{Environment.NewLine}";
            }
            else
            {
                long clientId = e.Message.To == Session.Client.ClientId ? e.Message.From : e.Message.To;
                var form = openPriveteMessage(clientId);
                form.ReceivedMessage(e.Message, e.Date);
            }
            txtMessages.SelectionStart = txtMessages.Text.Length;
            txtMessages.ScrollToCaret();
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            sendMessage();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Session.HasConnection)
            {
                if (Session.Client != null)
                    Session.Client.Disconnected();
            }
        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                sendMessage();
        }

        private void sendMessage()
        {
            if (!string.IsNullOrEmpty(txtMessage.Text) && Session.Client.SendMessage(txtMessage.Text))
                txtMessage.Clear();
        }

        private void lvClients_DoubleClick(object sender, EventArgs e)
        {
            if (lvClients.SelectedItems.Count == 1)
            {
                long clientId = long.Parse(lvClients.SelectedItems[0].Text);
                if (clientId == Session.Client.ClientId)
                    return;
                openPriveteMessage(clientId);
            }
        }

        private frmMessage openPriveteMessage(long clientId)
        {
            var toClient = Session.Clients.First(c => c.ClientId == clientId);
            if (privateMessageFormList.ContainsKey(clientId))
            {
                var form = privateMessageFormList[clientId];
                if (form.Created)
                {
                    form.Show();
                    form.TopMost = true;
                    form.TopMost = false;
                }
                else
                {
                    form = new frmMessage(toClient);
                    privateMessageFormList[clientId] = form;
                    form.Show();
                }
                return form;
            }
            else
            {
                var form = new frmMessage(toClient);
                privateMessageFormList.Add(clientId, form);
                form.Show();
                return form;
            }
        }
    }
}
