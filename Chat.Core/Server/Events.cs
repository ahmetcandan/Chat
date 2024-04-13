using System;

namespace Chat.Core.Server
{
    public delegate void dgClientConnected(ClientConnectionArguments e);
    public delegate void dgClientDisconnected(ClientConnectionArguments e);
    public delegate void dgNewMessageReceivedFromClient(ClientSendMessageArguments e);

    public class ClientConnectionArguments : EventArgs
    {
        public IClient Client { get; }

        public ClientConnectionArguments(IClient client)
        {
            Client = client;
            Date = DateTime.Now;
        }

        public DateTime Date { get; }
    }

    public class ClientSendMessageArguments : EventArgs
    {
        public IClient Client { get; }

        public Message Message { get; set; }

        public DateTime Date { get; }

        public ClientSendMessageArguments(IClient client, Message message)
        {
            Client = client;
            Message = message;
            Date = DateTime.Now;
        }
    }
}
