using Chat.Abstraction.Enum;

namespace Chat.Abstraction.Model;

public class Command
{
    public Cmd Cmd { get; set; }
    public string Content { get; set; }
}
