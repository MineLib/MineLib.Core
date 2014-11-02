using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolMinetest.Packets.Client
{
    public class ToServerPlayerPos : IPacketWithSize
    {
        public short Command;
        public int KeyPressed;

        public byte ID { get { return 0x10; } }
        public short Size { get { return 34; } }
        IPacket IPacket.ReadPacket(IProtocolDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacketWithSize ReadPacket(IProtocolDataReader reader)
        {
            Command = reader.ReadShort();
            KeyPressed = reader.ReadInt();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteShort(Command);
            stream.WriteInt(KeyPressed);
            stream.Purge();

            return this;
        }
    }
}
