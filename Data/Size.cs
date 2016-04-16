using System.Runtime.InteropServices;

using MineLib.Core.Extensions;

namespace MineLib.Core.Data
{
    /// <summary>
    /// Represents the size of an object in 3D space.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Size
    {
        public readonly float Width;
        public readonly float Height;
        public readonly float Depth;


        public Size(float width, float height, float depth) { Width = width; Height = height; Depth = depth; }
        public Size(double width, double height, double depth) { Width = (float) width; Height = (float) height; Depth = (float) depth; }
        public Size(Size s) { Width = s.Width; Height = s.Height; Depth = s.Depth; }


        /// <summary>
        /// Converts this Size to a string.
        /// </summary>
        public override string ToString() => $"Width: {Width}, Height: {Height}, Depth: {Depth}";

        public static bool operator ==(Size a, float b) => a.Width == b && a.Height == b && a.Depth == b;
        public static bool operator !=(Size a, float b) => !(a == b);

        public static bool operator ==(Size a, Size b) => a.Width == b.Width && a.Height == b.Height && a.Depth == b.Depth;
        public static bool operator !=(Size a, Size b) => !(a == b);

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            if(obj is float)
                return Equals((float) obj);

            if (obj is Size)
                return Equals((Size) obj);

            return false;
        }
        public bool Equals(float other) => other.Equals(Width) && other.Equals(Height) && other.Equals(Depth);
        public bool Equals(Size other) => other.Width.Equals(Width) && other.Height.Equals(Height) && other.Depth.Equals(Depth);

        public override int GetHashCode() => Width.GetHashCode() ^ Height.GetHashCode() ^ Depth.GetHashCode();
    }
}