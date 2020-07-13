using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chat.Core;
using Chat.Core.Client;
using Chat.Core.Cryptography;
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
                close = true;
                Application.Exit();
                return;
            }
            Text = $"Client | Chat [{Session.Client.Nick}]";
            openToolStripMenuItem.Image = global::ChatClient.Properties.Resources.line;
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
            openToolStripMenuItem.Image = global::ChatClient.Properties.Resources.gray;
            txtMessage.Clear();
            txtMessage.ReadOnly = true;
            lvClients.Items.Clear();
            btnSendMessage.Enabled = false;
        }

        private void refreshClientList(ClientListResponse response)
        {
            Session.Clients = response.Clients;
            lvClients.Items.Clear();
            Text = $"Client | Chat [{response.Client.Nick} -  {response.Client.Status.ClientStatusToString()}]";
            notifyIcon1.Text = Text;
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(18, 18);
            imageList.Images.Add(((int)ClientStatus.Available).ToString(), global::ChatClient.Properties.Resources.line);
            imageList.Images.Add(((int)ClientStatus.Away).ToString(), global::ChatClient.Properties.Resources.yellow);
            imageList.Images.Add(((int)ClientStatus.Busy).ToString(), global::ChatClient.Properties.Resources.red);
            imageList.Images.Add(((int)ClientStatus.DoNotDisturb).ToString(), global::ChatClient.Properties.Resources.dnd);
            imageList.Images.Add(((int)ClientStatus.Invisible).ToString(), global::ChatClient.Properties.Resources.gray);
            lvClients.SmallImageList = imageList;
            foreach (var client in response.Clients.Where(c => c.ClientId != Session.Client.ClientId))
            {
                ListViewItem item = new ListViewItem();
                item.Text = client.Nick;
                item.ImageIndex = imageList.Images.IndexOfKey(((int)client.Status).ToString());
                item.SubItems.Add(client.ClientId.ToString());
                lvClients.Items.Add(item);
            }
        }

        private void newMessage(MessageReceivingArguments e)
        {
            if (e.Message.To == 0)
            {
                if (Session.Client.Status != ClientStatus.DoNotDisturb)
                    TopMost = true;
                var fromClient = Session.Clients.First(c => c.ClientId == e.Message.From);
                txtMessages.Text += $@"{fromClient.Nick}: {e.Message.Content} [{e.Date.ToShortTimeString()}]{Environment.NewLine}";
                if (Session.Client.Status != ClientStatus.DoNotDisturb)
                    TopMost = false;
            }
            else
            {
                long clientId = e.Message.To == Session.Client.ClientId ? e.Message.From : e.Message.To;
                var clientItem = Session.Clients.First(c => c.ClientId == clientId);
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
            if (!close)
            {
                e.Cancel = true;
                Hide();
            }
            else if (Session.HasConnection)
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
                long clientId = long.Parse(lvClients.SelectedItems[0].SubItems[1].Text);
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
                    if (Session.Client.Status != ClientStatus.DoNotDisturb)
                    {
                        form.TopMost = true;
                        form.TopMost = false;
                    }
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

        bool close = false;

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            close = true;
            Application.Exit();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void availableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setStatus(ClientStatus.Available);
        }

        private void busyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setStatus(ClientStatus.Busy);
        }

        private void doNotDistrubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setStatus(ClientStatus.DoNotDisturb);
        }

        private void invisibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setStatus(ClientStatus.Invisible);
        }

        private void awayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setStatus(ClientStatus.Away);
        }

        private void setStatus(ClientStatus status)
        {
            Session.Client.SetStatus(status);
            switch (status)
            {
                case ClientStatus.Available:
                    openToolStripMenuItem.Image = global::ChatClient.Properties.Resources.line;
                    break;
                case ClientStatus.Busy:
                    openToolStripMenuItem.Image = global::ChatClient.Properties.Resources.red;
                    break;
                case ClientStatus.Away:
                    openToolStripMenuItem.Image = global::ChatClient.Properties.Resources.yellow;
                    break;
                case ClientStatus.DoNotDisturb:
                    openToolStripMenuItem.Image = global::ChatClient.Properties.Resources.dnd;
                    break;
                case ClientStatus.Invisible:
                    openToolStripMenuItem.Image = global::ChatClient.Properties.Resources.gray;
                    break;
                default:
                    openToolStripMenuItem.Image = global::ChatClient.Properties.Resources.line;
                    break;
            }
        }
    }
}
