using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chat.Core.Server;

namespace ChatServer
{
    public partial class frmMain : Form
    {
        Chat.Core.Server.ChatServer server;
        bool start = false;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void clientDisconnected(ClientConnectionArguments e)
        {
            Invoke(new dgClientDisconnected(removeClient), e);
        }

        private void clientConnection(ClientConnectionArguments e)
        {
            Invoke(new dgClientConnected(newClient), e);
        }

        private void newMessageReceivedFromClient(ClientSendMessageArguments e)
        {
            Invoke(new dgNewMessageReceivedFromClient(newMessage), e);
        }

        private void removeClient(ClientConnectionArguments e)
        {
            Session.Clients.Remove(Session.Clients.First(c => c.ClientId == e.Client.ClientId));
            setMessage($"Logout => Nick: {e.Client.Nick}, IP: {e.Client.IPAddress}, Logout Date: {e.Date.ToShortDateString()} {e.Date.ToLongTimeString()}");
            refreshClientList();
        }

        private void newClient(ClientConnectionArguments e)
        {
            Session.Clients.Add(new Chat.Core.ClientItem() { ClientId = e.Client.ClientId, Nick = e.Client.Nick, IPAddress = e.Client.IPAddress });
            setMessage($"Login => Nick: {e.Client.Nick}, IP: {e.Client.IPAddress}, Login Date: {e.Date.ToShortDateString()} {e.Date.ToLongTimeString()}");
            refreshClientList();
        }

        private void refreshClientList()
        {
            lvClients.Items.Clear();
            foreach (var client in Session.Clients)
            {
                ListViewItem item = new ListViewItem();
                item.Text = client.ClientId.ToString();
                item.SubItems.Add(client.Nick);
                item.SubItems.Add(client.IPAddress);
                lvClients.Items.Add(item);
            }
        }

        private void newMessage(ClientSendMessageArguments e)
        {
            string to = e.Message.To == 0 ? string.Empty : "- " + Session.Clients.First(c => c.ClientId == e.Message.To).Nick;
            setMessage($@"{e.Client.Nick} {to}: {(e.Message.To == 0 ? e.Message.Content : "#######")} [{e.Date.ToShortTimeString()}]");
        }

        private void setMessage(string message)
        {
            txtMessages.Text += $@"{message}{Environment.NewLine}";
            txtMessages.SelectionStart = txtMessages.Text.Length;
            txtMessages.ScrollToCaret();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!start)
                {
                    int portNo = 0;
                    if (string.IsNullOrEmpty(txtPortNo.Text))
                    {
                        errorProvider1.SetError(txtPortNo, "Cannot be empty");
                        return;
                    }
                    else if (!int.TryParse(txtPortNo.Text, out portNo))
                    {
                        errorProvider1.SetError(txtPortNo, "Not valid Port No");
                        return;
                    }
                    else if (portNo < 100 && portNo > 65535)
                    {
                        errorProvider1.SetError(txtPortNo, "Not valid Port No");
                        return;
                    }

                    server = new Chat.Core.Server.ChatServer(portNo);
                    server.NewMessageReceivedFromClient += new dgNewMessageReceivedFromClient(newMessageReceivedFromClient);
                    server.ClientDisconnected += new dgClientDisconnected(clientDisconnected);
                    server.ClientConnected += new dgClientConnected(clientConnection);

                    server.Start();
                    string hostName = Dns.GetHostName();
                    var addressList = Dns.GetHostByName(hostName).AddressList;
                    setMessage($"Server started => IP: {(addressList.Length > 0 ? addressList[0].ToString() : "hostName")} Port: {portNo}, Date: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}");
                    btnStart.Text = "Stop";
                    btnStart.BackColor = Color.Crimson;
                    BackColor = Color.GhostWhite;
                }
                else
                {
                    server.Stop();
                    string hostName = Dns.GetHostName();
                    var addressList = Dns.GetHostByName(hostName).AddressList;
                    setMessage($"Server stopped => IP: {(addressList.Length > 0 ? addressList[0].ToString() : "hostName")} Port: {txtPortNo.Text}, Date: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}");
                    btnStart.Text = "Start";
                    btnStart.BackColor = Color.LimeGreen;
                    BackColor = Color.AliceBlue;
                    lvClients.Items.Clear();
                }

                start = !start;
                txtPortNo.ReadOnly = start;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (start && server != null)
                server.Stop();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuClients_Opening(object sender, CancelEventArgs e)
        {
            banToolStripMenuItem.Enabled = lvClients.SelectedItems.Count == 1;
            if (lvClients.SelectedItems.Count == 1)
            {
                long clientId = long.Parse(lvClients.SelectedItems[0].Text);
                var client = server.Clients[clientId];
                banToolStripMenuItem.Text = client.BlockStatus ? "Unblock" : "Block";
            }
        }

        private void banToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long clientId = long.Parse(lvClients.SelectedItems[0].Text);
            var client = server.Clients[clientId];
            if (client.BlockStatus)
                client.Unblock();
            else
                client.Block();
        }
    }

    class ListClientItem
    {
        public long Id { get; set; }
        public string Nick { get; set; }
        public string IPAddress { get; set; }
    }
}
