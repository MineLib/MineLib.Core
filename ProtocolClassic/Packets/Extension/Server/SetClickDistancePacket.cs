using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Extension.Server
{
    public struct SetClickDistancePacket : IPacketWithSize
    {
        public short Distance;

        public byte ID { get { return 0x12; } }
        public short Size { get { return 3; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            Distance = stream.ReadShort();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort(Distance);
            stream.Purge();

            return this;
        }
    }
}
