using MineLib.Network;
using MineLib.Network.IO;
using ProtocolClassic.Data;

namespace ProtocolClassic.Packets.Server
{
    public struct LevelDataChunkPacket : IPacketWithSize
    {
        public short ChunkLength;
        public byte[] ChunkData;
        public byte PercentComplete;

        public byte ID { get { return 0x03; } }
        public short Size { get { return 1028; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader reader)
        {
            ChunkLength = reader.ReadShort();
            ChunkData = reader.ReadByteArray(1024);
            PercentComplete = reader.ReadByte();

            var level = Level.ReadFromArray(ChunkData);

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader reader)
        {
            return ReadPacket(reader);
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
