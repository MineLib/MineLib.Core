using System;

using MineLib.Core.Events;
using MineLib.Core.Loader;
using MineLib.Core.Protocols;

namespace MineLib.Core
{
    /// <summary>
    /// Proxy that loads defined IProtocol and handles it.
    /// </summary>
    public abstract class NetworkHandler : IDisposable
    {
        public ProtocolType NetworkMode => Client.Mode;
        public ConnectionState ConnectionState => Protocol.State;

        public bool UseLogin => Client.UseLogin;

        public bool SavePackets => Protocol.SavePackets;

        public string Host => Protocol.Host;
        public ushort Port => Protocol.Port;
        public bool Connected => Protocol.Connected;

        protected MineLibClient Client { get; }
        protected Protocol Protocol { get; set; }


        protected NetworkHandler(MineLibClient client, ProtocolAssembly protocol, bool debugPackets = false) { Client = client; }


        //public abstract List<ProtocolAssembly> GetModules();
        
        public void Connect(string host, ushort port) { Protocol.Connect(host, port); }
        public void Disconnect() { Protocol.Disconnect(); }
        
        public void DoSending(Type sendingType, SendingEventArgs args) { Protocol.DoSending(sendingType, args); }
        
        public virtual void Dispose() { Protocol?.Dispose(); }
    }


    public class NetworkHandlerException : Exception
    {
        public NetworkHandlerException() : base() { }

        public NetworkHandlerException(string message) : base(message) { }

        public NetworkHandlerException(string format, params object[] args) : base(string.Format(format, args)) { }

        public NetworkHandlerException(string message, Exception innerException) : base(message, innerException) { }

        public NetworkHandlerException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }
    }
}