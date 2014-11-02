using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct EntityMetadataPacket : IPacket
    {
        public int EntityID;
        public EntityMetadata Metadata;

        public byte ID { get { return 0x1C; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Metadata = EntityMetadata.FromReader(reader);

            return this;
        }
    
        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            Metadata.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}