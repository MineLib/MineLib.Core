using System;
using System.Collections.Generic;

using MineLib.Network.IO;
using MineLib.Network.Module;

namespace MineLib.Network
{
    /// <summary>
    /// Any Minecraft protocol can be implemented by using this. And even more.
    /// </summary>
    public interface IProtocol : IModule, IProtocolLogin, IProtocolDebug, IProtocolAsyncConnection, IDisposable
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
        void SendPacket(IPacket packet);

        void Connect(String ip, UInt16 port);
        void Disconnect();

        
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

    public interface IProtocolAsyncConnection
    {
        IAsyncResult BeginSendPacketHandled(IPacket packet, AsyncCallback asyncCallback, Object state);
        IAsyncResult BeginSendPacket(IPacket packet, AsyncCallback asyncCallback, Object state);
        void EndSendPacket(IAsyncResult asyncResult);

        IAsyncResult BeginConnect(String ip, UInt16 port, AsyncCallback asyncCallback, Object state);
        //void EndConnect(IAsyncResult asyncResult);

        IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, Object state);
        void EndDisconnect(IAsyncResult asyncResult);

        void RegisterAsyncSending(Type asyncSendingType, Func<IAsyncSendingArgs, IAsyncResult> method);
        IAsyncResult DoAsyncSending(Type asyncSendingType, IAsyncSendingArgs parameters);
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
