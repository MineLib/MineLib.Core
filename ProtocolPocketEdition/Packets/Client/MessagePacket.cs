using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolPocketEdition.Packets.Client
{
    public class MessagePacket : IPacketWithSize
    {
        public string Message;

        public byte ID { get { return 0x85; } }
        public short Size { get { return 0; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader reader)
        {
            Message = reader.ReadString();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Message);
            stream.Purge();

            return this;
        }
    }
}
