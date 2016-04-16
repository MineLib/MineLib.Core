using System;
using System.Runtime.InteropServices;

using MineLib.Core.Extensions;

namespace MineLib.Core.Data
{
    /// <summary>
    /// Represents mostly head location of an entity
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Rotation : IEquatable<Rotation>
    {
        public readonly float Pitch;
        public readonly float Yaw;
        public readonly float Roll;


        public Rotation(float pitch, float yaw, float roll) { Pitch = pitch; Yaw = yaw; Roll = roll; }
        public Rotation(double pitch, double yaw, double roll) { Pitch = (float) pitch; Yaw = (float) yaw; Roll = (float) roll; }
        public Rotation(Rotation r) { Pitch = r.Pitch;  Yaw = r.Yaw; Roll = r.Roll; }


        /// <summary>
        /// Converts this Rotation to a string.
        /// </summary>
        public override string ToString() => $"Pitch: {Pitch}, Yaw: {Yaw}, Roll: {Roll}";

        public static bool operator ==(Rotation a, Rotation b) => a.Pitch == b.Pitch && a.Yaw == b.Yaw && a.Roll == b.Roll;
        public static bool operator !=(Rotation a, Rotation b) => !(a == b);

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Rotation) obj);         
        }
        public bool Equals(Rotation other) => other.Pitch.Equals(Pitch) && other.Yaw.Equals(Yaw) && other.Roll.Equals(Roll);

        public override int GetHashCode() => Pitch.GetHashCode() ^ Yaw.GetHashCode() ^ Roll.GetHashCode();
    }
}
