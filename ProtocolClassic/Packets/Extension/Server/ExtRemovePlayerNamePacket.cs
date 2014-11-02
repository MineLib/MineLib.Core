using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Extension.Server
{
    public struct ExtRemovePlayerNamePacket : IPacketWithSize
    {
        public short NameID;

        public byte ID { get { return 0x18; } }
        public short Size { get { return 3; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            NameID = stream.ReadShort();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort(NameID);
            stream.Purge();

            return this;
        }
    }
}
