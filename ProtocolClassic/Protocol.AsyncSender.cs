using System;
using MineLib.Network;
using MineLib.Network.Data;
using ProtocolClassic.Enum;
using ProtocolClassic.Packets.Client;

namespace ProtocolClassic
{
    public partial class Protocol
    {
        public IAsyncResult BeginConnectToServer(AsyncCallback asyncCallback, object state)
        {
            State = ConnectionState.JoiningServer;

            return BeginSendPacketHandled(new PlayerIdentificationPacket
            {
                ProtocolVersion = 0x07,
                Username = _minecraft.ClientUsername,
                VerificationKey = _minecraft.AccessToken,
                UnUsed = 0x42
            }, asyncCallback, state);
        }

        public IAsyncResult BeginKeepAlive(int value, AsyncCallback asyncCallback, object state)
        {
            return null;
        }

        public IAsyncResult BeginSendClientInfo(AsyncCallback asyncCallback, object state)
        {
            return null;
        }

        public IAsyncResult BeginRespawn(AsyncCallback asyncCallback, object state)
        {
            return null;
        }

        private byte _playerID_Slot = 255;
        public IAsyncResult BeginPlayerMoved(PlaverMovedData data, AsyncCallback asyncCallback, object state)
        {
            switch (data.Mode)
            {
                case PlaverMovedMode.YawPitch:
                    return BeginSendPacketHandled(
                            new PositionAndOrientationPacket
                            {
                                Position = data.Vector3,
                                Yaw = (byte) data.Yaw,
                                Pitch = (byte) data.Pitch,
                                PlayerID = _playerID_Slot
                            }, asyncCallback, state);

                default:
                    return null;
            }
        }

        public IAsyncResult BeginPlayerSetRemoveBlock(PlayerSetRemoveBlockData data, AsyncCallback asyncCallback, object state)
        {
            switch (data.Mode)
            {
                case PlayerSetRemoveBlockEnum.Place:
                case PlayerSetRemoveBlockEnum.Remove:
                    return BeginSendPacketHandled(new SetBlockPacket
                    {
                        Coordinates = data.Location,
                        BlockType = (byte) data.BlockID,
                        Mode = (SetBlockMode) data.Mode
                    }, asyncCallback, state);

                default:
                    throw new Exception("PacketError");
            }
        }

        public IAsyncResult BeginSendMessage(string message, AsyncCallback asyncCallback, object state)
        {
            return BeginSendPacketHandled(new MessagePacket { Message = message }, asyncCallback, state);
        }

        public IAsyncResult BeginPlayerHeldItem(short slot, AsyncCallback asyncCallback, object state)
        {
            if (UsingExtensions)
                _playerID_Slot = (byte) slot;

            return null;
        }
    }
}
