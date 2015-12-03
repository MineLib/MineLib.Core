using System;

using MineLib.Core.Extensions;

namespace MineLib.Core.Data.Structs
{
    public struct ChunkColumnMetadata : IEquatable<ChunkColumnMetadata>
    {
        public Coordinates2D Coordinates;
        public ushort PrimaryBitMap;

        // -- Debugging
        public bool[] PrimaryBitMapConverted => Helper.ConvertFromUShort(PrimaryBitMap);
        // -- Debugging

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((ChunkColumnMetadata) obj);
        }

        public bool Equals(ChunkColumnMetadata other)
        {
            return Coordinates == other.Coordinates && PrimaryBitMap == other.PrimaryBitMap;
        }

        public override int GetHashCode()
        {
            return Coordinates.GetHashCode() ^ PrimaryBitMap.GetHashCode();
        }
    }
}
