using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Aragas.Core.Packets;

using MineLib.Core.Events;

namespace MineLib.Core.Protocols
{
    /// <summary>
    /// Any Minecraft protocol can be implemented by using this. And even more.
    /// </summary>
    public abstract class Protocol : IDisposable
    {
        #region Login

        public abstract Boolean UseLogin { get; }

        public abstract Task<Boolean> Login(String login, String password);
        public abstract Task<Boolean> Logout();

        #endregion Login

        #region Debug

        protected abstract List<ProtobufPacket> PacketsReceived { get; }
        protected abstract List<ProtobufPacket> PacketsSended { get; }
        protected abstract List<ProtobufPacket> LastPackets { get; }
        protected abstract ProtobufPacket LastProtobufPacket { get; }
        public abstract Boolean SavePackets { get; }

        #endregion Debug

        public abstract String Name { get; }
        public abstract String Version { get; }

        /// <summary>
        /// Current state of IProtocol client/server connection.
        /// </summary>
        public abstract ConnectionState State { get; protected set; }


        public abstract String Host { get; }
        public abstract UInt16 Port { get; }
        public abstract Boolean Connected { get; }

        protected Protocol(MineLibClient client, Boolean debugPackets = false) { }

        public abstract void Connect(String host, UInt16 port);
        public abstract void Disconnect();

        /// <summary>
        /// Only for internal use!
        /// </summary>
        public abstract void RegisterSending(Type sendingType, Action<SendingEventArgs> func);
        /// <summary>
        /// Only for internal use!
        /// </summary>
        public abstract void DeregisterSending(Type sendingType, Action<SendingEventArgs> func);
        public abstract void DoSending(Type sendingType, SendingEventArgs args);

        /// <summary>
        /// Only for internal use!
        /// </summary>
        public abstract void RegisterReceiving(Type packetType, Func<ProtobufPacket, Task> func);
        protected abstract void DeregisterReceiving(Type packetType, Func<ProtobufPacket, Task> func);
        protected abstract void DoReceiving(Type packetType, ProtobufPacket protobufPacket);

        public abstract void Dispose();
    }
}
