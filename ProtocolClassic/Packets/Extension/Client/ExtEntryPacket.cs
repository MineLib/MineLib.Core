using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Extension.Client
{
    public struct ExtEntryPacket : IPacketWithSize
    {
        public string ExtName;
        public int Version;

        public byte ID { get { return 0x11; } }
        public short Size { get { return 69; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader reader)
        {
            ExtName = reader.ReadString();
            Version = reader.ReadInt();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteString(ExtName);
            stream.WriteInt(Version);
            stream.Purge();

            return this;
        }
    }
}
