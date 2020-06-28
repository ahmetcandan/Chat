using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core
{
    public delegate void dgConnectionClosed();
    public delegate void dgNewMessageReceived(MessageReceivingArguments e);
    public delegate void dgNewClientConnected(ClientItem client);
    public delegate void dgNewClientDisconnected(ClientItem client);
    public delegate void dgClientListRefresh(List<ClientItem> clients);

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
            this.Message = message;
        }
    }

    public class ClientItem
    {
        public long ClientId { get; set; }
        public string Nick { get; set; }
        public string IPAddress { get; set; }
    }
}
