using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MineLib.Core.IO;
using MineLib.Core.Module;

namespace MineLib.Core
{
    /// <summary>
    /// Any Minecraft protocol can be implemented by using this. And even more.
    /// </summary>
    public interface IProtocol : IModule, IProtocolLogin, IProtocolDebug, IProtocolAsync, IDisposable
    {
        /// <summary>
        /// Current state of IProtocol client/server connection.
        /// </summary>
        ConnectionState State { get; set; }

        /// <summary>
        /// Current state of IProtocol network connection
        /// </summary>
        Boolean Connected { get; }

        /// <summary>
        /// "Constructor". Interface cannot use a real constructor. 
        /// </summary> 
        IProtocol Initialize(IMinecraftClient client, INetworkTCP tcp, Boolean debugPackets = false);

        /// <summary>
        /// IPacket sending function.
        /// </summary>
        void SendPacket(ref IPacket packet);

        void SendPacket(IPacket packet);

        void Connect(String ip, UInt16 port);
        void Disconnect();

        void DoSending(Type sendingType, ISendingAsyncArgs args);
    }

    public interface IProtocolLogin
    {
        /// <summary>
        /// Using some sort of online authorization mechanism.
        /// </summary>
        Boolean UseLogin { get; }

        Boolean Login(String login, String password);
        Boolean Logout();
    }

    public interface IProtocolDebug
    {
        List<IPacket> PacketsReceived { get; }
        List<IPacket> PacketsSended { get; }
        List<IPacket> LastPackets { get; }
        IPacket LastPacket { get; }
        Boolean SavePackets { get; }
    }

    public interface IProtocolAsync
    {
        Task SendPacketAsync(IPacket packet);
        Task ConnectAsync(String ip, UInt16 port);
        Boolean DisconnectAsync();

        void RegisterSending(Type sendingAsyncType, Func<ISendingAsyncArgs, Task> func);
        Task DoSendingAsync(Type sendingAsyncType, ISendingAsyncArgs args);
    }

    public class ProtocolException : Exception
    {
        public ProtocolException() : base() { }

        public ProtocolException(string message) : base(message) { }

        public ProtocolException(string format, params object[] args) : base(string.Format(format, args)) { }

        public ProtocolException(string message, Exception innerException) : base(message, innerException) { }

        public ProtocolException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }
    }
}
