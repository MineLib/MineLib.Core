using Aragas.Core.Interfaces;

namespace MineLib.Core.Data.EntityMetadata
{
    /// <summary>
    /// Short Metadata
    /// </summary>
    public class EntityMetadataShort : EntityMetadataEntry
    {
        public override byte Identifier { get { return 1; } }
        public override string FriendlyName { get { return "short"; } }

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

        public override void FromReader(IPacketDataReader reader)
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
