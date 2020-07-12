using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Server
{
    public interface IClient
    {
        long ClientId { get; }
        string Nick { get; }
        string IPAddress { get; }
        bool HasConnection { get; }
        bool SendMessage(string mesaj);
        void CloseConnection();
        string PublicKey { get; }
        ClientStatus Status { get; }
        void SetStatus(ClientStatus status);
    }
}
