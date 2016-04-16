using System;
using System.Runtime.InteropServices;

namespace MineLib.Core.Data
{
    /// <summary>
    /// Represents mostly Chunk coordinates
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Coordinates2D : IEquatable<Coordinates2D>
    {
        public readonly int X;
        public readonly int Z;


        public Coordinates2D(int value) { X = Z = value; }
        public Coordinates2D(int x, int z) { X = x; Z = z; }
        public Coordinates2D(Coordinates2D c) { X = c.X; Z = c.Z; }


        /// <summary>
        /// Converts this Coordinates2D to a string.
        /// </summary>
        public override string ToString() => $"X: {X}, Z: {Z}";

        #region Math

        private static int Square(int num) => num * num;

        /// <summary>
        /// Calculates the distance between two Coordinates2D objects.
        /// </summary>
        public double DistanceTo(Coordinates2D other) => Math.Sqrt(Square(other.X - X) + Square(other.Z - Z));
        
        /// <summary>
        /// Finds the distance of this Coordinates2D from Coordinates2D.Zero
        /// </summary>
        public double Distance() => DistanceTo(Zero);

        public static Coordinates2D Min(Coordinates2D a, Coordinates2D b) => new Coordinates2D(Math.Min(a.X, b.X), Math.Min(a.Z, b.Z));
        public static Coordinates2D Max(Coordinates2D a, Coordinates2D b) => new Coordinates2D(Math.Max(a.X, b.X), Math.Max(a.Z, b.Z));

        #endregion

        #region Operators

        public static bool operator ==(Coordinates2D a, Coordinates2D b) => a.X == b.X && a.Z == b.Z;
        public static bool operator !=(Coordinates2D a, Coordinates2D b) => !(a == b);

        public static Coordinates2D operator +(Coordinates2D a, Coordinates2D b) => new Coordinates2D(a.X + b.X, a.Z + b.Z);
        public static Coordinates2D operator -(Coordinates2D a, Coordinates2D b) => new Coordinates2D(a.X - b.X, a.Z - b.Z);
        public static Coordinates2D operator -(Coordinates2D a) => new Coordinates2D(-a.X, -a.Z);
        public static Coordinates2D operator *(Coordinates2D a, Coordinates2D b) => new Coordinates2D(a.X * b.X, a.Z * b.Z);
        public static Coordinates2D operator /(Coordinates2D a, Coordinates2D b) => new Coordinates2D(a.X / b.X, a.Z / b.Z);
        public static Coordinates2D operator %(Coordinates2D a, Coordinates2D b) => new Coordinates2D(a.X % b.X, a.Z % b.Z);

        public static Coordinates2D operator +(Coordinates2D a, int b) => new Coordinates2D(a.X + b, a.Z + b);
        public static Coordinates2D operator -(Coordinates2D a, int b) => new Coordinates2D(a.X - b, a.Z - b);
        public static Coordinates2D operator *(Coordinates2D a, int b) => new Coordinates2D(a.X * b, a.Z * b);
        public static Coordinates2D operator /(Coordinates2D a, int b) => new Coordinates2D(a.X / b, a.Z / b);
        public static Coordinates2D operator %(Coordinates2D a, int b) => new Coordinates2D(a.X % b, a.Z % b);

        public static Coordinates2D operator +(int a, Coordinates2D b) => new Coordinates2D(a + b.X, a + b.Z);
        public static Coordinates2D operator -(int a, Coordinates2D b) => new Coordinates2D(a - b.X, a - b.Z);
        public static Coordinates2D operator *(int a, Coordinates2D b) => new Coordinates2D(a * b.X, a * b.Z);
        public static Coordinates2D operator /(int a, Coordinates2D b) => new Coordinates2D(a / b.X, a / b.Z);
        public static Coordinates2D operator %(int a, Coordinates2D b) => new Coordinates2D(a % b.X, a % b.Z);

        #endregion

        #region Constants

        public static readonly Coordinates2D Zero = new Coordinates2D(0);
        public static readonly Coordinates2D One = new Coordinates2D(1);
        
        public static readonly Coordinates2D Forward = new Coordinates2D(0, 1);
        public static readonly Coordinates2D Backward = new Coordinates2D(0, -1);
        public static readonly Coordinates2D Left = new Coordinates2D(-1, 0);
        public static readonly Coordinates2D Right = new Coordinates2D(1, 0);

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Coordinates2D) obj);
        }
        public bool Equals(Coordinates2D other) => other.X.Equals(X) && other.Z.Equals(Z);

        public override int GetHashCode() => X.GetHashCode() ^ Z.GetHashCode();
    }
}