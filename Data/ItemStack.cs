using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using fNbt;
using fNbt.Serialization;

using MineLib.Core.Extensions;
using MineLib.Core.IO;

namespace MineLib.Core.Data
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ItemStack  : IEquatable<ItemStack>
    {
        public short ID;
        public byte Count;
        public short Damage; // Level
        [IgnoreOnNull]
        public NbtCompound Nbt;
        [NbtIgnore]
        public byte Index;

        [NbtIgnore]
        public bool Empty { get { return ID == -1; } }

        public static ItemStack EmptyStack { get { return new ItemStack(-1); } }


        public ItemStack(short id)
        {
            ID = id;
            Damage = 0;
            Count = 1;
            Nbt = null;
            Index = 0;
        }

        public ItemStack(short id, byte count) : this(id)
        {
            Count = count;
        }

        public ItemStack(short id, byte count, short damage) : this(id, count)
        {
            Damage = damage;
        }

        public ItemStack(short id, byte count, short damage, NbtCompound nbtData) : this(id, count, damage)
        {
            Nbt = nbtData;

            if (Count == 0)
            {
                ID = -1;
                Damage = 0;
                Nbt = null;
            }
        }


        public override string ToString()
        {
            if (Empty)
                return "(Empty)";
            var result = "ID: " + ID;
            if (Count != 1) result += "; Count: " + Count;
            if (Damage != 0) result += "; Damage: " + Damage;
            if (Nbt != null) result += Environment.NewLine + Nbt;
            return "(" + result + ")";
        }


        #region NBT

        public static ItemStack FromNbt(NbtCompound compound)
        {
            var itemStack = EmptyStack;
            itemStack.ID = compound.Get<NbtShort>("id").Value;
            itemStack.Damage = compound.Get<NbtShort>("Damage").Value;
            itemStack.Count = compound.Get<NbtByte>("Count").Value;
            itemStack.Index = compound.Get<NbtByte>("Slot").Value;
            if (compound.Get<NbtCompound>("tag") != null)
                itemStack.Nbt = compound.Get<NbtCompound>("tag");
            return itemStack;
        }

        public NbtCompound ToNbt()
        {
            var nbtCompound = new NbtCompound();
            nbtCompound.Add(new NbtShort("id", ID));
            nbtCompound.Add(new NbtShort("Damage", Damage));
            nbtCompound.Add(new NbtByte("Count", Count));
            nbtCompound.Add(new NbtByte("Slot", Index));
            if (Nbt != null)
                nbtCompound.Add(new NbtCompound("tag"));
            return nbtCompound;
        }

        #endregion


        #region Network

        public static ItemStack FromReader(IProtocolDataReader reader)
        {
            var itemStack = new ItemStack(reader.ReadShort());

            if (itemStack.Empty)
                return itemStack;
            
            itemStack.Count = reader.ReadByte();
            itemStack.Damage = reader.ReadShort();

            var length = reader.ReadVarInt();
            if (length == -1 || length == 0)
                return itemStack;

            itemStack.Nbt = new NbtCompound();
            var buffer = reader.ReadByteArray(length);
            var nbt = new NbtFile();
            nbt.LoadFromBuffer(buffer, 0, length, NbtCompression.GZip, null);
            itemStack.Nbt = nbt.RootTag;
            
            return itemStack;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteShort(ID);
            if (Empty)
                return;

            stream.WriteByte(Count);
            stream.WriteShort(Damage);
            if (Nbt == null)
            {
                stream.WriteShort(-1);
                return;
            }

            var file = new NbtFile(Nbt);
            file.SaveToProtocolStream(stream, NbtCompression.GZip);
        }

        #endregion


        #region Operators

        public static bool operator ==(ItemStack left, ItemStack right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ItemStack left, ItemStack right)
        {
            return !left.Equals(right);
        }

        #endregion
        

        public bool Equals(ItemStack other)
        {
            return ID.Equals(other.ID) && Damage.Equals(other.Damage) && Count.Equals(other.Count) && Index.Equals(other.Index) && Equals(Nbt, other.Nbt);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((ItemStack) obj);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode() ^ Damage.GetHashCode() ^ Count.GetHashCode() ^ Index.GetHashCode() ^ (Nbt != null ? Nbt.GetHashCode() : 0);
        }
    }

    public class ItemStackList : IEquatable<ItemStackList>
    {
        private readonly List<ItemStack> _entries;

        public ItemStackList()
        {
            _entries = new List<ItemStack>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public ItemStack this[int index]
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

        public static ItemStackList FromReader(IProtocolDataReader reader)
        {
            var value = new ItemStackList();

            var count = reader.ReadShort();
            for (int i = 0; i < count; i++)
            {
                var slot = ItemStack.FromReader(reader);
                value._entries.Add(slot);
            }

            return value;
        }

        public void ToStream(IProtocolStream stream)
        {
            foreach (var itemStack in _entries)
            {
                //if (itemStack.ID == 1) // AIR
                //    return;

                stream.WriteShort(itemStack.ID);
                stream.WriteShort(itemStack.Damage);
                stream.WriteShort(itemStack.Count);

                //if (itemStack.Empty)
                //    stream.WriteSByte(itemStack.Slot);

                if (itemStack.Nbt == null)
                {
                    stream.WriteShort(-1);
                    return;
                }
            }
        }

        #endregion

        #region Operators

        public static bool operator ==(ItemStackList left, ItemStackList right)
        {
            return left == right;
        }

        public static bool operator !=(ItemStackList left, ItemStackList right)
        {
            return left != right;
        }

        #endregion

        public bool Equals(ItemStackList other)
        {
            if (other.Count != Count)
                return false;

            for (int i = 0; i < Count; i++)
                if (other[i] != this[i]) return false;
            
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(ItemStackList)) return false;
            return Equals((ItemStackList)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = _entries.GetHashCode();
                result = (result * 397) ^ Count.GetHashCode();
                return result;
            }
        }
    }
}
