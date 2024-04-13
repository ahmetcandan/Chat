using Chat.Abstraction.Model;

namespace Chat.Abstraction.Event;

public delegate void dgConnectionClosed();
public delegate void dgNewMessageReceived(MessageReceivingArguments e);
public delegate void dgNewClientConnected(ClientItem client);
public delegate void dgNewClientDisconnected(ClientItem client);
public delegate void dgClientListRefresh(ClientListResponse clients);
public delegate void dgServerStopped();

public class MessageReceivingArguments(Message message) : EventArgs
{
    public Message GetMessage()
    { return Message; }
    public void SetMessage(Message value)
    { Message = value; }

    public Message Message { get; set; } = message;

    public DateTime Date { get; } = DateTime.Now;
}
