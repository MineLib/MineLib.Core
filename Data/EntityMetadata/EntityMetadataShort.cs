using Aragas.Core.Interfaces;

namespace MineLib.Core.Data.EntityMetadata
{
    /// <summary>
    /// Short Metadata
    /// </summary>
    public class EntityMetadataShort : EntityMetadataEntry
    {
        protected override byte Identifier => 1;
        protected override string FriendlyName => "short";

        public short Value;

        public static implicit operator EntityMetadataShort(short value)
        {
            return new EntityMetadataShort(value);
        }

        public EntityMetadataShort()
        {
        }

        public EntityMetadataShort(short value)
        {
            Value = value;
        }

        public override void FromReader(PacketDataReader reader)
        {
            Value = reader.Read(Value);
        }

        public override void ToStream(IPacketStream stream, byte index)
        {
            stream.Write(GetKey(index));
            stream.Write(Value);
        }
    }
}
