using Chat.Abstraction.Model;

namespace Chat.ClientApp;

public partial class FrmMessage : Form
{
    private readonly ClientItem _toClient;
    public FrmMessage(ClientItem toClient)
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
        if (!string.IsNullOrEmpty(TxtMessage.Text) && Session.Client.SendMessage(TxtMessage.Text, _toClient.ClientId))
            TxtMessage.Clear();
    }

    private void BtnSendMessage_Click(object sender, EventArgs e)
    {
        SendMessage();
    }

    public void ReceivedMessage(Abstraction.Model.Message message, DateTime date)
    {
        var fromClient = Session.Clients.First(c => c.ClientId == message.From);
        TxtMessages.Text += $@"{fromClient.Nick}: {message.Content} [{date.ToShortTimeString()}]{Environment.NewLine}";
    }

    private void TxtMessages_TextChanged(object sender, EventArgs e)
    {
        TxtMessages.SelectionStart = TxtMessages.Text.Length;
        TxtMessages.ScrollToCaret();
    }
}
