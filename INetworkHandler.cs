using System;
using System.Collections.Generic;

using MineLib.Network.IO;
using MineLib.Network.Module;

namespace MineLib.Network
{
    public interface INetworkHandler : IDisposable
    {
        #region Properties

        ProtocolType NetworkMode { get; }
        ConnectionState ConnectionState { get; }

        Boolean UseLogin { get; }

        Boolean SavePackets { get; }

        Boolean Connected { get; }

        #endregion

        event LoadAssembly LoadAssembly;
        event Storage GetStorage;

        /// <summary>
        /// Get all Modules that INetworkHandler can use.
        /// </summary>
        /// <returns></returns>
        List<ProtocolModule> GetModules();

        /// <summary>
        /// "Constructor". Interface cannot use a real constructor. 
        /// </summary>
        /// <param name="module">Which protocol type INetworkHandler will use.</param>
        /// <param name="client">IMinecraftClient that INetworkHandler will interact.</param>
        /// <param name="tcp">INetworkTCP client that INetworkHandler will use.</param>
        /// <param name="debugPackets">Should INetworkHandler save IPacket information.</param>
        /// <returns></returns>
        INetworkHandler Initialize(ProtocolModule module, IMinecraftClient client, INetworkTCP tcp, Boolean debugPackets = false);

        /// <summary>
        /// EndConnect is called automatically by IProtocol.
        /// </summary>
        IAsyncResult BeginConnect(String ip, UInt16 port, AsyncCallback asyncCallback, Object state);
        //void EndConnect(IAsyncResult asyncResult);

        IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, Object state);
        void EndDisconnect(IAsyncResult asyncResult);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncSendingType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IAsyncResult DoAsyncSending(Type asyncSendingType, IAsyncSendingArgs parameters);

        void Connect(String ip, UInt16 port);
        void Disconnect();
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