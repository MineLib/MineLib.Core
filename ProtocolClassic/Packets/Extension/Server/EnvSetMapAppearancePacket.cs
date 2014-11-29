using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Extension.Server
{
    public struct EnvSetMapAppearancePacket : IPacketWithSize
    {
        public string TextureURL;
        public byte SideBlock;
        public byte EdgeBlock;
        public short SideLevel;

        public byte ID { get { return 0x1E; } }
        public short Size { get { return 69; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader reader)
        {
            TextureURL = reader.ReadString();
            SideBlock = reader.ReadByte();
            EdgeBlock = reader.ReadByte();
            SideLevel = reader.ReadShort();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteString(TextureURL);
            stream.WriteByte(SideBlock);
            stream.WriteByte(EdgeBlock);
            stream.WriteShort(SideLevel);
            stream.Purge();

            return this;
        }
    }
}
