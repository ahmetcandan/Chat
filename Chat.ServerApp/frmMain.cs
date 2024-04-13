using Chat.Abstraction.Model;
using Chat.Server;
using Chat.Server.Event;
using System.ComponentModel;
using System.Net;

namespace Chat.ServerApp;

public partial class FrmMain : Form
{
    private ChatServer _server;
    private bool _start = false;
    public FrmMain()
    {
        InitializeComponent();
    }

    private void FrmMain_Load(object sender, EventArgs e)
    {

    }

    private void ClientDisconnected(ClientConnectionArguments e)
    {
        Invoke(new dgClientDisconnected(RemoveClient), e);
    }

    private void ClientConnection(ClientConnectionArguments e)
    {
        Invoke(new dgClientConnected(NewClient), e);
    }

    private void NewMessageReceivedFromClient(ClientSendMessageArguments e)
    {
        Invoke(new dgNewMessageReceivedFromClient(NewMessage), e);
    }

    private void RemoveClient(ClientConnectionArguments e)
    {
        Session.Clients.Remove(Session.Clients.First(c => c.ClientId == e.Client.ClientId));
        SetMessage($"Logout => Nick: {e.Client.Nick}, IP: {e.Client.IPAddress}, Logout Date: {e.Date.ToShortDateString()} {e.Date.ToLongTimeString()}");
        RefreshClientList();
    }

    private void NewClient(ClientConnectionArguments e)
    {
        Session.Clients.Add(new ClientItem(e.Client.ClientId, e.Client.Nick, e.Client.IPAddress, e.Client.PublicKey, e.Client.Status));
        SetMessage($"Login => Nick: {e.Client.Nick}, IP: {e.Client.IPAddress}, Login Date: {e.Date.ToShortDateString()} {e.Date.ToLongTimeString()}");
        RefreshClientList();
    }

    private void RefreshClientList()
    {
        lvClients.Items.Clear();
        foreach (var client in Session.Clients)
        {
            ListViewItem item = new()
            {
                Text = client.ClientId.ToString()
            };
            item.SubItems.Add(client.Nick);
            item.SubItems.Add(client.IPAddress);
            lvClients.Items.Add(item);
        }
    }

    private void NewMessage(ClientSendMessageArguments e)
    {
        string to = e.Message.To == 0 ? string.Empty : "- " + Session.Clients.First(c => c.ClientId == e.Message.To).Nick;
        SetMessage($@"{e.Client.Nick} {to}: {(e.Message.To == 0 ? e.Message.Content : "#######")} [{e.Date.ToShortTimeString()}]");
    }

    private void SetMessage(string message)
    {
        txtMessages.Text += $@"{message}{Environment.NewLine}";
        txtMessages.SelectionStart = txtMessages.Text.Length;
        txtMessages.ScrollToCaret();
    }

    private void BtnStart_Click(object sender, EventArgs e)
    {
        try
        {
            if (!_start)
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

                _server = new ChatServer(portNo);
                _server.NewMessageReceivedFromClient += new dgNewMessageReceivedFromClient(NewMessageReceivedFromClient);
                _server.ClientDisconnected += new dgClientDisconnected(ClientDisconnected);
                _server.ClientConnected += new dgClientConnected(ClientConnection);

                _server.Start();
                string hostName = Dns.GetHostName();
                var addressList = Dns.GetHostByName(hostName).AddressList;
                SetMessage($"Server started => IP: {(addressList.Length > 0 ? addressList[0].ToString() : hostName)} Port: {portNo}, Date: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}");
                btnStart.Text = "Stop";
                btnStart.BackColor = Color.Crimson;
                BackColor = Color.GhostWhite;
                notifyIcon1.Text = $"Chat | Server [{(addressList.Length > 0 ? addressList[0].ToString() : hostName)}:{portNo}]";
            }
            else
            {
                _server.Stop();
                string hostName = Dns.GetHostName();
                var addressList = Dns.GetHostByName(hostName).AddressList;
                SetMessage($"Server stopped => IP: {(addressList.Length > 0 ? addressList[0].ToString() : "hostName")} Port: {txtPortNo.Text}, Date: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}");
                btnStart.Text = "Start";
                btnStart.BackColor = Color.LimeGreen;
                BackColor = Color.AliceBlue;
                lvClients.Items.Clear();
                notifyIcon1.Text = $"Chat | Server";
            }

            _start = !_start;
            txtPortNo.ReadOnly = _start;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private bool _close = false;

    private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!_close)
        {
            e.Cancel = true;
            Hide();
        }
        else if (_start && _server != null)
            _server.Stop();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _close = true;
        Application.Exit();
    }

    private void MenuClients_Opening(object sender, CancelEventArgs e)
    {
        banToolStripMenuItem.Enabled = lvClients.SelectedItems.Count == 1;
        if (lvClients.SelectedItems.Count == 1)
        {
            long clientId = long.Parse(lvClients.SelectedItems[0].Text);
            banToolStripMenuItem.Text = _server.ClientBlockStatus(clientId) ? "Unblock" : "Block";
        }
    }

    private void BanToolStripMenuItem_Click(object sender, EventArgs e)
    {
        long clientId = long.Parse(lvClients.SelectedItems[0].Text);
        _server.BlockClient(clientId);
    }

    private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Show();
    }


    private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
    {
        Show();
    }
}
