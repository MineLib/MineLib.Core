using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolMinetest.Packets.Client
{
    public class ToServerInit : IPacketWithSize
    {
        public short Init;
        public byte SerFmtVerHighestRead;
        public string PlayerName;
        public string Password;
        public short MinimumSupportedProtocol;
        public short MaximumSupportedProtocol;

        public byte ID { get { return 0x10; } }
        public short Size { get { return 53; } }
        IPacket IPacket.ReadPacket(IProtocolDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacketWithSize ReadPacket(IProtocolDataReader reader)
        {
            Init = reader.ReadShort();
            SerFmtVerHighestRead = reader.ReadByte();
            PlayerName = reader.ReadString(20);
            Password = reader.ReadString(28);
            MinimumSupportedProtocol = reader.ReadShort();
            MaximumSupportedProtocol = reader.ReadShort();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteShort(Init);
            stream.WriteByte(SerFmtVerHighestRead);
            stream.WriteString(PlayerName, 20);
            stream.WriteString(Password, 28);
            stream.WriteShort(MinimumSupportedProtocol);
            stream.WriteShort(MaximumSupportedProtocol);
            stream.Purge();

            return this;
        }
    }
}
