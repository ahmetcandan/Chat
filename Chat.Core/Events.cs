using System;

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

        public Message Message { get; set; }

        public MessageReceivingArguments(Message message)
        {
            Message = message;
            Date = DateTime.Now;
        }

        public DateTime Date { get; }
    }
}
