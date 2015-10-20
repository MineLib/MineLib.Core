using Aragas.Core.Interfaces;

namespace MineLib.Core.Data.EntityMetadata
{
    /// <summary>
    /// Byte Metadata
    /// </summary>
    public class EntityMetadataByte : EntityMetadataEntry
    {
        public override byte Identifier { get { return 0; } }
        public override string FriendlyName { get { return "byte"; } }

        public byte Value;

        public static implicit operator EntityMetadataByte(byte value)
        {
            return new EntityMetadataByte(value);
        }

        public EntityMetadataByte()
        {
        }

        public EntityMetadataByte(byte value)
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
