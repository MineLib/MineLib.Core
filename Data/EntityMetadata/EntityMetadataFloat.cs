using Aragas.Core.Interfaces;

namespace MineLib.Core.Data.EntityMetadata
{
    /// <summary>
    /// Float Metadata
    /// </summary>
    public class EntityMetadataFloat : EntityMetadataEntry
    {
        protected override byte Identifier => 3;
        protected override string FriendlyName => "float";

        public float Value;

        public static implicit operator EntityMetadataFloat(float value)
        {
            return new EntityMetadataFloat(value);
        }

        public EntityMetadataFloat()
        {
        }

        public EntityMetadataFloat(float value)
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
