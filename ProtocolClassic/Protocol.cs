using System;
using System.Collections.Generic;
using System.Net.Sockets;
using MineLib.Network;
using ProtocolClassic.IO;
using ProtocolClassic.Packets;

namespace ProtocolClassic
{
    public partial class Protocol : IProtocol
    {
        #region Properties

        public string Name { get { return "Classic"; } }
        public string Version { get { return "0.30"; } }

        public ConnectionState ConnectionState { get; set; }
        public IProtocolAsyncReceiver PacketReceiver { get; set; }

        public bool Connected { get { return _baseSock != null && _baseSock.Connected; } }

        // -- Debugging
        public List<IPacket> PacketsReceived { get; private set; }
        public List<IPacket> PacketsSended { get; private set; }

        public List<IPacket> LastPackets
        {
            get
            {
                try { return PacketsReceived.GetRange(PacketsReceived.Count - 50, 50); }
                catch { return null; }
            }
        }
        public IPacket LastPacket { get { return PacketsReceived[PacketsReceived.Count - 1]; } }

        public bool SavePackets { get; private set; }
        // -- Debugging

        #endregion

        private IMinecraftClient _minecraft;

        private Socket _baseSock;
        private MinecraftStream _stream;


        public IProtocol Create(IMinecraftClient client, bool debugPackets = false)
        {
            _minecraft = client;
            SavePackets = debugPackets;

            PacketsReceived = new List<IPacket>();
            PacketsSended = new List<IPacket>();

            return this;
        }

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
            if (!Connected)
                return;

            var packetId = _stream.ReadByte();

            // Connection lost
            if (packetId == 255)
                Disconnect();

            var length = ServerResponseClassic.ServerResponse[packetId]().Size;
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
                if (ServerResponseClassic.ServerResponse[id] == null)
                    return;

                var packet = ServerResponseClassic.ServerResponse[id]().ReadPacket(reader);

                RaisePacketHandledClassic(id, packet, null);
            }
        }


        #region Network

        public IAsyncResult BeginSendPacketHandled(IPacket packet, AsyncCallback asyncCallback, object state)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to a Minecraft Server.");

            IAsyncResult result = BeginSendPacket(packet, asyncCallback, state);
            EndSendPacket(result);

            if (SavePackets)
                PacketsSended.Add(packet);

            return result;
        }

        public IAsyncResult BeginSendPacket(IPacket packet, AsyncCallback asyncCallback, object state)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to a Minecraft Server.");

            return _stream.BeginSendPacket(packet, asyncCallback, state);
        }

        public void EndSendPacket(IAsyncResult asyncResult)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to a Minecraft Server.");

            _stream.EndSend(asyncResult);
        }

        /// <summary>
        /// If connected, don't call EndConnect.
        /// </summary>
        public IAsyncResult BeginConnect(string ip, short port, AsyncCallback asyncCallback, object state)
        {
            if (Connected)
                throw new ProtocolException("Connection error: Already connected to a Minecraft Server.");

            // -- Connect to server.
            _baseSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var result = _baseSock.BeginConnect(_minecraft.ServerHost, _minecraft.ServerPort, asyncCallback, state);
            EndConnect(result);


            return result;
        }

        public void EndConnect(IAsyncResult asyncResult)
        {
            _baseSock.EndConnect(asyncResult);

            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to a Minecraft Server.");

            // -- Create our Wrapped socket.
            _stream = new MinecraftStream(new NetworkStream(_baseSock));

            // -- Begin data reading.
            _stream.BeginRead(new byte[0], 0, 0, PacketReceiverAsync, null);
        }


        public IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, object state)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to a Minecraft Server.");

            return _baseSock.BeginDisconnect(false, asyncCallback, state);
        }

        public void EndDisconnect(IAsyncResult asyncResult)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to a Minecraft Server.");

            _baseSock.EndDisconnect(asyncResult);
        }


        public void SendPacket(IPacket packet)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to a Minecraft Server.");

            _stream.SendPacket(packet);

            if (SavePackets)
                PacketsSended.Add(packet);
        }

        public void Connect()
        {
            if (Connected)
                throw new ProtocolException("Connection error: Already connected to a Minecraft Server.");

            // -- Connect to server.
            _baseSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _baseSock.Connect(_minecraft.ServerHost, _minecraft.ServerPort);

            // -- Create our Wrapped socket.
            _stream = new MinecraftStream(new NetworkStream(_baseSock));

            // -- Begin data reading.
            _stream.BeginRead(new byte[0], 0, 0, PacketReceiverAsync, null);
        }

        public void Disconnect()
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to a Minecraft Server.");

            _baseSock.Disconnect(false);
        }

        #endregion


        public void Dispose()
        {
            if (_baseSock != null)
                _baseSock.Dispose();

            if (_stream != null)
                _stream.Dispose();

            if (PacketsReceived != null)
                PacketsReceived.Clear();

            if (PacketsSended != null)
                PacketsSended.Clear();
        }
    }
}
