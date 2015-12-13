using System.Collections.Generic;
using System.Reflection;

using Aragas.Core.IO;

namespace MineLib.Core.Data.EntityMetadata
{
    public abstract class EntityMetadataEntry
    {
        protected abstract byte Identifier { get; }
        protected abstract string FriendlyName { get; }
        internal byte Index { private get; set; }

        public abstract void FromReader(PacketDataReader reader);
        public abstract void ToStream(PacketStream stream, byte index);

        public static implicit operator EntityMetadataEntry(byte value)
        {
            return new EntityMetadataByte(value);
        }

        public static implicit operator EntityMetadataEntry(short value)
        {
            return new EntityMetadataShort(value);
        }

        public static implicit operator EntityMetadataEntry(int value)
        {
            return new EntityMetadataInt(value);
        }

        public static implicit operator EntityMetadataEntry(float value)
        {
            return new EntityMetadataFloat(value);
        }

        public static implicit operator EntityMetadataEntry(string value)
        {
            return new EntityMetadataString(value);
        }

        public static implicit operator EntityMetadataEntry(ItemStack value)
        {
            return new EntityMetadataSlot(value);
        }

        public static implicit operator EntityMetadataEntry(Position value)
        {
            return new EntityMetadataVector(value);
        }

        public static implicit operator EntityMetadataEntry(Rotation value)
        {
            return new EntityMetadataRotation(value);
        }

        protected byte GetKey(byte index)
        {
            Index = index; // Cheat to get this for ToString
            return (byte) ((Identifier << 5) | (index & 0x1F));
        }

        public override string ToString()
        {
            var type = GetType();
            var fields = new List<FieldInfo>(type.GetRuntimeFields());
            var result = FriendlyName + "[" + Index + "]: ";
            if (fields.Count != 0)
                result += fields[0].GetValue(this).ToString();
            return result;
        }
    }
}