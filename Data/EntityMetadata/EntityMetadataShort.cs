using MineLib.Network.IO;

namespace MineLib.Network.Data.EntityMetadata
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

        public override void FromReader(IProtocolDataReader reader)
        {
            Value = reader.ReadShort();
        }

        public override void ToStream(IProtocolStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteShort(Value);
        }
    }
}
