using NamedPipeServerWService.NamedPipeServer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NamedPipeServerWService.NamedPipeServer
{
    public class NamedPipeServer : NamedPipeServerBase<NamedPipeServerStream>, INamedPipeServer
    {
        public event EventHandler ServerStarted;
        public event EventHandler ClientConnected;

        public NamedPipeServer(string name)
            : base(name)
        {
        }

        
        private void OnClientConnected()
        {
            ClientConnected?.Invoke(this, EventArgs.Empty);
        }

        
        private void OnServerStarted()
        {
            ServerStarted?.Invoke(this, EventArgs.Empty);
        }

        public async Task Start()
        {

            PipeSecurity ps = new PipeSecurity();
            ps.AddAccessRule(new PipeAccessRule("Users", PipeAccessRights.ReadWrite | PipeAccessRights.CreateNewInstance, AccessControlType.Allow));
            ps.AddAccessRule(new PipeAccessRule("CREATOR OWNER", PipeAccessRights.FullControl, AccessControlType.Allow));
            ps.AddAccessRule(new PipeAccessRule("SYSTEM", PipeAccessRights.FullControl, AccessControlType.Allow));
            

            //Initialize(new NamedPipeServerStream(_name, PipeDirection.InOut, 1,
                    //PipeTransmissionMode.Message, PipeOptions.Asynchronous));

            Initialize(new NamedPipeServerStream(_name, PipeDirection.InOut, 10,
                                    PipeTransmissionMode.Message, PipeOptions.Asynchronous, 1024, 1024, ps));

            try
            {
                Pipe.BeginWaitForConnection(WaitForConnectionCallBack, null);

                OnServerStarted();
            }
            catch (Exception ex)
            {
                NamedPipeServerManager.Instance.WriteLogs(ex.ToString());
            }
        }

        private void WaitForConnectionCallBack(IAsyncResult result)
        {
            Pipe.EndWaitForConnection(result);
            OnClientConnected();

            StartReading().GetAwaiter().GetResult();
        }

        public override void Dispose()
        {
            Pipe?.Disconnect();
            Pipe?.Dispose();
        }
    }
}
