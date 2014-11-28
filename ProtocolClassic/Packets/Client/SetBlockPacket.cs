using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;
using ProtocolClassic.Enums;

namespace ProtocolClassic.Packets.Client
{
    public struct SetBlockPacket : IPacketWithSize
    {
        public Position Coordinates;
        public SetBlockMode Mode;
        public byte BlockType;

        public byte ID { get { return 0x05; } }
        public short Size { get { return 9; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            Coordinates = Position.FromReaderShort(stream);
            Mode = (SetBlockMode) stream.ReadByte();
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
            Coordinates.ToStreamShort(stream);
            stream.WriteByte((byte) Mode);
            stream.WriteByte(BlockType);
            stream.Purge();

            return this;
        }
    }
}
