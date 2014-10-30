using System;
using System.Net.Sockets;
using MineLib.Network;
using MineLib.Network.Events;
using MineLib.Network.IO;
using ProtocolPocketEdition.IO;
using ProtocolPocketEdition.Packets;

namespace ProtocolPocketEdition
{
    public class Protocol : IProtocol
    {
        public event PacketHandler PacketHandled;

        public IPacketSender PacketSender { get; set; }

        public string Name { get { return "Pocket Edition"; } }
        public string Version { get { return "0.9.0"; } }

        public bool Connected { get { return _baseSock.Connected; } }
        public bool Crashed { get; private set; }

        private IMinecraftClient _minecraft;

        private Socket _baseSock;
        private MinecraftStream _stream;


        public void Connect(IMinecraftClient client)
        {
            _minecraft = client;

            // -- Connect to server.
            _baseSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _baseSock.BeginConnect(_minecraft.ServerHost, _minecraft.ServerPort, OnConnected, null);
        }

        private void OnConnected(IAsyncResult asyncResult)
        {
            _baseSock.EndConnect(asyncResult);

            // -- Create our Wrapped socket.
            _stream = new MinecraftStream(new NetworkStream(_baseSock));

            // -- Begin data reading.
            _stream.BeginRead(new byte[0], 0, 0, PacketReceiverAsync, null);
        }


        private void PacketReceiverAsync(IAsyncResult ar)
        {
            if (_baseSock == null || _stream == null || !Connected || Crashed)
                return;

            var packetId = _stream.ReadByte();

            var length = ServerResponsePocketEdition.ServerResponse[packetId]().Size;
            var data = _stream.ReadByteArray(length - 1);

            HandlePacket(packetId, data);

            _baseSock.EndReceive(ar);
            _baseSock.BeginReceive(new byte[0], 0, 0, SocketFlags.None, PacketReceiverAsync, null);
        }

        /// <summary>
        /// Packets are handled here.
        /// </summary>
        /// <param name="id">Packet ID</param>
        /// <param name="data">Packet byte[] data</param>
        private void HandlePacket(int id, byte[] data)
        {
            using (var reader = new MinecraftDataReader(data))
            {
                if (ServerResponsePocketEdition.ServerResponse[id] == null)
                    return;

                var packet = ServerResponsePocketEdition.ServerResponse[id]().ReadPacket(reader);

                PacketHandled(id, packet, null);
            }
        }


        public IAsyncResult BeginSend(IPacket packet, AsyncCallback asyncCallback, object state)
        {
            return _stream.BeginWritePacket(packet, asyncCallback, state);
        }

        public void EndSend(IAsyncResult asyncResult)
        {
            _stream.EndWrite(asyncResult);
        }


        public void Dispose()
        {
            if (_baseSock != null)
                _baseSock.Dispose();

            if (_stream != null)
                _stream.Dispose();
        }
    }
}
