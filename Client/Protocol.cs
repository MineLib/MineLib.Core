using System;
using System.Threading.Tasks;

using Aragas.Core.PacketHandlers;
using Aragas.Core.Packets;

using MineLib.Core.Events;

namespace MineLib.Core.Client
{
    public abstract class Protocol : IPacketHandlerContext, IDisposable
    {
        protected MineLibClient Client { get; }

        public abstract string Name { get; }
        public abstract string Version { get; }
        public abstract int NetworkVersion { get; }
        
        public abstract string Host { get; }
        public abstract ushort Port { get; }
        public abstract bool Connected { get; }
        public ClientState State { get; protected set; }


        protected Protocol(MineLibClient client, ProtocolPurpose purpose)
        {
            Client = client;
            switch (purpose)
            {
                case ProtocolPurpose.Play:
                    State = ClientState.Joining;
                    break;
                case ProtocolPurpose.InfoRequest:
                    State = ClientState.InfoRequest;
                    break;
            }
        }


        public abstract IStatusClient CreateStatusClient();

        public abstract Task<bool> Login(string login, string password);
        public abstract Task<bool> Logout();

        public abstract void Connect(ServerInfo serverInfo);
        public abstract void Disconnect();

        public abstract void RegisterSending<TSendingType>(Action<TSendingType> func) where TSendingType : SendingEvent;
        public abstract void DeregisterSending<TSendingType>(Action<TSendingType> func) where TSendingType : SendingEvent;
        public abstract void FireEvent<TSendingType>(TSendingType args) where TSendingType : SendingEvent;

        public abstract void RegisterCustomReceiving<TPacketType>(Action<TPacketType> func) where TPacketType : ProtobufPacket;
        public abstract void DeregisterCustomReceiving<TPacketType>(Action<TPacketType> func) where TPacketType : ProtobufPacket;
        public abstract void DoCustomReceiving<TPacketType>(TPacketType protobufPacket) where TPacketType : ProtobufPacket;

        public abstract void Dispose();
    }
}
