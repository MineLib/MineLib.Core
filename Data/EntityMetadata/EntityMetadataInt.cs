using Aragas.Core.Interfaces;

namespace MineLib.Core.Data.EntityMetadata
{
    /// <summary>
    /// Int32 Metadata
    /// </summary>
    public class EntityMetadataInt : EntityMetadataEntry
    {
        public override byte Identifier { get { return 2; } }
        public override string FriendlyName { get { return "int"; } }

        public int Value;

        public static implicit operator EntityMetadataInt(int value)
        {
            return new EntityMetadataInt(value);
        }

        public EntityMetadataInt()
        {
        }

        public EntityMetadataInt(int value)
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