using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipeServerWService.NamedPipeServer.Interfaces
{
    public interface INamedPipeServer : IPipeConnection
    {
        Task Start();
        event EventHandler ServerStarted;
        event EventHandler ClientConnected;
    }
}
