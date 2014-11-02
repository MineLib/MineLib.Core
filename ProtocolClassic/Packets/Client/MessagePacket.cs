using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Client
{
    public struct MessagePacket : IPacketWithSize
    {
        public byte UnUsed;
        public string Message;

        public byte ID { get { return 0x0D; } }
        public short Size { get { return 66; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            UnUsed = stream.ReadByte();
            Message = stream.ReadString();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(UnUsed);
            stream.WriteString(Message);
            stream.Purge();

            return this;
        }
    }
}
