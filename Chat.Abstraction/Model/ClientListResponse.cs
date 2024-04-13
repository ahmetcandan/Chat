using Chat.Abstraction.Enum;

namespace Chat.Abstraction.Model;

public class ClientListResponse
{
    public List<ClientItem> Clients { get; set; }
    public ClientItem Client { get; set; }
    public ClientItem ProcessClient { get; set; }
    public ClientEvent ClientEvent { get; set; }
}
