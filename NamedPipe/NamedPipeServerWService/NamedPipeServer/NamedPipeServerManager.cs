using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamedPipeServerWService.NamedPipeServer.EventArguments;
using NamedPipeServerWService.NamedPipeServer.Interfaces;

namespace NamedPipeServerWService.NamedPipeServer
{
    public class NamedPipeServerManager
    {
        public EventHandler<string> OnClientDisconnected;
        INamedPipeServer server = null;
        public static NamedPipeServerManager Instance = Singleton<NamedPipeServerManager>.Instance;

        public async void StartServer()
        {
            WriteLogs("Start Server");
            server = new NamedPipeServer("mypipe");
            server.ServerStarted += ServerStarted;
            server.ClientConnected += ClientConnected;
            server.Disconnected += ServerDisconnected;
            server.MessageReceived += ServerMessageReceived;


            await server.Start();

            
        }

        private void ServerMessageReceived(object sender, ReceiveMessageEventArgs e)
        {
            WriteLogs($"Message received from client:" + e.Message);
        }

        private async void ServerDisconnected(object sender, EventArgs e)
        {
            WriteLogs($"A client disconnected.");
            OnClientDisconnected.Invoke(sender, "Disconnected");

        }

        private void ClientConnected(object sender, EventArgs e)
        {
            WriteLogs("A client connected to Server");

            StartTimer();
        }

        private void ServerStarted(object sender, EventArgs e)
        {
            WriteLogs("Server started.");
        }

        public void StopServer () 
        {
            
        }

        public void WriteLogs(string logmessage)
        {
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = "NamedPipeServerLogs.txt";
            string filePath = Path.Combine(directoryPath, fileName);

            using (StreamWriter sw = new StreamWriter(File.Open(filePath, System.IO.FileMode.Append)))
            {
                sw.WriteLineAsync(DateTime.Now + " : " + logmessage);
            }
        }

        public void StartTimer()
        {

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            server.Send("A Message From Server");
        }
    }
}
