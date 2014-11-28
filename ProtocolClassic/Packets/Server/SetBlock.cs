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

        public IPacketWithSize ReadPacket(IProtocolDataReader reader)
        {
            Coordinates = Position.FromReaderShort(reader);
            BlockType = reader.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            Coordinates.ToStreamShort(stream);
            stream.WriteByte(BlockType);
            stream.Purge();

            return this;
        }
    }
}
