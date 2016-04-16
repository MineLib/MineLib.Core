using System;

using MineLib.Core.Data.Anvil;

namespace MineLib.Core.Data
{
    public class BlockPosition : IEquatable<BlockPosition>
    {
        private ushort BlockIDMeta { get; }
        public Block Block => new Block((ushort) (BlockIDMeta >> 4), (byte) (BlockIDMeta & 15));
        public Position Coordinates { get; }


        public BlockPosition(ushort blockIDMeta, Position coordinates)
        {
            BlockIDMeta = blockIDMeta;
            Coordinates = coordinates;
        }
        public BlockPosition(Block block, Position coordinates)
        {
            BlockIDMeta = (ushort) (block.ID << 4 | (block.Meta & 15));
            Coordinates = coordinates;
        }


        public static bool operator ==(BlockPosition a, BlockPosition b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.BlockIDMeta == b.BlockIDMeta && a.Coordinates == b.Coordinates;
        }
        public static bool operator !=(BlockPosition a, BlockPosition b) => !(a == b);

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((BlockPosition) obj);
        }
        public bool Equals(BlockPosition other) => BlockIDMeta.Equals(other.BlockIDMeta) && Coordinates.Equals(other.Coordinates);

        public override int GetHashCode() => BlockIDMeta.GetHashCode() ^ Coordinates.GetHashCode();
    }
}
