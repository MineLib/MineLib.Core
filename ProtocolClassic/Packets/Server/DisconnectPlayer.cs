using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Server
{
    public struct DisconnectPlayerPacket : IPacketWithSize
    {
        public string Reason;

        public byte ID { get { return 0x0E; } }
        public short Size { get { return 65; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            Reason = stream.ReadString();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteString(Reason);
            stream.Purge();

            return this;
        }
    }
}
