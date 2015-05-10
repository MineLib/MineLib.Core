using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MineLib.Core.IO;
using MineLib.Core.Module;

namespace MineLib.Core
{
    public interface INetworkHandlerAsync
    {
        Task ConnectAsync(String ip, UInt16 port);

        Boolean DisconnectAsync();

        Task DoSendingAsync(Type asyncSendingType, ISendingAsyncArgs parameters);
    }

    public interface INetworkHandler : INetworkHandlerAsync, IDisposable
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

        void Connect(String ip, UInt16 port);
        void Disconnect();

        void DoSending(Type sendingType, ISendingAsyncArgs args);
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