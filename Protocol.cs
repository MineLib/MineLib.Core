using System;
using System.Threading.Tasks;

using Aragas.Core.PacketHandlers;
using Aragas.Core.Packets;

using MineLib.Core.Events;
using MineLib.Core.Interfaces;

namespace MineLib.Core
{
    public enum ProtocolMode { Play, Status }

    public abstract class Protocol : IPacketHandlerContext, IDisposable
    {
        protected MineLibClient Client { get; }

        #region Login

        protected bool UseLogin => Client.UseLogin;

        public abstract Task<bool> Login(string login, string password);
        public abstract Task<bool> Logout();

        #endregion Login

        public abstract string Name { get; }
        public abstract string Version { get; }
        public abstract int ProtocolVersion { get; }

        public abstract IStatusClient CreateStatusClient { get; }

        public ConnectionState State { get; protected set; }


        public abstract string Host { get; }
        public abstract ushort Port { get; }
        public abstract bool Connected { get; }


        protected Protocol(MineLibClient client, ProtocolMode mode)
        {
            Client = client;
            switch (mode)
            {
                case ProtocolMode.Play:
                    State = ConnectionState.Joining;
                    break;
                case ProtocolMode.Status:
                    State = ConnectionState.InfoRequest;
                    break;
            }
        }


        public abstract void Connect(string host, ushort port);
        public abstract void Disconnect();

        public abstract void RegisterSending<TSendingType>(Action<TSendingType> func) where TSendingType : SendingEvent;
        public abstract void DeregisterSending<TSendingType>(Action<TSendingType> func) where TSendingType : SendingEvent;
        public abstract void DoSending<TSendingType>(TSendingType args) where TSendingType : SendingEvent;

        public abstract void RegisterCustomReceiving<TPacketType>(Action<TPacketType> func) where TPacketType : ProtobufPacket;
        public abstract void DeregisterCustomReceiving<TPacketType>(Action<TPacketType> func) where TPacketType : ProtobufPacket;
        public abstract void DoCustomReceiving<TPacketType>(TPacketType protobufPacket) where TPacketType : ProtobufPacket;

        public abstract void Dispose();
    }
}
