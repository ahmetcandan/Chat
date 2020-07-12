using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core
{
    public class Command
    {
        public Cmd Cmd { get; set; }
        public string Content { get; set; }
    }

    public class ClientItem
    {
        public ClientItem()
        {

        }

        public ClientItem(long ClientId, string Nick, string IPAddress, string PublicKey, ClientStatus status)
        {
            this.ClientId = ClientId;
            this.Nick = Nick;
            this.IPAddress = IPAddress;
            this.publickey = PublicKey;
            this.status = status;
        }

        public long ClientId { get; set; }
        public string Nick { get; set; }
        public string IPAddress { get; set; }
        public string PublicKey { get { return publickey; } set { publickey = value; } }
        private string publickey;
        private ClientStatus status;
        public ClientStatus Status { get { return status; } set { status = value; } }
    }

    public class ClientListResponse
    {
        public List<ClientItem> Clients { get; set; }
        public ClientItem Client { get; set; }
        public ClientItem ProcessClient { get; set; }
        public ClientEvent ClientEvent { get; set; }
    }
}
