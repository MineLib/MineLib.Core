using MineLib.Network.IO;

namespace MineLib.Network.Data.EntityMetadata
{
    /// <summary>
    /// Float Metadata
    /// </summary>
    public class EntityMetadataFloat : EntityMetadataEntry
    {
        public override byte Identifier { get { return 3; } }
        public override string FriendlyName { get { return "float"; } }

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

        public override void FromReader(IProtocolDataReader reader)
        {
            Value = reader.ReadFloat();
        }

        public override void ToStream(IProtocolStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteFloat(Value);
        }
    }
}
