using System;
using System.Collections.Generic;

using MineLib.Core.IO;

using Org.BouncyCastle.Math;

namespace MineLib.Core.Data.Structs
{
    public struct Modifiers : IEquatable<Modifiers>
    {
        public BigInteger UUID;
        public float Amount;
        public sbyte Operation;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Modifiers) obj);
        }

        public bool Equals(Modifiers other)
        {
            return UUID == other.UUID && Amount == other.Amount && Operation == other.Operation;
        }

        public override int GetHashCode()
        {
            return UUID.GetHashCode() ^ Amount.GetHashCode() ^ Operation.GetHashCode();
        }
    }

    public struct EntityProperty : IEquatable<EntityProperty>
    {
        public string Key;
        public float Value;
        public Modifiers[] Modifiers;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((EntityProperty) obj);
        }

        public bool Equals(EntityProperty other)
        {
            return Key == other.Key && Value == other.Value && Modifiers == other.Modifiers;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode() ^ Value.GetHashCode() ^ Modifiers.GetHashCode();
        }
    }

    public class EntityPropertyList : IEquatable<EntityPropertyList>
    {
        private readonly List<EntityProperty> _entries;

        public EntityPropertyList()
        {
            _entries = new List<EntityProperty>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public EntityProperty this[int index]
        {
            get { return _entries[index]; }
            set
            {
                if (_entries.Count - 1 < index)
                    _entries.Add(value);
                else
                    _entries[index] = value;
            }
        }

        #region Network

        public static EntityPropertyList FromReader(IProtocolDataReader reader)
        {
            var count = reader.ReadInt();

            var value = new EntityPropertyList();
            for (int i = 0; i < count; i++)
            {
                var property = new EntityProperty();

                property.Key = reader.ReadString();
                property.Value = (float) reader.ReadDouble();
                var listLength = reader.ReadVarInt();

                property.Modifiers = new Modifiers[listLength];
                for (var j = 0; j < listLength; j++)
                {
                    var item = new Modifiers
                    {
                        UUID = reader.ReadBigInteger(),
                        Amount = (float) reader.ReadDouble(),
                        Operation = reader.ReadSByte()
                    };

                    property.Modifiers[j] = item;
                }

                value[i] = property;
            }

            return value;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteInt(Count);

            foreach (var entry in _entries)
            {
                stream.WriteString(entry.Key);
                stream.WriteDouble(entry.Value);

                stream.WriteShort((short)entry.Modifiers.Length);
                foreach (var modifiers in entry.Modifiers)
                {
                    stream.WriteBigInteger(modifiers.UUID);
                    stream.WriteDouble(modifiers.Amount);
                    stream.WriteSByte(modifiers.Operation);
                }
            }
        }

        #endregion

        public bool Equals(EntityPropertyList other)
        {
            if (!Count.Equals(other.Count))
                return false;

            for (int i = 0; i < Count; i++)
                if (!this[i].Equals(other[i])) return false;
            
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((EntityPropertyList) obj);
        }

        public override int GetHashCode()
        {
            return _entries.GetHashCode() ^ Count.GetHashCode();
        }
    }
}
