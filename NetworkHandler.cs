using System;
using System.Collections.Generic;
using MineLib.Core.Events;
using MineLib.Core.Interfaces;
using MineLib.Core.Loader;

namespace MineLib.Core
{
    /// <summary>
    /// Proxy that loads defined IProtocol and handles it.
    /// </summary>
    public abstract class NetworkHandler : IDisposable
    {
        protected MineLibClient Client { get; }
        protected bool UseLogin => Client.UseLogin;

        protected Protocol Protocol { get; set; }
        public string Host => Protocol.Host;
        public ushort Port => Protocol.Port;
        public bool Connected => Protocol.Connected;
        public ConnectionState ConnectionState => Protocol.State;
        public IStatusClient CreateStatusClient => Protocol.CreateStatusClient;

        protected ProtocolAssembly ProtocolAssembly { get; }


        protected NetworkHandler(MineLibClient client, ProtocolAssembly protocolAssembly) { Client = client; ProtocolAssembly = protocolAssembly; }


        public void Connect(string host, ushort port) { Protocol.Connect(host, port); }
        public void Disconnect() { Protocol.Disconnect(); }
        
        public void DoSending(SendingEvent args) { Protocol.DoSending(args); }
        
        public virtual void Dispose() { Protocol?.Dispose(); }
    }
}