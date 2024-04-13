using Chat.Abstraction.Enum;

namespace Chat.Abstraction.Model;

public class Command
{
    public Command()
    {
        
    }

    public Command(Cmd cmd, string content)
    {
        Cmd = cmd;
        Content = content;
    }

    public Cmd Cmd { get; set; }
    public string Content { get; set; }
}
