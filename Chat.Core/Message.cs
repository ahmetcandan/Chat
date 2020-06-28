using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core
{
    public class Message
    {
        public long From { get; set; }
        public long To { get; set; }
        public string Content { get; set; }
    }
}
