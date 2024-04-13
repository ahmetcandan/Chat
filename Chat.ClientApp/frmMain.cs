using Chat.Abstraction.Enum;
using Chat.Abstraction.Event;
using Chat.Abstraction.Helper;
using Chat.Abstraction.Model;

namespace Chat.ClientApp;

public partial class FrmMain : Form
{
    private readonly Dictionary<long, FrmMessage> _privateMessageFormList = [];
    public FrmMain()
    {
        InitializeComponent();
    }

    private void FrmMain_Load(object sender, EventArgs e)
    {
        FrmLogin frmLogin = new();
        frmLogin.ShowDialog();
        if (Session.Client == null)
        {
            _close = true;
            Application.Exit();
            return;
        }
        Text = $"Client | Chat [{Session.Client.Nick}]";
        openToolStripMenuItem.Image = Properties.Resources.line;
        Session.Client.NewMessgeReceived += new dgNewMessageReceived(NewMessageReceived);
        Session.Client.ClientListRefresh += new dgClientListRefresh(ClientListRefresh);
        Session.Client.ServerStopped += new dgServerStopped(() => Invoke(new dgServerStopped(ServerStopped)));
        TxtMessage.Focus();
    }

    private void NewMessageReceived(MessageReceivingArguments e)
    {
        Invoke(new dgNewMessageReceived(NewMessage), e);
    }

    private void ClientListRefresh(ClientListResponse response)
    {
        Invoke(new dgClientListRefresh(RefreshClientList), response);
    }

    private void ServerStopped()
    {
        openToolStripMenuItem.Image = Properties.Resources.gray;
        TxtMessage.Clear();
        TxtMessage.ReadOnly = true;
        LvClients.Items.Clear();
        BtnSendMessage.Enabled = false;
    }

    private void RefreshClientList(ClientListResponse response)
    {
        Session.Clients = response.Clients;
        LvClients.Items.Clear();
        Text = $"Client | Chat [{response.Client.Nick} -  {response.Client.Status.ClientStatusToString()}]";
        NotifyIcon1.Text = Text;
        ImageList imageList = new()
        {
            ImageSize = new Size(18, 18)
        };
        imageList.Images.Add(((int)ClientStatus.Available).ToString(), Properties.Resources.line);
        imageList.Images.Add(((int)ClientStatus.Away).ToString(), Properties.Resources.yellow);
        imageList.Images.Add(((int)ClientStatus.Busy).ToString(), Properties.Resources.red);
        imageList.Images.Add(((int)ClientStatus.DoNotDisturb).ToString(), Properties.Resources.dnd);
        imageList.Images.Add(((int)ClientStatus.Invisible).ToString(), Properties.Resources.gray);
        LvClients.SmallImageList = imageList;
        foreach (var client in response.Clients.Where(c => c.ClientId != Session.Client.ClientId))
        {
            ListViewItem item = new()
            {
                Text = client.Nick,
                ImageIndex = imageList.Images.IndexOfKey(((int)client.Status).ToString())
            };
            item.SubItems.Add(client.ClientId.ToString());
            LvClients.Items.Add(item);
        }
    }

    private void NewMessage(MessageReceivingArguments e)
    {
        if (e.Message.To == 0)
        {
            if (Session.Client.Status != ClientStatus.DoNotDisturb)
                TopMost = true;
            var fromClient = Session.Clients.First(c => c.ClientId == e.Message.From);
            TxtMessages.Text += $@"{fromClient.Nick}: {e.Message.Content} [{e.Date.ToShortTimeString()}]{Environment.NewLine}";
            if (Session.Client.Status != ClientStatus.DoNotDisturb)
                TopMost = false;
        }
        else
        {
            long clientId = e.Message.To == Session.Client.ClientId ? e.Message.From : e.Message.To;
            var clientItem = Session.Clients.First(c => c.ClientId == clientId);
            var form = OpenPriveteMessage(clientId);
            form.ReceivedMessage(e.Message, e.Date);
        }
        TxtMessages.SelectionStart = TxtMessages.Text.Length;
        TxtMessages.ScrollToCaret();
    }

    private void BtnSendMessage_Click(object sender, EventArgs e)
    {
        SendMessage();
    }

    private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!_close)
        {
            e.Cancel = true;
            Hide();
        }
        else if (Session.HasConnection)
        {
            Session.Client?.Disconnected();
        }
    }

    private void TxtMessage_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == 13)
            SendMessage();
    }

    private void SendMessage()
    {
        if (!string.IsNullOrEmpty(TxtMessage.Text) && Session.Client.SendMessage(TxtMessage.Text))
            TxtMessage.Clear();
    }

    private void LvClients_DoubleClick(object sender, EventArgs e)
    {
        if (LvClients.SelectedItems.Count == 1)
        {
            long clientId = long.Parse(LvClients.SelectedItems[0].SubItems[1].Text);
            if (clientId == Session.Client.ClientId)
                return;
            OpenPriveteMessage(clientId);
        }
    }

    private FrmMessage OpenPriveteMessage(long clientId)
    {
        var toClient = Session.Clients.First(c => c.ClientId == clientId);
        if (_privateMessageFormList.TryGetValue(clientId, out FrmMessage? value))
        {
            var form = value;
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
                form = new FrmMessage(toClient);
                _privateMessageFormList[clientId] = form;
                form.Show();
            }
            return form;
        }
        else
        {
            var form = new FrmMessage(toClient);
            _privateMessageFormList.Add(clientId, form);
            form.Show();
            return form;
        }
    }

    private bool _close = false;

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _close = true;
        Application.Exit();
    }

    private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
    {
        Show();
    }

    private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Show();
    }

    private void AvailableToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SetStatus(ClientStatus.Available);
    }

    private void BusyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SetStatus(ClientStatus.Busy);
    }

    private void DoNotDistrubToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SetStatus(ClientStatus.DoNotDisturb);
    }

    private void InvisibleToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SetStatus(ClientStatus.Invisible);
    }

    private void AwayToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SetStatus(ClientStatus.Away);
    }

    private void SetStatus(ClientStatus status)
    {
        Session.Client.SetStatus(status);
        openToolStripMenuItem.Image = status switch
        {
            ClientStatus.Available => Properties.Resources.line,
            ClientStatus.Busy => Properties.Resources.red,
            ClientStatus.Away => Properties.Resources.yellow,
            ClientStatus.DoNotDisturb => Properties.Resources.dnd,
            ClientStatus.Invisible => Properties.Resources.gray,
            _ => Properties.Resources.line,
        };
    }
}
