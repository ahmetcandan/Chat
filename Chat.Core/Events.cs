using Chat.Core.Cryptography;
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

    public class ClientItem
    {
        public ClientItem()
        {

        }

        public ClientItem(long ClientId, string Nick, string IPAddress, string PublicKey)
        {
            this.ClientId = ClientId;
            this.Nick = Nick;
            this.IPAddress = IPAddress;
            this.publickey = PublicKey;
        }

        public long ClientId { get; set; }
        public string Nick { get; set; }
        public string IPAddress { get; set; }
        public string PublicKey { get { return publickey; } set { publickey = value; } }
        private string publickey;
    }

    public class ClientListResponse
    {
        public List<ClientItem> Clients { get; set; }
        public ClientItem Client { get; set; }
        public ClientItem ProcessClient { get; set; }
        public ClientEvent ClientEvent { get; set; }
    }
}
