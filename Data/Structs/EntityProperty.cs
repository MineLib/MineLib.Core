using System;

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
}
