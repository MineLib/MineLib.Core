using System;
using System.Runtime.InteropServices;

using Aragas.Core.Data;

namespace MineLib.Core.Data
{
    /// <summary>
    /// Represents the location of an object in 3D space (int).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Position : IEquatable<Position>
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;


        public Position(int value) { X = Y = Z = value; }
        public Position(int x, int y, int z) { X = x; Y = y; Z = z; }
        public Position(Position p) { X = p.X; Y = p.Y; Z = p.Z; }

        public static Position FromLong(long value) => new Position((int) (value >> 38), (int) (value >> 26) & 0xFFF, (int) value << 38 >> 38);
        public long ToLong() => ((X & 0x3FFFFFF) << 38) | ((Y & 0xFFF) << 26) | (Z & 0x3FFFFFF);

        /// <summary>
        /// Converts this Position to a string.
        /// </summary>
        public override string ToString() => $"X: {X}, Y: {Y}, Z: {Z}";

        #region Math

        /// <summary>
        /// Calculates the distance between two Position objects.
        /// </summary>
        public double DistanceTo(Position other)
        {
            return Math.Sqrt(Square(other.X - X) +
                             Square(other.Y - Y) +
                             Square(other.Z - Z));
        }

        /// <summary>
        /// Calculates the square of a num.
        /// </summary>
        private static int Square(int num)
        {
            return num * num;
        }

        /// <summary>
        /// Finds the distance of this Position from Position.Zero
        /// </summary>
        public double Distance()
        {
            return DistanceTo(Zero);
        }

        public static Position Min(Position value1, Position value2)
        {
            return new Position(
                Math.Min(value1.X, value2.X),
                Math.Min(value1.Y, value2.Y),
                Math.Min(value1.Z, value2.Z)
                );
        }

        public static Position Max(Position value1, Position value2)
        {
            return new Position(
                Math.Max(value1.X, value2.X),
                Math.Max(value1.Y, value2.Y),
                Math.Max(value1.Z, value2.Z)
                );
        }

        #endregion

        #region Operators

        public static Position operator -(Position a) => new Position(-a.X, -a.Y, -a.Z);
        public static Position operator ++(Position a) => new Position(a.X, a.Y, a.Z) + 1;
        public static Position operator --(Position a) => new Position(a.X, a.Y, a.Z) - 1;

        public static bool operator ==(Position a, Position b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        public static bool operator !=(Position a, Position b) => !(a == b);
        
        public static Position operator +(Position a, Position b) => new Position(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Position operator -(Position a, Position b) => new Position(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Position operator *(Position a, Position b) => new Position(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        public static Position operator /(Position a, Position b) => new Position(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        public static Position operator %(Position a, Position b) => new Position(a.X % b.X, a.Y % b.Y, a.Z % b.Z);

        public static Position operator +(Position a, int b) => new Position(a.X + b, a.Y + b, a.Z + b);
        public static Position operator -(Position a, int b) => new Position(a.X - b, a.Y - b, a.Z - b);
        public static Position operator *(Position a, int b) => new Position(a.X * b, a.Y * b, a.Z * b);
        public static Position operator /(Position a, int b) => new Position(a.X / b, a.Y / b, a.Z / b);
        public static Position operator %(Position a, int b) => new Position(a.X % b, a.Y % b, a.Z % b);

        public static Position operator +(int a, Position b) => new Position(a + b.X, a + b.Y, a + b.Z);
        public static Position operator -(int a, Position b) => new Position(a - b.X, a - b.Y, a - b.Z);
        public static Position operator *(int a, Position b) => new Position(a * b.X, a * b.Y, a * b.Z);
        public static Position operator /(int a, Position b) => new Position(a / b.X, a / b.Y, a / b.Z);
        public static Position operator %(int a, Position b) => new Position(a % b.X, a % b.Y, a % b.Z);

        public static implicit operator Position(Vector3 a) => new Position((int) a.X, (int) a.Y, (int) a.Z);
        public static implicit operator Vector3(Position a) => new Vector3(a.X, a.Y, a.Z);

        #endregion

        #region Constants

        public static readonly Position Zero = new Position(0);
        public static readonly Position One = new Position(1);

        public static readonly Position Up = new Position(0, 1, 0);
        public static readonly Position Down = new Position(0, -1, 0);
        public static readonly Position Left = new Position(-1, 0, 0);
        public static readonly Position Right = new Position(1, 0, 0);
        public static readonly Position Backwards = new Position(0, 0, -1);
        public static readonly Position Forwards = new Position(0, 0, 1);

        public static readonly Position East = new Position(1, 0, 0);
        public static readonly Position West = new Position(-1, 0, 0);
        public static readonly Position North = new Position(0, 0, -1);
        public static readonly Position South = new Position(0, 0, 1);

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Position) obj);
        }
        public bool Equals(Position other) => other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);

        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
    }
}
