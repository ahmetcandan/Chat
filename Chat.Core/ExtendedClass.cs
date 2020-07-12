using Chat.Core.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core
{
    public static class ExtendedClass
    {
        public static string ClientStatusToString(this ClientStatus status)
        {
            switch (status)
            {
                case ClientStatus.Available:
                    return "Available";
                case ClientStatus.Busy:
                    return "Busy";
                case ClientStatus.Away:
                    return "Away";
                case ClientStatus.DoNotDisturb:
                    return "Do Not Disturb";
                case ClientStatus.Invisible:
                    return "Invisible";
                default:
                    return "Unknow";
            }
        }
    }
}
