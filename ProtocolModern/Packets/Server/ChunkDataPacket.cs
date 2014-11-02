using MineLib.Network;
using MineLib.Network.Data.Anvil;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct ChunkDataPacket : IPacket
    {
        public Chunk Chunk;

        public byte ID { get { return 0x21; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Chunk = Chunk.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            Chunk.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}