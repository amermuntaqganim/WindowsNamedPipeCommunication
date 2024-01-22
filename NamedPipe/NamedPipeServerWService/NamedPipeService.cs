using NamedPipeServerWService.NamedPipeServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipeServerWService
{
    public partial class NamedPipeService : ServiceBase
    {
        public NamedPipeService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            NamedPipeServerManager.Instance.WriteLogs("On Start method from Service");
            NamedPipeServerManager.Instance.StartServer();

            NamedPipeServerManager.Instance.OnClientDisconnected += OnDisconnectedEvent;
        }

        private void OnDisconnectedEvent(object sender, string e)
        {
            NamedPipeServerManager.Instance.WriteLogs("Client disconnected, need to start server again");
            NamedPipeServerManager.Instance.StartServer();
        }

        protected override void OnStop()
        {
            NamedPipeServerManager.Instance.WriteLogs("On Stop Service");
        }
    }
}
