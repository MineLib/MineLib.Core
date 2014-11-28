using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using MineLib.Network;
using ProtocolModern.IO;
using ProtocolModern.Packets;
using ProtocolModern.Packets.Client.Login;
using ProtocolModern.Packets.Server.Login;

namespace ProtocolModern
{
    public partial class Protocol : IProtocol
    {
        #region Properties

        public string Name { get { return "Modern"; } }
        public string Version { get { return "1.8"; } }

        public ConnectionState State { get; set; }

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

        private TcpClient _baseSock;
        private IProtocolStreamExtended _stream;

        private bool CompressionEnabled { get { return _stream != null && _stream.ModernCompressionEnabled; } }
        private long CompressionThreshold { get { return _stream == null ? -1 : _stream.ModernCompressionThreshold; } }


        public IProtocol Create(IMinecraftClient client, bool debugPackets = false)
        {
            _minecraft = client;
            SavePackets = debugPackets;

            PacketsReceived = new List<IPacket>();
            PacketsSended = new List<IPacket>();

            return this;
        }

        private void PacketReceiverAsync(IAsyncResult ar)
        {
            if (!Connected)
                return; // -- Terminate cycle

            if (_baseSock.Available > 0)
            {
                int packetId;
                byte[] data;

                #region No Compression

                if (!CompressionEnabled)
                {
                    var packetLength = _stream.ReadVarInt();
                    if (packetLength == 0)
                        throw new ProtocolException("Reading error: Packet Length size is 0");

                    packetId = _stream.ReadVarInt();

                    data = _stream.ReadByteArray(packetLength - 1);
                }

                #endregion

                #region Compression

                else // (CompressionEnabled)
                {
                    var packetLength = _stream.ReadVarInt();
                    if (packetLength == 0)
                        throw new ProtocolException("Reading error: Packet Length size is 0");

                    var dataLength = _stream.ReadVarInt();
                    if (dataLength == 0)
                    {
                        if (packetLength >= CompressionThreshold)
                            throw new ProtocolException("Reading error: Received uncompressed message of size " + packetLength +
                                " greater than threshold " + CompressionThreshold);

                        packetId = _stream.ReadVarInt();

                        data = _stream.ReadByteArray(packetLength - 2);
                    }
                    else // (dataLength > 0)
                    {
                        var dataLengthBytes = ModernStream.GetVarIntBytes(dataLength).Length;

                        var tempBuff = _stream.ReadByteArray(packetLength - dataLengthBytes); // -- Compressed

                        using (var outputStream = new MemoryStream())
                        using (var inputStream = new InflaterInputStream(new MemoryStream(tempBuff)))
                        //using (var reader = new MinecraftDataReader(new MemoryStream(tempBuff), NetworkMode)) // -- For VarInt
                        {
                            inputStream.CopyTo(outputStream);
                            tempBuff = outputStream.ToArray(); // -- Decompressed

                            packetId = tempBuff[0]; // -- Only 255 packets available. ReadVarInt doesn't work.
                            var packetIdBytes = ModernStream.GetVarIntBytes(packetId).Length;

                            data = new byte[tempBuff.Length - packetIdBytes];
                            Buffer.BlockCopy(tempBuff, packetIdBytes, data, 0, data.Length);
                        }
                    }
                }

                #endregion

                HandlePacket(packetId, data);
            }

            // -- If it will throw an error, then the cause is too slow _stream dispose
            _stream.EndRead(ar);
            _stream.BeginRead(new byte[0], 0, 0, PacketReceiverAsync, null);
        }

        /// <summary>
        /// Packets are handled here. Compression and encryption are handled here too
        /// </summary>
        /// <param name="id">Packet ID</param>
        /// <param name="data">Packet byte[] data</param>
        private void HandlePacket(int id, byte[] data)
        {
            using (var reader = new ModernDataReader(data))
            {
                IPacket packet = null;

                switch (State)
                {
                    #region Status

                    case ConnectionState.InfoRequest:
                        if (ServerResponse.InfoRequest[id] == null)
                            throw new ProtocolException("Reading error: Wrong Status packet ID.");

                        packet = ServerResponse.InfoRequest[id]().ReadPacket(reader);

                        RaisePacketHandled(id, packet, ConnectionState.InfoRequest);
                        break;

                    #endregion Status

                    #region Login

                    case ConnectionState.JoiningServer:
                        if (ServerResponse.JoiningServer[id] == null)
                            throw new ProtocolException("Reading error: Wrong Login packet ID.");

                        packet = ServerResponse.JoiningServer[id]().ReadPacket(reader);

                        RaisePacketHandled(id, packet, ConnectionState.JoiningServer);
                        break;

                    #endregion Login

                    #region Play

                    case ConnectionState.JoinedServer:
                        if (ServerResponse.JoinedServer[id] == null)
                            throw new ProtocolException("Reading error: Wrong Play packet ID.");

                        packet = ServerResponse.JoinedServer[id]().ReadPacket(reader);

                        RaisePacketHandled(id, packet, ConnectionState.JoinedServer);
                        break;

                    #endregion Play
                }

                if (SavePackets)
                    PacketsReceived.Add(packet);
            }
        }


        /// <summary>
        /// Enabling encryption
        /// </summary>
        /// <param name="packet">EncryptionRequestPacket</param>
        private void ModernEnableEncryption(IPacket packet)
        {
            var request = (EncryptionRequestPacket)packet;

            var hash = Asn1.GetServerIDHash(request.PublicKey, request.SharedKey, request.ServerId);

            if (!Yggdrasil.ClientAuth(_minecraft.AccessToken, _minecraft.SelectedProfile, hash))
                throw new ProtocolException("Yggdrasil error: Not authenticated.");

            var rsaParameter = Asn1.GetRsaParameters(request.PublicKey);

            var encryptedSecret = Asn1.EncryptData(rsaParameter, request.SharedKey);
            var encryptedVerify = Asn1.EncryptData(rsaParameter, request.VerificationToken);

            BeginSendPacketHandled(new EncryptionResponsePacket
            {
                SharedSecret = encryptedSecret,
                VerificationToken = encryptedVerify
            }, null, null);

            _stream.InitializeEncryption(request.SharedKey);
        }

        /// <summary>
        /// Setting compression
        /// </summary>
        /// <param name="packet">EncryptionRequestPacket</param>
        private void ModernSetCompression(IPacket packet)
        {
            var request = (ISetCompression)packet;

            _stream.SetCompression(request.Threshold);
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

            var result = _baseSock.BeginConnect(_minecraft.ServerHost, _minecraft.ServerPort, asyncCallback, state);
            EndConnect(result);


            return result;
        }

        public void EndConnect(IAsyncResult asyncResult)
        {
            _baseSock.EndConnect(asyncResult);

            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            // -- Create our Wrapped socket.
            _stream = new ModernStream(new NetworkStream(_baseSock.Client));

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
            if(!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            _stream.SendPacket(packet);

            if (SavePackets)
                PacketsSended.Add(packet);
        }

        public void Connect()
        {
            if (Connected)
                throw new ProtocolException("Connection error: Already connected to server.");

            // -- Connect to server.
            _baseSock = new TcpClient();
            _baseSock.Connect(_minecraft.ServerHost, _minecraft.ServerPort);

            // -- Create our Wrapped socket.
            _stream = new ModernStream(new NetworkStream(_baseSock.Client));

            // -- Begin data reading.
            _stream.BeginRead(new byte[0], 0, 0, PacketReceiverAsync, null);
        }

        public void Disconnect()
        {
            if(!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            _baseSock.Client.Disconnect(false);
        }

        #endregion


        public void Dispose()
        {
            if(_baseSock != null)
                _baseSock.Client.Dispose();

            if(_stream != null)
                _stream.Dispose();

            if (PacketsReceived != null)
                PacketsReceived.Clear();

            if (PacketsSended != null)
                PacketsSended.Clear();
        }
    }
}
