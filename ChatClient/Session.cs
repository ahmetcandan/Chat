using Chat.Core;
using System.Collections.Generic;

namespace ChatClient
{
    public static class Session
    {
        public static Chat.Core.Client.ChatClient Client;
        public static bool HasConnection = false;
        public static List<ClientItem> Clients = new List<ClientItem>();
    }
}
