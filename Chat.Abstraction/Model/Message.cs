namespace Chat.Abstraction.Model;

public class Message
{
    public long From { get; set; }
    public long To { get; set; }
    public string Content { get; set; }
    public byte[] EncryptContent { get; set; }
}
