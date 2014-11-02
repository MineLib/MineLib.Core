using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolMinetest.Packets.Client
{
    public class ToServerInit2 : IPacketWithSize
    {
        public short Init2;

        public byte ID { get { return 0x11; } }
        public short Size { get { return 2; } }
        IPacket IPacket.ReadPacket(IProtocolDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacketWithSize ReadPacket(IProtocolDataReader reader)
        {
            Init2 = reader.ReadShort();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteShort(Init2);
            stream.Purge();

            return this;
        }
    }
}
