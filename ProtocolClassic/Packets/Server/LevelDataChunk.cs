using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Server
{
    public struct LevelDataChunkPacket : IPacketWithSize
    {
        public short ChunkLength;
        public byte[] ChunkData;
        public byte PercentComplete;

        public byte ID { get { return 0x03; } }
        public short Size { get { return 1028; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            ChunkLength = stream.ReadShort();
            ChunkData = stream.ReadByteArray(1024);
            PercentComplete = stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort(ChunkLength);
            stream.WriteByteArray(ChunkData);
            stream.WriteByte(PercentComplete);
            stream.Purge();

            return this;
        }
    }
}
