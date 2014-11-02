using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Server
{
    public struct PositionUpdatePacket : IPacketWithSize
    {
        public sbyte PlayerID;
        public sbyte ChangeX;
        public sbyte ChangeY;
        public sbyte ChangeZ;

        public byte ID { get { return 0x0A; } }
        public short Size { get { return 5; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            PlayerID = stream.ReadSByte();
            ChangeX = stream.ReadSByte();
            ChangeY = stream.ReadSByte();
            ChangeZ = stream.ReadSByte();

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
            stream.Purge();

            return this;
        }
    }
}
