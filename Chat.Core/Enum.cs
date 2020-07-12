using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core
{
    public enum Cmd
    {
        Message = 1,
        Login = 2,
        Logout = 3,
        SetNick = 4,
        Command = 5,
        UserList = 6,
        ServerStop = 7,
        Block = 8,
        Unblock = 9,
        SetStatus = 10
    }

    public enum ClientEvent
    {
        Login = 2,
        Logout = 3,
        Refresh = 10
    }

    public enum ClientStatus
    {
        Available = 1,
        Busy = 2,
        Away = 3,
        DoNotDisturb = 4,
        Invisible = 5
    }
}
