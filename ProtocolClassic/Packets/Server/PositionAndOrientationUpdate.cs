using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Server
{
    public struct PositionAndOrientationUpdatePacket : IPacketWithSize
    {
        public sbyte PlayerID;
        public sbyte ChangeX;
        public sbyte ChangeY;
        public sbyte ChangeZ;
        public byte Yaw;
        public byte Pitch;

        public byte ID { get { return 0x09; } }
        public short Size { get { return 7; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            PlayerID = stream.ReadSByte();
            ChangeX = stream.ReadSByte();
            ChangeY = stream.ReadSByte();
            ChangeZ = stream.ReadSByte();
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteSByte(PlayerID);
            stream.WriteSByte(ChangeX);
            stream.WriteSByte(ChangeY);
            stream.WriteSByte(ChangeZ);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.Purge();

            return this;
        }
    }
}
