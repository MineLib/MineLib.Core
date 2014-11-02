using MineLib.Network;
using MineLib.Network.Data.Anvil;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct MapChunkBulkPacket : IPacket
    {
        public ChunkList ChunkList;

        public byte ID { get { return 0x26; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            ChunkList = ChunkList.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            ChunkList.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}