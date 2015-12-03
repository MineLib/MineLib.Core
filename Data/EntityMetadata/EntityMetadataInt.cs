using Aragas.Core.Interfaces;

namespace MineLib.Core.Data.EntityMetadata
{
    /// <summary>
    /// Int32 Metadata
    /// </summary>
    public class EntityMetadataInt : EntityMetadataEntry
    {
        protected override byte Identifier => 2;
        protected override string FriendlyName => "int";

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