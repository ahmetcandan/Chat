using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Server
{
    public delegate void dgClientConnected(ClientConnectionArguments e);
    public delegate void dgClientDisconnected(ClientConnectionArguments e);
    public delegate void dgNewMessageReceivedFromClient(ClientSendMessageArguments e);

    public class ClientConnectionArguments : EventArgs
    {
        public IClient Client
        {
            get { return client; }
        }
        private IClient client;

        public ClientConnectionArguments(IClient client)
        {
            this.client = client;
            date = DateTime.Now;
        }

        public DateTime Date { get { return date; } }
        private DateTime date;
    }

    public class ClientSendMessageArguments : EventArgs
    {
        public IClient Client
        {
            get { return client; }
        }
        private IClient client;

        public Message Message
        {
            get { return message; }
            set { message = value; }
        }
        private Message message;

        public DateTime Date { get { return date; } }
        private DateTime date;

        public ClientSendMessageArguments(IClient client, Message message)
        {
            this.client = client;
            this.message = message;
            date = DateTime.Now;
        }
    }
}
