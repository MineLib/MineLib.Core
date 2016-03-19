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

        public abstract bool UseLogin { get; }

        public abstract Task<bool> Login(string login, string password);
        public abstract Task<bool> Logout();

        #endregion Login

        #region Debug

        protected abstract List<ProtobufPacket> PacketsReceived { get; }
        protected abstract List<ProtobufPacket> PacketsSended { get; }
        protected abstract List<ProtobufPacket> LastPackets { get; }
        protected abstract ProtobufPacket LastProtobufPacket { get; }
        public abstract bool SavePackets { get; }

        #endregion Debug

        public abstract string Name { get; }
        public abstract string Version { get; }

        /// <summary>
        /// Current state of IProtocol client/server connection.
        /// </summary>
        public abstract ConnectionState State { get; protected set; }


        public abstract string Host { get; }
        public abstract ushort Port { get; }
        public abstract bool Connected { get; }

        protected Protocol(MineLibClient client, bool debugPackets = false) { }

        public abstract void Connect(string host, ushort port);
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
