using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aragas.Core.Packets;

namespace MineLib.Core.Interfaces
{
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
        List<ProtobufPacket> PacketsReceived { get; }
        List<ProtobufPacket> PacketsSended { get; }
        List<ProtobufPacket> LastPackets { get; }
        ProtobufPacket LastProtobufPacket { get; }
        Boolean SavePackets { get; }
    }

    public interface IProtocolAsync
    {
        Task ConnectAsync(String host, UInt16 port);
        Task<Boolean> DisconnectAsync();
    }

    public interface IProtocolLoginAsync
    {
        /// <summary>
        /// Using some sort of online authorization mechanism.
        /// </summary>
        Boolean UseLogin { get; }

        Task<Boolean> Login(String login, String password);
        Task<Boolean> Logout();
    }


    /// <summary>
    /// Any Minecraft protocol can be implemented by using this. And even more.
    /// </summary>
    public interface IProtocol : IProtocolLoginAsync, IProtocolDebug, IProtocolAsync, IDisposable
    {
        String Name { get; }
        String Version { get; }

        /// <summary>
        /// Current state of IProtocol client/server connection.
        /// </summary>
        ConnectionState State { get; }

        /// <summary>
        /// Current state of IProtocol network connection
        /// </summary>
        Boolean Connected { get; }

        /// <summary>
        /// "Constructor". Interface cannot use a real constructor. 
        /// </summary> 
        IProtocol Initialize(IMinecraftClient client, Boolean debugPackets = false);

        void Connect(String host, UInt16 port);
        void Disconnect();
         
        /// <summary>
        /// Only for internal use!
        /// </summary>
        void RegisterSending(Type sendingType, Func<SendingArgs, Task> func);
        /// <summary>
        /// Only for internal use!
        /// </summary>
        void DeregisterSending(Type sendingType, Func<SendingArgs, Task> func);
        void DoSending(Type sendingType, SendingArgs args);

        /// <summary>
        /// Only for internal use!
        /// </summary>
        void RegisterReceiving(Type packetType, Func<ProtobufPacket, Task> func);
        /// <summary>
        /// Only for internal use!
        /// </summary>
        void DeregisterReceiving(Type packetType, Func<ProtobufPacket, Task> func);
        /// <summary>
        /// Only for internal use!
        /// </summary>
        void DoReceiving(Type packetType, ProtobufPacket protobufPacket);
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
