using System;

using MineLib.Core.Events;
using MineLib.Core.Loader;

namespace MineLib.Core.Client
{
    public abstract class ProtocolHandler : IDisposable
    {
        protected MineLibClient Client { get; }

        protected Protocol Protocol { get; set; }
        public string Host => Protocol.Host;
        public ushort Port => Protocol.Port;
        public bool Connected => Protocol.Connected;
        public ClientState State => Protocol.State;

        protected AssemblyInfo AssemblyInfo { get; }


        protected ProtocolHandler(MineLibClient client, AssemblyInfo assemblyInfo) { Client = client; AssemblyInfo = assemblyInfo; }


        public IStatusClient CreateStatusClient() => Protocol.CreateStatusClient();

        public void Connect(ServerInfo serverInfo) { Protocol.Connect(serverInfo); }
        public void Disconnect() { Protocol.Disconnect(); }
        
        public void FireEvent(SendingEvent args) { Protocol.FireEvent(args); }
        
        public virtual void Dispose() { Protocol?.Dispose(); }
    }
}