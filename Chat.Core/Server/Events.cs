using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Server
{
    public delegate void dgNewClientConnected(ClientConnectionArguments e);
    public delegate void dgClientConnectionClosed(ClientConnectionArguments e);
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
        }
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
        
        public ClientSendMessageArguments(IClient client, Message message)
        {
            this.client = client;
            this.message = message;
        }
    }
}
