using System;

using MineLib.Core.Data.Anvil;

namespace MineLib.Core.Data.Structs
{
    public struct BlockPosition : IEquatable<BlockPosition>
    {
        //public ushort BlockIDMeta;
        public Block Block;
        public Position Coordinates;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((BlockPosition) obj);
        }
        public bool Equals(BlockPosition other) => Block == other.Block && Coordinates == other.Coordinates;

        public override int GetHashCode() => Block.GetHashCode() ^ Coordinates.GetHashCode();
    }
}
