using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core
{
    public class Command
    {
        public Cmd Cmd { get; set; }
        public string Content { get; set; }
    }

    public enum Cmd
    {
        Message = 1,
        Login = 2,
        Logout = 3,
        SetNick = 4,
        Command = 5,
        UserList = 6
    }
}
