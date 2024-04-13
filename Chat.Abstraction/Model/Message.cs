namespace Chat.Abstraction.Model;

public class Message
{
    public Message(long from, long to, byte[] encryptContent)
    {
        From = from;
        To = to;
        EncryptContent = encryptContent;
    }

    public Message(long from, long to, string content)
    {
        From = from; 
        To = to; 
        Content = content;
    }

    public Message(long from, string content)
    {
        From = from;
        To = 0;
        Content = content;
    }

    public long From { get; set; }
    public long To { get; set; }
    public string Content { get; set; }
    public byte[] EncryptContent { get; set; }
}
