using MineLib.Network;
using MineLib.Network.IO;
using ProtocolClassic.Enums;

namespace ProtocolClassic.Packets.Extension.Server
{
    public struct HoldThisPacket : IPacketWithSize
    {
        public byte BlockToHold;
        public PreventChange PreventChange;

        public byte ID { get { return 0x14; } }
        public short Size { get { return 3; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            BlockToHold = stream.ReadByte();
            PreventChange = (PreventChange) stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(BlockToHold);
            stream.WriteByte((byte) PreventChange);
            stream.Purge();

            return this;
        }
    }
}
