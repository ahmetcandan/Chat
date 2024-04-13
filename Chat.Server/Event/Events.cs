using Chat.Abstraction.Model;

namespace Chat.Server.Event;

public delegate void dgClientConnected(ClientConnectionArguments e);
public delegate void dgClientDisconnected(ClientConnectionArguments e);
public delegate void dgNewMessageReceivedFromClient(ClientSendMessageArguments e);

public class ClientConnectionArguments(IClient client) : EventArgs
{
    public IClient Client { get; } = client;

    public DateTime Date { get; } = DateTime.Now;
}

public class ClientSendMessageArguments(IClient client, Message message) : EventArgs
{
    public IClient Client { get; } = client;

    public Message Message { get; set; } = message;

    public DateTime Date { get; } = DateTime.Now;
}
