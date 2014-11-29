using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Extension.Server
{
    public struct ExtAddPlayerNamePacket : IPacketWithSize
    {
        public short NameID;
        public string PlayerName;
        public string ListName;
        public string GroupName;
        public byte GroupRank;

        public byte ID { get { return 0x16; } }
        public short Size { get { return 196; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader reader)
        {
            NameID = reader.ReadShort();
            PlayerName = reader.ReadString();
            ListName = reader.ReadString();
            GroupName = reader.ReadString();
            GroupRank = reader.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort(NameID);
            stream.WriteString(PlayerName);
            stream.WriteString(ListName);
            stream.WriteString(GroupName);
            stream.WriteByte(GroupRank);
            stream.Purge();

            return this;
        }
    }
}
