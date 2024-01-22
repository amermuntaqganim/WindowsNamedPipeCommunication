using NamedPipeClientWPF.NamedPipeClient.EventArguments;
using NamedPipeClientWPF.NamedPipeClient.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipeClientWPF.NamedPipeClient
{
    public class NamedPipeClientManager
    {
        public static NamedPipeClientManager Instance = Singleton<NamedPipeClientManager>.Instance;
        INamedPipeClient client = null;

        public async void StartClient()
        {
            client = new NamedPipeClient("mypipe");
            client.MessageReceived += ClientMessageReceived;
            client.ConnectedToServer += ClientConnectedToServer;
            client.ClientStarted += ClientClientStarted;
            client.Disconnected += ClientDisconnected;


            await client.Connect();
        }

        private void ClientDisconnected(object sender, EventArgs e)
        {
            WriteLogs($"CLIENT => Server disconnected.");
        }

        private void ClientClientStarted(object sender, EventArgs e)
        {
            Console.WriteLine("Client Started");
            WriteLogs("CLIENT => Client started.");
        }

        private void ClientConnectedToServer(object sender, EventArgs e)
        {
            Console.WriteLine("Connected To Server");
            WriteLogs("CLIENT => Client connected to server.");
        }

        private void ClientMessageReceived(object sender, ReceiveMessageEventArgs e)
        {
            WriteLogs("Received Message:" + e.Message);

            if (e.Message.Contains("A Message From Server"))
                client.Send("A Message From Client");
        }

        public void ConnectToServer()
        { 
        
        }

        public void SendMessageToServer(string message)
        {
            client.Send(message);
        }

        public void WriteLogs(string logmessage)
        {
            string path = "NamedPipeClientLogs.txt";
            using (StreamWriter sw = new StreamWriter(File.Open(path, System.IO.FileMode.Append, FileAccess.Write)))
            {
                sw.WriteLineAsync(DateTime.Now+" : "+logmessage);
            }
        }

    }
}
