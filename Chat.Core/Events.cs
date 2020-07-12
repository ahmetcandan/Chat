using Chat.Core.Cryptography;
using Chat.Core.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core
{
    public delegate void dgConnectionClosed();
    public delegate void dgNewMessageReceived(MessageReceivingArguments e);
    public delegate void dgNewClientConnected(ClientItem client);
    public delegate void dgNewClientDisconnected(ClientItem client);
    public delegate void dgClientListRefresh(ClientListResponse clients);
    public delegate void dgServerStopped();

    public class MessageReceivingArguments : EventArgs
    {
        public Message GetMessage()
        { return Message; }
        public void SetMessage(Message value)
        { Message = value; }
        private Message message;

        public Message Message { get => message; set => message = value; }

        public MessageReceivingArguments(Message message)
        {
            Message = message;
            date = DateTime.Now;
        }

        public DateTime Date { get { return date; } }
        private DateTime date;
    }
}
