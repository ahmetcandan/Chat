using Chat.Abstraction.Model;

namespace Chat.ClientApp;

public partial class frmMessage : Form
{
    private readonly ClientItem _toClient;
    public frmMessage(ClientItem toClient)
    {
        InitializeComponent();
        _toClient = toClient;
        Text = $"{toClient.Nick}";
    }

    private void TxtMessage_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == 13)
            SendMessage();
    }

    private void SendMessage()
    {
        if (!string.IsNullOrEmpty(txtMessage.Text) && Session.Client.SendMessage(txtMessage.Text, _toClient.ClientId))
            txtMessage.Clear();
    }

    private void BtnSendMessage_Click(object sender, EventArgs e)
    {
        SendMessage();
    }

    public void ReceivedMessage(Abstraction.Model.Message message, DateTime date)
    {
        var fromClient = Session.Clients.First(c => c.ClientId == message.From);
        txtMessages.Text += $@"{fromClient.Nick}: {message.Content} [{date.ToShortTimeString()}]{Environment.NewLine}";
    }

    private void TxtMessages_TextChanged(object sender, EventArgs e)
    {
        txtMessages.SelectionStart = txtMessages.Text.Length;
        txtMessages.ScrollToCaret();
    }
}
