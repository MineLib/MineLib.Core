using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Server
{
    public struct SetBlockPacket : IPacketWithSize
    {
        public Position Coordinates;
        public byte BlockType;

        public byte ID { get { return 0x06; } }
        public short Size { get { return 8; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            Coordinates.X = stream.ReadShort();
            Coordinates.Y = stream.ReadShort();
            Coordinates.Z = stream.ReadShort();
            BlockType = stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort((short)Coordinates.X);
            stream.WriteShort((short)Coordinates.Y);
            stream.WriteShort((short)Coordinates.Z);
            stream.WriteByte(BlockType);
            stream.Purge();

            return this;
        }
    }
}
