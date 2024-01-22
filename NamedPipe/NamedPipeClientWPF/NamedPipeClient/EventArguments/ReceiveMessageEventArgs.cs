using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipeClientWPF.NamedPipeClient.EventArguments
{
    public class ReceiveMessageEventArgs : EventArgs
    {
        public string Message { get; }

        public ReceiveMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
