using Chat.Abstraction.Enum;

namespace Chat.Abstraction.Model;

public class ClientItem(long ClientId, string Nick, string IPAddress, string PublicKey, ClientStatus status)
{
    public long ClientId { get; set; } = ClientId;
    public string Nick { get; set; } = Nick;
    public string IPAddress { get; set; } = IPAddress;
    public string PublicKey { get; set; } = PublicKey;
    public ClientStatus Status { get; set; } = status;
}
