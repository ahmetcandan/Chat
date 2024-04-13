using Chat.Abstraction.Model;
using Chat.Client;

namespace Chat.ClientApp;

public static class Session
{
    public static ChatClient Client;
    public static bool HasConnection = false;
    public static List<ClientItem> Clients = [];
}
