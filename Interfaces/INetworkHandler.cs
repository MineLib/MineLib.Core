using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MineLib.Core.Loader;

namespace MineLib.Core.Interfaces
{
    public interface INetworkHandlerAsync
    {
        Task ConnectAsync(String ip, UInt16 port);

        Boolean DisconnectAsync();
    }


    /// <summary>
    /// Proxy that loads defined IProtocol and handles it.
    /// </summary>
    public interface INetworkHandler : INetworkHandlerAsync, IDisposable
    {
        #region Properties

        ProtocolType NetworkMode { get; }
        ConnectionState ConnectionState { get; }

        Boolean UseLogin { get; }

        Boolean SavePackets { get; }

        Boolean Connected { get; }

        #endregion

        /// <summary>
        /// Get all Modules that INetworkHandler can use.
        /// </summary>
        /// <returns></returns>
        List<ProtocolAssembly> GetModules();

        /// <summary>
        /// "Constructor". Interface cannot use a real constructor. 
        /// </summary>
        /// <param name="client">IMinecraftClient that INetworkHandler will interact.</param>
        /// <param name="module">Which protocol type INetworkHandler will use.</param>
        /// <param name="debugPackets">Should INetworkHandler save IPacket information.</param>
        /// <returns></returns>
        INetworkHandler Initialize(IMinecraftClient client, ProtocolAssembly module, Boolean debugPackets = false);

        void Connect(String host, UInt16 port);
        void Disconnect();

        void DoSending(Type sendingType, SendingArgs args);
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