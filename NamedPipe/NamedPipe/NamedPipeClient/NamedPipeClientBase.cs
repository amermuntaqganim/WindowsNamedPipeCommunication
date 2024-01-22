using NamedPipeClientWPF.NamedPipeClient.EventArguments;
using NamedPipeClientWPF.NamedPipeClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipeClientWPF.NamedPipeClient
{
    public abstract class NamedPipeClientBase<T> : IPipeConnection
        where T : PipeStream
    {
        protected readonly string _name;
        protected T Pipe;
        private StringStreamMessage _stream;

        public NamedPipeClientBase(string pipeName)
        {
            _name = pipeName;
        }

        public event EventHandler Disconnected;
        private void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<ReceiveMessageEventArgs> MessageReceived;
        private void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(this, new ReceiveMessageEventArgs(message));
        }

        protected void Initialize(T pipeStream)
        {
            Pipe = pipeStream;
            _stream = new StringStreamMessage(pipeStream);
        }

        protected async Task StartReading()
        {
            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    while (true)
                    {
                        var message = await _stream.ReadString();
                        OnMessageReceived(message);
                    }
                }
                catch (InvalidOperationException)
                {
                    OnDisconnected();
                    Dispose();
                }
            });
        }

        public async Task Send(string message)
        {
            await _stream.WriteString(message);
        }

        public abstract void Dispose();
    }
}
