using NamedPipeServerWService.NamedPipeServer.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipeServerWService.NamedPipeServer.Interfaces
{
    public interface IPipeConnection : IDisposable
    {
        Task Send(string message);
        event EventHandler Disconnected;
        event EventHandler<ReceiveMessageEventArgs>  MessageReceived;
    }
}
