using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core
{
    public delegate void dgConnectionClosed();
    public delegate void dgNewMessageReceived(MessageReceivingArguments e);

    public class MessageReceivingArguments : EventArgs
    {
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        private string message;

        public MessageReceivingArguments(string message)
        {
            this.message = message;
        }
    }
}
