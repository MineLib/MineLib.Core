using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using MineLib.Network;
using MineLib.Network.Cryptography;
using MineLib.Network.Events;
using MineLib.Network.IO;
using ProtocolModern.IO;
using ProtocolModern.Packets;
using ProtocolModern.Packets.Client.Login;
using ProtocolModern.Packets.Server.Login;

namespace ProtocolModern
{
    public class Protocol : IProtocol
    {
        public event PacketHandler PacketHandled;

        public IPacketSender PacketSender { get; set; }

        public string Name { get { return "Modern"; } }
        public string Version { get { return "1.8"; } }

        public bool Connected { get { return _baseSock.Connected; } }
        public bool Crashed { get; private set; }

        private IMinecraftClient _minecraft;

        private Socket _baseSock;
        private MinecraftStream _stream;

        private bool CompressionEnabled { get { return _stream.ModernCompressionEnabled; } }
        private long CompressionThreshold { get { return _stream.ModernCompressionThreshold; } }


        public void Connect(IMinecraftClient client)
        {
            _minecraft = client;

            PacketSender = new PacketSender().Create(_minecraft, this);

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
                return; // -- Terminate cycle

            if (_baseSock.Available > 0)
            {
                int packetId = 0;
                byte[] data = new byte[0];

                #region No Compression

                if (!CompressionEnabled)
                {
                    var packetLength = _stream.ReadVarInt();
                    if (packetLength == 0)
                        throw new Exception("Reading Error: Packet Length size is 0");

                    packetId = _stream.ReadVarInt();

                    data = _stream.ReadByteArray(packetLength - 1);
                }

                #endregion

                #region Compression

                else // (CompressionEnabled)
                {
                    var packetLength = _stream.ReadVarInt();
                    if (packetLength == 0)
                        throw new Exception("Reading Error: Packet Length size is 0");

                    var dataLength = _stream.ReadVarInt();
                    if (dataLength == 0)
                    {
                        if (packetLength >= CompressionThreshold)
                            throw new Exception("Reading Error: Received uncompressed message of size " + packetLength +
                                                " greater than threshold " + CompressionThreshold);

                        packetId = _stream.ReadVarInt();

                        data = _stream.ReadByteArray(packetLength - 2);
                    }
                    else // (dataLength > 0)
                    {
                        var dataLengthBytes = MinecraftStream.GetVarIntBytes(dataLength).Length;

                        var tempBuff = _stream.ReadByteArray(packetLength - dataLengthBytes); // -- Compressed

                        using (var outputStream = new MemoryStream())
                        using (var inputStream = new InflaterInputStream(new MemoryStream(tempBuff)))
                        //using (var reader = new MinecraftDataReader(new MemoryStream(tempBuff), NetworkMode))
                        {
                            inputStream.CopyTo(outputStream);
                            tempBuff = outputStream.ToArray(); // -- Decompressed

                            packetId = tempBuff[0]; // -- Only 255 packets available. ReadVarInt doesn't work.
                            var packetIdBytes = MinecraftStream.GetVarIntBytes(packetId).Length;

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
            using (var reader = new MinecraftDataReader(data))
            {
                IPacket packet;

                switch (_minecraft.State)
                {
                    #region Status

                    case ServerState.ModernStatus:
                        if (ServerResponse.Status[id] == null)
                            break;

                        packet = ServerResponse.Status[id]().ReadPacket(reader);

                        PacketHandled(id, packet, ServerState.ModernStatus);

                        break;

                    #endregion Status

                    #region Login

                    case ServerState.ModernLogin:
                        if (ServerResponse.Login[id] == null)
                            break;

                        packet = ServerResponse.Login[id]().ReadPacket(reader);

                        PacketHandled(id, packet, ServerState.ModernLogin);

                        if (id == 0x01)
                            ModernEnableEncryption(packet);  // -- Low-level encryption handle

                        if (id == 0x03)
                            ModernSetCompression(packet); // -- Low-level compression handle

                        break;

                    #endregion Login

                    #region Play

                    case ServerState.ModernPlay:
                        if (ServerResponse.Play[id] == null)
                            break;

                        packet = ServerResponse.Play[id]().ReadPacket(reader);

                        if (ServerResponse.Play[id]() == null)
                            throw new Exception("");

                        PacketHandled(id, packet, ServerState.ModernPlay);

                        if (id == 0x46)
                            ModernSetCompression(packet); // -- Low-level compression handle

                        // Connection lost
                        if (id == 0x40)
                            Crashed = true;

                        break;

                    #endregion Play
                }
            }
        }

        /// <summary>
        /// Enabling encryption
        /// </summary>
        /// <param name="packet">EncryptionRequestPacket</param>
        private void ModernEnableEncryption(IPacket packet)
        {
            // From libMC.NET
            var request = (EncryptionRequestPacket)packet;

            var hashlist = new List<byte>();
            hashlist.AddRange(Encoding.ASCII.GetBytes(request.ServerId));
            hashlist.AddRange(request.SharedKey);
            hashlist.AddRange(request.PublicKey);

            var hashData = hashlist.ToArray();

            var hash = JavaHelper.JavaHexDigest(hashData);

            if (!Yggdrasil.ClientAuth(_minecraft.AccessToken, _minecraft.SelectedProfile, hash))
                throw new Exception("Auth failure");

            // -- You pass it the key data and ask it to parse, and it will 
            // -- Extract the server's public key, then parse that into RSA for us.
            var keyParser = new AsnKeyParser(request.PublicKey);
            var deKey = keyParser.ParseRSAPublicKey();

            // -- Now we create an encrypter, and encrypt the token sent to us by the server
            // -- as well as our newly made shared key (Which can then only be decrypted with the server's private key)
            // -- and we send it to the server.
            var cryptoService = new RSACryptoServiceProvider();
            cryptoService.ImportParameters(deKey);

            var encryptedSecret = cryptoService.Encrypt(request.SharedKey, false);
            var encryptedVerify = cryptoService.Encrypt(request.VerificationToken, false);

            _stream.InitializeEncryption(request.SharedKey);

            // -- This packet won't be added in sended packet list.
            BeginSend(new EncryptionResponsePacket
            {
                SharedSecret = encryptedSecret,
                VerificationToken = encryptedVerify
            }, null, null);

            _stream.EncryptionEnabled = true;
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


        public IAsyncResult BeginSendPacket(IPacket packet, AsyncCallback asyncCallback, object state)
        {
            if (!Connected)
                throw new Exception("Connection error");

            IAsyncResult result = BeginSend(packet, asyncCallback, state);
            EndSend(result);
            return result;
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
            if(_baseSock != null)
                _baseSock.Dispose();

            if(_stream != null)
                _stream.Dispose();
        }
    }
}
