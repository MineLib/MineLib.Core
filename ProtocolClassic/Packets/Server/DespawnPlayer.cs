using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Server
{
    public struct DespawnPlayerPacket : IPacketWithSize
    {
        public sbyte PlayerID;

        public byte ID { get { return 0x0C; } }
        public short Size { get { return 2; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            PlayerID = stream.ReadSByte();

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
            stream.Purge();

            return this;
        }
    }
}
