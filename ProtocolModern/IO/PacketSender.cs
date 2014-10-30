using System;
using System.Text;
using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;
using ProtocolModern.Enum;
using ProtocolModern.Packets;
using ProtocolModern.Packets.Client;
using ProtocolModern.Packets.Client.Login;

namespace ProtocolModern.IO
{
    public class PacketSender : IPacketSender
    {
        public event Action ConnectedToServer;
        public event Action Disconnected;

        //public event 

        private IMinecraftClient _minecraft;
        private IProtocol _protocol;

        public ConnectionState ConnectionState { get; set; }

        public IPacketSender Create(IMinecraftClient minecraft, IProtocol protocol)
        {
            _minecraft = minecraft;
            _protocol = protocol;

            ConnectedToServer += OnConnectedToServer;
            Disconnected += OnDisconnected;

            return this;
        }


        public void ConnectToServer()
        {
            ConnectionState = ConnectionState.Connecting;

            _protocol.BeginSendPacket(new HandshakePacket
            {
                ProtocolVersion = 47,
                ServerAddress = _minecraft.ServerHost,
                ServerPort = _minecraft.ServerPort,
                NextState = NextState.Login,
            }, null, null);

            _protocol.BeginSendPacket(new LoginStartPacket { Name = _minecraft.PlayerName }, null, null);
        }

        public void KeepAlive(int value)
        {
            _protocol.BeginSendPacket(new KeepAlivePacket { KeepAlive = value }, null, null);
        }

        public void SendClientInfo()
        {
            _protocol.BeginSendPacket(new PluginMessagePacket
            {
                Channel = "MC|Brand",
                Data = Encoding.UTF8.GetBytes(_minecraft.ClientBrand)
            }, null, null);
        }

        public void Respawn()
        {
            _protocol.BeginSendPacket(new ClientStatusPacket { Status = ClientStatus.Respawn }, null, null);
        }

        public void PlayerMoved(byte mode, Vector3 vector3 = default(Vector3), bool onGround = false, float yaw = 0f, float pitch = 0f)
        {
            switch (mode)
            {
                case 0x00:
                    _protocol.BeginSendPacket(new PlayerPacket { OnGround = onGround }, null, null);
                    break;

                case 0x01:
                    _protocol.BeginSendPacket(new PlayerPositionPacket
                    {
                        X = vector3.X,
                        FeetY = vector3.Y,
                        Z = vector3.Z,
                        OnGround = onGround
                    }, null, null);
                    break;

                case 0x02:
                    _protocol.BeginSendPacket(new PlayerLookPacket
                    {
                        Yaw = yaw,
                        Pitch = pitch,
                        OnGround = onGround
                    }, null, null);
                    break;

                case 0x03:
                    _protocol.BeginSendPacket(new PlayerPositionAndLookPacket
                    {
                        X = vector3.X,
                        FeetY = vector3.Y,
                        Z = vector3.Z,
                        Yaw = yaw,
                        Pitch = pitch,
                        OnGround = onGround
                    }, null, null);
                    break;
            }
        }

        public void PlayerSetRemoveBlock(byte mode, Position location = default(Position), int blockID = 0, byte status = 0x00, byte face = 0x00, byte direction = 0x00, ItemStack slot = default(ItemStack), Position crosshair = default(Position))
        {
            switch (mode)
            {
                case 0x01:
                    _protocol.BeginSendPacket(new PlayerBlockPlacementPacket
                    {
                        Location = location,
                        Slot = slot,
                        CursorVector3 = crosshair,
                        Direction = (Direction)direction
                    }, null, null);
                    break;

                case 0x02:
                    _protocol.BeginSendPacket(new PlayerDiggingPacket
                    {
                        Status = (BlockStatus) status,
                        Location = location,
                        Face = face
                    }, null, null);
                    break;
            }
        }

        public void SendMessage(string message)
        {
            _protocol.BeginSendPacket(new ChatMessagePacket { Message = message }, null, null);
        }



        private void OnConnectedToServer()
        {
            ConnectionState = ConnectionState.Connected;
        }

        private void OnDisconnected()
        {
            ConnectionState = ConnectionState.None;
        }
    }
}
