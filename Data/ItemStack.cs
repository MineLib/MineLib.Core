using System;

using fNbt;
using fNbt.Serialization;

namespace MineLib.Core.Data
{
    public class ItemStack  : IEquatable<ItemStack>
    {
        public static ItemStack EmptyStack => new ItemStack(-1);

        public short ID
        {
            get { return _id; }
            set
            {
                _id = value;
                if (_id == -1)
                {
                    _count = 0;
                    Damage = 0;
                    Nbt = null;
                }
            }
        }
        public byte Count
        {
            get { return _count; }
            set
            {
                _count = value;
                if (_count == 0)
                {
                    _id = -1;
                    Damage = 0;
                    Nbt = null;
                }
            }
        }
        public short Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }
        [IgnoreOnNull]
        public NbtCompound Nbt
        {
            get { return _nbt; }
            set
            {
                _nbt = value;
                if (Count == 0)
                {
                    ID = -1;
                    Damage = 0;
                    Nbt = null;
                }
            }
        }
        [NbtIgnore]
        public byte Index
        {
            get { return _index; }
            set { _index = value; }
        }

        private short _id;
        private byte _count;
        private short _damage;
        private NbtCompound _nbt;
        private byte _index;

        [NbtIgnore]
        public bool Empty => ID == -1;


        public ItemStack(short id) { ID = id; }
        public ItemStack(short id, byte count) : this(id) { Count = count; }
        public ItemStack(short id, byte count, short damage) : this(id, count) { Damage = damage; }
        public ItemStack(short id, byte count, short damage, NbtCompound nbtData) : this(id, count, damage) { Nbt = nbtData; }


        public override string ToString() => Empty ? "(Empty)" : $"(ID: {ID}, Count: {Count}, Damage: {Damage}, {Environment.NewLine} {Nbt})";

        #region NBT

        public static ItemStack FromNbt(NbtCompound compound)
        {
            var itemStack = EmptyStack;
            itemStack.ID = compound.Get<NbtShort>("id")?.Value ?? 0;
            itemStack.Damage = compound.Get<NbtShort>("Damage")?.Value ?? 0;
            itemStack.Count = compound.Get<NbtByte>("Count")?.Value ?? 0;
            itemStack.Index = compound.Get<NbtByte>("Slot")?.Value ?? 0;
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

        public static bool operator ==(ItemStack a, ItemStack b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return  a.ID == b.ID && a.Damage == b.Damage && a.Count == b.Count && a.Index == b.Index && a.Nbt == b.Nbt;
        }
        public static bool operator !=(ItemStack a, ItemStack b) => !(a == b);
        
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((ItemStack) obj);
        }
        public bool Equals(ItemStack other) => ID.Equals(other.ID) && Damage.Equals(other.Damage) && Count.Equals(other.Count) && Index.Equals(other.Index) && Equals(Nbt, other.Nbt);

        public override int GetHashCode() => ID.GetHashCode() ^ Damage.GetHashCode() ^ Count.GetHashCode() ^ Index.GetHashCode() ^ (Nbt?.GetHashCode() ?? 0);
    }
}
