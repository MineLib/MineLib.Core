using System;
using System.Collections.Generic;
using System.Net.Sockets;
using MineLib.Network;
using MineLib.Network.IO;
using ProtocolClassic.IO;
using ProtocolClassic.Packets;

namespace ProtocolClassic
{
    public partial class Protocol : IProtocol
    {
        #region Properties

        public string Name { get { return "Classic"; } }
        public string Version { get { return "0.30"; } }

        public ConnectionState State { get; set; }

        public bool Connected { get { return _baseSock != null && _baseSock.Connected; } }

        public bool UseLogin { get; private set; }

        // -- Debugging
        public bool SavePackets { get; private set; }

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
        // -- Debugging

        #endregion

        private bool UsingExtensions { get; set; }

        private IMinecraftClient _minecraft;

        private TcpClient _baseSock;
        private IProtocolStream _stream;


        public IProtocol Initialize(IMinecraftClient client, bool debugPackets = false)
        {
            _minecraft = client;
            SavePackets = debugPackets;

            PacketsReceived = new List<IPacket>();
            PacketsSended = new List<IPacket>();

            AsyncSendingHandlers = new Dictionary<Type, Func<IAsyncSendingParameters, IAsyncResult>>();
            RegisterSupportedAsyncSendings();

            return this;
        }

        
        private void PacketReceiverAsync(IAsyncResult ar)
        {
            if (!Connected)
                return; // -- Terminate cycle

            if (_baseSock.Available > 0)
            {
                var packetId = _stream.ReadByte();

                // Connection lost
                if (packetId == 255)
                {
                    Disconnect();
                    return;
                }

                var length = ServerResponseClassic.ServerResponse[packetId]().Size;
                var data = _stream.ReadByteArray(length - 1);

                HandlePacket(packetId, data);
            }

            _stream.EndRead(ar);
            _stream.BeginRead(new byte[0], 0, 0, PacketReceiverAsync, null);
        }

        /// <summary>
        /// Packets are handled here.
        /// </summary>
        /// <param name="id">Packet ID</param>
        /// <param name="data">Packet byte[] data</param>
        private void HandlePacket(int id, byte[] data)
        {
            using (var reader = new ClassicDataReader(data))
            {
                if (ServerResponseClassic.ServerResponse[id] == null)
                    return;

                var packet = ServerResponseClassic.ServerResponse[id]().ReadPacket(reader);

                OnPacketHandled(id, packet, null);

                if (SavePackets)
                    PacketsReceived.Add(packet);
            }
        }


        #region Network

        public IAsyncResult BeginSendPacketHandled(IPacket packet, AsyncCallback asyncCallback, object state)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            IAsyncResult result = BeginSendPacket(packet, asyncCallback, state);
            EndSendPacket(result);

            if (SavePackets)
                PacketsSended.Add(packet);

            return result;
        }

        public IAsyncResult BeginSendPacket(IPacket packet, AsyncCallback asyncCallback, object state)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            return _stream.BeginSendPacket(packet, asyncCallback, state);
        }

        public void EndSendPacket(IAsyncResult asyncResult)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            _stream.EndSend(asyncResult);
        }

        /// <summary>
        /// If connected, don't call EndConnect.
        /// </summary>
        public IAsyncResult BeginConnect(string ip, ushort port, AsyncCallback asyncCallback, object state)
        {
            if (Connected)
                throw new ProtocolException("Connection error: Already connected to server.");

            // -- Connect to server.
            _baseSock = new TcpClient();

            var result = _baseSock.BeginConnect(ip, port, asyncCallback, state);
            EndConnect(result);

            return result;
        }

        private void EndConnect(IAsyncResult asyncResult)
        {
            _baseSock.EndConnect(asyncResult);

            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            // -- Create our Wrapped socket.
            _stream = new ClassicStream(new NetworkStream(_baseSock.Client));

            // -- Begin data reading.
            _stream.BeginRead(new byte[0], 0, 0, PacketReceiverAsync, null);
        }


        public IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, object state)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            return _baseSock.Client.BeginDisconnect(false, asyncCallback, state);
        }

        public void EndDisconnect(IAsyncResult asyncResult)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            _baseSock.Client.EndDisconnect(asyncResult);
        }


        public void SendPacket(IPacket packet)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            _stream.SendPacket(packet);

            if (SavePackets)
                PacketsSended.Add(packet);
        }

        public void Connect(string ip, ushort port)
        {
            if (Connected)
                throw new ProtocolException("Connection error: Already connected to server.");

            // -- Connect to server.
            _baseSock = new TcpClient();
            _baseSock.Connect(ip, port);

            // -- Create our Wrapped socket.
            _stream = new ClassicStream(new NetworkStream(_baseSock.Client));

            // -- Begin data reading.
            _stream.BeginRead(new byte[0], 0, 0, PacketReceiverAsync, null);
        }

        public void Disconnect()
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            _baseSock.Client.Disconnect(false);
        }

        #endregion


        public void Dispose()
        {
            if (_baseSock != null)
                _baseSock.Client.Dispose();

            if (_stream != null)
                _stream.Dispose();

            if (PacketsReceived != null)
                PacketsReceived.Clear();

            if (PacketsSended != null)
                PacketsSended.Clear();
        }
    }
}
