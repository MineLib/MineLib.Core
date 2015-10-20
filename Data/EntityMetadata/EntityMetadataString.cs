using System;

using Aragas.Core.Interfaces;

namespace MineLib.Core.Data.EntityMetadata
{
    /// <summary>
    /// String Metadata
    /// </summary>
    public class EntityMetadataString : EntityMetadataEntry
    {
        public override byte Identifier { get { return 4; } }
        public override string FriendlyName { get { return "string"; } }

        public string Value;

        public static implicit operator EntityMetadataString(string value)
        {
            return new EntityMetadataString(value);
        }

        public EntityMetadataString()
        {
        }

        public EntityMetadataString(string value)
        {
            if (value.Length > 16)
                throw new ArgumentOutOfRangeException("value", "Maximum string length is 16 characters");
            while (value.Length < 16)
                value = value + "\0";
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