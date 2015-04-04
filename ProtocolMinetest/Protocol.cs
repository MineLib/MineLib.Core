using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using MineLib.Network;
using MineLib.Network.IO;
using ProtocolMinetest.IO;

namespace ProtocolMinetest
{
    public partial class Protocol : IProtocol
    {
        #region Properties

        public string Name { get { return "Minetest"; } }
        public string Version { get { return "None"; } }

        public ConnectionState State { get; set; }

        public bool Connected { get { return _baseSock != null && _baseSock.Client.Connected; } }

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

        private IMinecraftClient _minecraft;

        private UdpClient _baseSock;
        private IProtocolStream _stream;


        public IProtocol Initialize(IMinecraftClient client, bool debugPackets = false)
        {
            _minecraft = client;
            SavePackets = debugPackets;

            PacketsReceived = new List<IPacket>();
            PacketsSended = new List<IPacket>();

            return this;
        }

        private void PacketReceiverAsync(IAsyncResult result)
        {
            if (!Connected)
                return;

            var remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 8000);

            var buffer = _baseSock.EndReceive(result, ref remoteIpEndPoint);

            _baseSock.BeginReceive(PacketReceiverAsync, null);
        }

        /// <summary>
        /// Packets are handled here. Compression and encryption are handled here too
        /// </summary>
        /// <param name="id">Packet ID</param>
        /// <param name="data">Packet byte[] data</param>
        private void HandlePacket(int id, byte[] data)
        {
            using (var reader = new MinetestDataReader(data))
            {
                IPacket packet = null;

                switch (State)
                {
                    #region Status

                    case ConnectionState.InfoRequest:
                        //if (ServerResponse.Status[id] == null)
                        //    throw new ProtocolException("Reading error: Wrong Status packet ID.");

                        //packet = ServerResponse.Status[id]().ReadPacket(reader);

                        OnPacketHandled(id, packet, ConnectionState.InfoRequest);
                        break;

                    #endregion Status

                    #region Login

                    case ConnectionState.JoiningServer:
                        //if (ServerResponse.Login[id] == null)
                        //    throw new ProtocolException("Reading error: Wrong Login packet ID.");

                        //packet = ServerResponse.Login[id]().ReadPacket(reader);

                        OnPacketHandled(id, packet, ConnectionState.JoiningServer);
                        break;

                    #endregion Login

                    #region Play

                    case ConnectionState.JoinedServer:
                        //if (ServerResponse.Play[id] == null)
                        //    throw new ProtocolException("Reading error: Wrong Play packet ID.");

                        //packet = ServerResponse.Play[id]().ReadPacket(reader);

                        OnPacketHandled(id, packet, ConnectionState.JoinedServer);                   
                        break;

                    #endregion Play
                }

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
        
        
        public IAsyncResult BeginConnect(string ip, ushort port, AsyncCallback asyncCallback, object state)
        {
            if (Connected)
                throw new ProtocolException("Connection error: Already connected to server.");

            // -- Connect to server.
            _baseSock = new UdpClient();

            var result = _baseSock.Client.BeginConnect(ip, port, asyncCallback, state);
            EndConnect(result);


            return result;
        }

        public void EndConnect(IAsyncResult asyncResult)
        {
            _baseSock.Client.EndConnect(asyncResult);

            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            _baseSock.BeginReceive(PacketReceiverAsync, null);
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
            if(!Connected)
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
            _baseSock = new UdpClient();
            _baseSock.Connect(ip, port);

            _baseSock.BeginReceive(PacketReceiverAsync, null);
        }


        public void Disconnect()
        {
            if(!Connected)
                throw new ProtocolException("Connection error: Not connected to a Minecraft Server.");

            _baseSock.Close();
        }

        #endregion


        public void Dispose()
        {
            if(_baseSock != null)
                _baseSock.Close();

            if(_stream != null)
                _stream.Dispose();

            if (PacketsReceived != null)
                PacketsReceived.Clear();

            if (PacketsSended != null)
                PacketsSended.Clear();
        }
    }
}
