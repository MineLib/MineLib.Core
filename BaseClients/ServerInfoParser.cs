﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using MineLib.Network.Enums;
using MineLib.Network.Packets;
using MineLib.Network.Packets.Client.Status;
using MineLib.Network.Packets.Server.Status;
using Newtonsoft.Json;

namespace MineLib.Network.BaseClients
{
    public struct ResponseData
    {
        public ServerInfo Info;
        public int Ping;
    }

    // TODO: Ping isn't correct
    // TODO: Handle this mess
    /// <summary>
    /// Initiate new instance for each new server
    /// </summary>
    public partial class ServerInfoParser : IMinecraftClient, IDisposable
    {
        #region Variables

        public string ServerHost { get; set; }

        public short ServerPort { get; set; }

        public ServerState State { get; set; }

        public string AccessToken
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string SelectedProfile
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion Variables

        private NetworkHandler _handler;
        private bool _connectionClosed { get { return !_handler.Connected; }}

        private bool _responsePacketReceived;
        private bool _pingPacketReceived;


        public ServerInfoParser()
        {
            State = ServerState.Status;
        }

        public ResponseData GetResponseData(string host, short port, int protocolVersion)
        {
            ServerHost = host;
            ServerPort = port;

            var info = new ServerInfo();

            _handler = new NetworkHandler(this);
            _handler.OnPacketHandled += RaisePacketHandled;
            _handler.Start();

            #region Handle Early connect error

            if (_connectionClosed)
            {
                Dispose();
                return new ResponseData { Info = new ServerInfo(), Ping = int.MaxValue };
            }

            #endregion

            #region Packet handlers

            FireResponsePacket += packet =>
            {
                // Send Ping Packet after receiving Response Packet
                SendPacket(new Packets.Client.Status.PingPacket { Time = DateTime.UtcNow.Millisecond });

                info = ParseResponse(packet);
                _responsePacketReceived = true;
            };

            FirePingPacket += packet =>
            {
                _pingPacketReceived = true;
            };

            #endregion

            RequestServerInfo(protocolVersion);

            #region Waiting for all packets handling and handling if something went wrong

            while (!_pingPacketReceived)
            {
                if (_handler == null || _handler.Crashed)
                {
                    Dispose();
                    return new ResponseData { Info = new ServerInfo(), Ping = int.MaxValue };
                }

                Thread.Sleep(50);
            }

            #endregion

            // Create a new TCP connection, if something is wrong use _handler TCP client
            var ping = (int) PingServer(host, port);

            Dispose();
            return new ResponseData { Info = info, Ping = ping };
        }

        public ServerInfo GetServerInfo(string host, short port, int protocolVersion)
        {
            ServerHost = host;
            ServerPort = port;

            var info = new ServerInfo();

            _handler = new NetworkHandler(this);
            _handler.OnPacketHandled += RaisePacketHandled;
            _handler.Start();

            #region Handle Early connect error

            if (_connectionClosed)
            {
                Dispose();
                return new ServerInfo();
            }

            #endregion
            
            #region Packet handlers

            FireResponsePacket += packet =>
            {
                info = ParseResponse(packet);
                _responsePacketReceived = true;
            };

            #endregion

            RequestServerInfo(protocolVersion);

            #region Waiting for all packets handling and handling if something went wrong

            while (!_responsePacketReceived)
            {
                if (_handler != null && _handler.Crashed)
                {
                    Dispose();
                    return new ServerInfo();
                }

                Thread.Sleep(50);
            }

            #endregion

            Dispose();
            return info;
        }

        public long GetPing(string host, short port, int protocolVersion)
        {
            ServerHost = host;
            ServerPort = port;

            _handler = new NetworkHandler(this);
            _handler.OnPacketHandled += RaisePacketHandled;
            _handler.Start();

            #region Handle Early connect error

            if (_connectionClosed)
            {
                Dispose();
                return int.MaxValue;
            }

            #endregion

            #region Packet handlers

            FireResponsePacket +=
                packet => SendPacket(new Packets.Client.Status.PingPacket {Time = DateTime.UtcNow.Millisecond});

            FirePingPacket += packet =>
            {
                _pingPacketReceived = true;
            };

            #endregion

            RequestServerInfo(protocolVersion);

            #region Waiting for all packets handling and handling if something went wrong

            while (!_pingPacketReceived)
            {
                if (_handler != null && _connectionClosed)
                {
                    Dispose();
                    return int.MaxValue;
                }
            }

            #endregion

            var ping = PingServer(host, port);

            Dispose();
            return ping;
        }


        private void RequestServerInfo(int protocolVersion)
        {
            SendPacket(new HandshakePacket
            {
                ServerAddress = ServerHost,
                ServerPort = ServerPort,
                ProtocolVersion = protocolVersion,
                NextState = NextState.Status
            });

            SendPacket(new RequestPacket());
        }

        private ServerInfo ParseResponse(IPacket packet)
        {
            var response = (ResponsePacket)packet;

            return JsonConvert.DeserializeObject<ServerInfo>(response.Response, new Base64Converter());
        }

        private void SendPacket(IPacket packet)
        {
            if (_handler != null)
                _handler.Send(packet);
        }

        private static long PingServer(string host, int port)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            TcpClient client = new TcpClient(host, port);
            watch.Stop();
            client.Close();

            return watch.ElapsedMilliseconds;

        }


        public void Dispose()
        {
            if(_handler != null)
                _handler.Dispose();
        }
    }
}