using Chat.Core;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class frmMessage : Form
    {
        private readonly ClientItem toClient;
        public frmMessage(ClientItem toClient)
        {
            InitializeComponent();
            this.toClient = toClient;
            Text = $"{toClient.Nick}";
        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                sendMessage();
        }

        private void sendMessage()
        {
            if (!string.IsNullOrEmpty(txtMessage.Text) && Session.Client.SendMessage(txtMessage.Text, toClient.ClientId))
                txtMessage.Clear();
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            sendMessage();
        }

        public void ReceivedMessage(Chat.Core.Message message, DateTime date)
        {
            var fromClient = Session.Clients.First(c => c.ClientId == message.From);
            txtMessages.Text += $@"{fromClient.Nick}: {message.Content} [{date.ToShortTimeString()}]{Environment.NewLine}";
        }

        private void txtMessages_TextChanged(object sender, EventArgs e)
        {
            txtMessages.SelectionStart = txtMessages.Text.Length;
            txtMessages.ScrollToCaret();
        }
    }
}
