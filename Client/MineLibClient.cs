using System;

using MineLib.Core.Events;

namespace MineLib.Core.Client
{
    public abstract class MineLibClient : IDisposable
    {
        public abstract string Name { get; }

        public string Username { get; set; }
        
        protected ProtocolHandler ProtocolHandler { get; set; }
        public string ServerHost => ProtocolHandler.Host;
        public ushort ServerPort => ProtocolHandler.Port;
        public bool Connected => ProtocolHandler.Connected;
        public ClientState State => ProtocolHandler.State;


        protected MineLibClient(string username) { Username = username; }


        public IStatusClient CreateStatusClient() => ProtocolHandler.CreateStatusClient();

        public abstract void Connect(ServerInfo serverInfo);
        public abstract void Disconnect();

        public abstract void RegisterReceiveEvent<TReceiveEvent>(Action<TReceiveEvent> func) where TReceiveEvent : ReceiveEvent;
        public abstract void DeregisterReceiveEvent<TReceiveEvent>(Action<TReceiveEvent> func) where TReceiveEvent : ReceiveEvent;
        public abstract void DoReceiveEvent<TReceiveEvent>(TReceiveEvent args) where TReceiveEvent : ReceiveEvent;

        public abstract void Dispose();
    }
}