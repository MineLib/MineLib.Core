using System;
using System.Text;
using MineLib.Network;
using MineLib.Network.Data;
using ProtocolModern.Enum;
using ProtocolModern.Packets;
using ProtocolModern.Packets.Client;
using ProtocolModern.Packets.Client.Login;

namespace ProtocolModern
{
    public partial class Protocol
    {
        public IAsyncResult BeginConnectToServer(AsyncCallback asyncCallback, object state)
        {
            ConnectionState = ConnectionState.JoiningServer;

            BeginSendPacketHandled(new HandshakePacket
            {
                ProtocolVersion = 47,
                ServerAddress = _minecraft.ServerHost,
                ServerPort = _minecraft.ServerPort,
                NextState = NextState.Login,
            }, null, null);

            return BeginSendPacketHandled(new LoginStartPacket { Name = _minecraft.ClientUsername }, asyncCallback, state);
        }

        public IAsyncResult BeginKeepAlive(int value, AsyncCallback asyncCallback, object state)
        {
            return BeginSendPacketHandled(new KeepAlivePacket { KeepAlive = value }, null, null);
        }

        public IAsyncResult BeginSendClientInfo(AsyncCallback asyncCallback, object state)
        {
            return BeginSendPacketHandled(new PluginMessagePacket
            {
                Channel = "MC|Brand",
                Data = Encoding.UTF8.GetBytes(_minecraft.ClientBrand)
            }, asyncCallback, state);
        }

        public IAsyncResult BeginRespawn(AsyncCallback asyncCallback, object state)
        {
            return BeginSendPacketHandled(new ClientStatusPacket { Status = ClientStatus.Respawn }, null, null);
        }

        public IAsyncResult BeginPlayerMoved(PlaverMovedData data, AsyncCallback asyncCallback, object state)
        {
            switch (data.Mode)
            {
                case PlaverMovedMode.OnGround:
                    return BeginSendPacketHandled(new PlayerPacket { OnGround = data.OnGround }, asyncCallback, state);

                case PlaverMovedMode.Vector3:
                    return BeginSendPacketHandled(new PlayerPositionPacket
                    {
                        X = data.Vector3.X,
                        FeetY = data.Vector3.Y,
                        Z = data.Vector3.Z,
                        OnGround = data.OnGround
                    }, asyncCallback, state);

                case PlaverMovedMode.YawPitch:
                    return BeginSendPacketHandled(new PlayerLookPacket
                    {
                        Yaw = data.Yaw,
                        Pitch = data.Pitch,
                        OnGround = data.OnGround
                    }, asyncCallback, state);

                case PlaverMovedMode.All:
                    return BeginSendPacketHandled(new PlayerPositionAndLookPacket
                    {
                        X = data.Vector3.X,
                        FeetY = data.Vector3.Y,
                        Z = data.Vector3.Z,
                        Yaw = data.Yaw,
                        Pitch = data.Pitch,
                        OnGround = data.OnGround
                    }, asyncCallback, state);

                default:
                    throw new Exception("PacketError");
            }
        }

        public IAsyncResult BeginPlayerSetRemoveBlock(PlayerSetRemoveBlockData data, AsyncCallback asyncCallback, object state)
        {
            switch (data.Mode)
            {
                case PlayerSetRemoveBlockEnum.Place:
                    return BeginSendPacketHandled(new PlayerBlockPlacementPacket
                    {
                        Location = data.Location,
                        Slot = data.Slot,
                        CursorVector3 = data.Crosshair,
                        Direction = (Direction)data.Direction
                    }, asyncCallback, state);

                case PlayerSetRemoveBlockEnum.Dig:
                    return BeginSendPacketHandled(new PlayerDiggingPacket
                    {
                        Status = (BlockStatus)data.Status,
                        Location = data.Location,
                        Face = data.Face
                    }, asyncCallback, state);

                default:
                    throw new Exception("PacketError");
            }
        }

        public IAsyncResult BeginSendMessage(string message, AsyncCallback asyncCallback, object state)
        {
            return BeginSendPacketHandled(new ChatMessagePacket { Message = message }, asyncCallback, state);
        }

        public IAsyncResult BeginPlayerHeldItem(short slot, AsyncCallback asyncCallback, object state)
        {
            return BeginSendPacketHandled(new HeldItemChangePacket { Slot = slot }, asyncCallback, state);
        }
    }
}
