using MineLib.Network;
using MineLib.Network.IO;
using ProtocolClassic.Enums;

namespace ProtocolClassic.Packets.Server
{
    public struct UpdateUserTypePacket : IPacketWithSize
    {
        public UserType UserType;

        public byte ID { get { return 0x0F; } }
        public short Size { get { return 2; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            UserType = (UserType) stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte((byte) UserType);
            stream.Purge();

            return this;
        }
    }
}
