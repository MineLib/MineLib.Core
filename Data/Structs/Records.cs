using System;

namespace MineLib.Core.Data.Structs
{
    public struct Record : IEquatable<Record>
    {
        public ushort BlockIDMeta;
        public Position Coordinates;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Record) obj);
        }

        public bool Equals(Record other)
        {
            return BlockIDMeta == other.BlockIDMeta && Coordinates == other.Coordinates;
        }

        public override int GetHashCode()
        {
            return BlockIDMeta.GetHashCode() ^ Coordinates.GetHashCode();
        }
    }
}
