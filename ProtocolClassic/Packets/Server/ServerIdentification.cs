using MineLib.Network;
using MineLib.Network.IO;
using ProtocolClassic.Enums;

namespace ProtocolClassic.Packets.Server
{
    public struct ServerIdentificationPacket : IPacketWithSize
    {
        public byte ProtocolVersion;
        public string ServerName;
        public string ServerMOTD;
        public UserType UserType;

        public byte ID { get { return 0x00; } }
        public short Size { get { return 131; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader reader)
        {
            ProtocolVersion = reader.ReadByte();
            ServerName = reader.ReadString();
            ServerMOTD = reader.ReadString();
            UserType = (UserType) reader.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(ProtocolVersion);
            stream.WriteString(ServerName);
            stream.WriteString(ServerMOTD);
            stream.WriteByte((byte) UserType);
            stream.Purge();

            return this;
        }
    }
}
