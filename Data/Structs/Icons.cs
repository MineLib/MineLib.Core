using System;

namespace MineLib.Core.Data.Structs
{
    public struct Icon : IEquatable<Icon>
    {
        public byte Direction;
        public byte Type;
        public int X;
        public int Y;
        
        public static bool operator ==(Icon a, Icon b)
        {
            return a.Direction == b.Direction && a.Type == b.Type && a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Icon a, Icon b)
        {
            return a.Direction != b.Direction && a.Type != b.Type && a.X != b.X && a.Y != b.Y;
        }

        public bool Equals(Icon other)
        {
            return Direction.Equals(other.Direction) && Type.Equals(other.Type) && X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Icon) obj);
        }

        public override int GetHashCode()
        {
            return Direction.GetHashCode() ^ Type.GetHashCode() ^ X.GetHashCode() ^ Y.GetHashCode();
        }
    }

}
