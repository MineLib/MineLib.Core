using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Server
{
    public struct LevelFinalizePacket : IPacketWithSize
    {
        public Position Coordinates;

        public byte ID { get { return 0x04; } }
        public short Size { get { return 7; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader reader)
        {
            Coordinates = Position.FromReaderShort(reader);

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
            stream.Purge();

            return this;
        }
    }
}
