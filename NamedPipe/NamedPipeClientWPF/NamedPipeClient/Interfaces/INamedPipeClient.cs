using NamedPipeClientWPF.NamedPipeClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipeClientWPF.NamedPipeClient.Interfaces
{
    public interface INamedPipeClient : IPipeConnection
    {
        Task Connect();
        event EventHandler ConnectedToServer;
        event EventHandler ClientStarted;
    }
}
