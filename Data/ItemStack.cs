using System;
using System.Runtime.InteropServices;

using fNbt;
using fNbt.Serialization;

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
        public bool Empty => ID == -1;

        public static ItemStack EmptyStack => new ItemStack(-1);


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
}
