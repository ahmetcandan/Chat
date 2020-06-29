using Chat.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    public static class Session
    {
        public static Chat.Core.Client.ChatClient Client;
        public static bool HasConnection = false;
        public static List<ClientItem> Clients = new List<ClientItem>();
    }
}
