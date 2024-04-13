using Chat.Abstraction.Enum;

namespace Chat.Abstraction.Helper;

public static class ExtendedClass
{
    public static string ClientStatusToString(this ClientStatus status)
    {
        return status switch
        {
            ClientStatus.Available => "Available",
            ClientStatus.Busy => "Busy",
            ClientStatus.Away => "Away",
            ClientStatus.DoNotDisturb => "Do Not Disturb",
            ClientStatus.Invisible => "Invisible",
            _ => "Unknow",
        };
    }
}
