using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Server
{
    public struct PingPacket : IPacketWithSize
    {
        public byte ID { get { return 0x01; } }
        public short Size { get { return 1; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.Purge();

            return this;
        }
    }
}
