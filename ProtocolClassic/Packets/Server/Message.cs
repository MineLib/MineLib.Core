using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Server
{
    public struct MessagePacket : IPacketWithSize
    {
        public sbyte PlayerID;
        public string Message;

        public byte ID { get { return 0x0D; } }
        public short Size { get { return 66; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            PlayerID = stream.ReadSByte();
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
            stream.WriteSByte(PlayerID);
            stream.WriteString(Message);
            stream.Purge();

            return this;
        }
    }
}
