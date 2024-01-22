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
            WriteLogs($"SERVER => Message received from client:" + e.Message);
        }

        private void ServerDisconnected(object sender, EventArgs e)
        {
            WriteLogs($"SERVER => A client disconnected.");
        }

        private void ClientConnected(object sender, EventArgs e)
        {
            WriteLogs("SERVER => A client connected.");

            StartTimer();
        }

        private void ServerStarted(object sender, EventArgs e)
        {
            WriteLogs("SERVER => Server started.");
        }

        public void StopServer() 
        {
            server.Dispose();
        }

        public void WriteLogs(string logmessage)
        {
            string path = "C:\\NamedPipeServerLogs.txt";
            using (StreamWriter sw = new StreamWriter(File.Open(path, System.IO.FileMode.Append)))
            {
                sw.WriteLineAsync(logmessage);
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
