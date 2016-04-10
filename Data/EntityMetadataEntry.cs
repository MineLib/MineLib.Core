using System.Collections.Generic;
using System.Reflection;

using Aragas.Core.IO;

namespace MineLib.Core.Data
{
    public abstract class EntityMetadataEntry
    {
        protected abstract byte Identifier { get; }
        protected abstract string FriendlyName { get; }
        public byte Index { private get; set; }

        public abstract void FromReader(PacketDataReader reader);
        public abstract void ToStream(PacketStream stream, byte index);


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