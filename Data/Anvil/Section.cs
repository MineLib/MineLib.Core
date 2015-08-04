using System;
using System.Runtime.InteropServices;

namespace MineLib.Core.Data.Anvil
{
    // -- Full  - 12304 bytes.
    // -- Empty - 20    bytes.
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Section : IEquatable<Section>
    {
        public const int Width = 16;
        public const int Height = 16;
        public const int Depth = 16;

        public readonly Position ChunkPosition;

        public BlockList Blocks;

        public bool IsFilled;


        public Section(Position position)
        {
            ChunkPosition = position;

            Blocks = new BlockList(0);
            IsFilled = false;
        }

        public override string ToString()
        {
            return IsFilled ? "Filled" : "Empty";
        }

        public void BuildEmpty()
        {
            if(IsFilled)
                return;

            Blocks = new BlockList(Width, Height, Depth);

            IsFilled = true;
        }

        // REWRITE: Big indivilual work
        public void BuildFromNibbleData(byte[] blocks, byte[] blockLights, byte[] blockSkyLights)
        {
            if (IsFilled)
                return;

            Blocks = new BlockList(Width, Height, Depth);

            var blockLight = ToBytePerBlock(blockLights);
            var skyLight = ToBytePerBlock(blockSkyLights);

            for (int i = 0, j = 0; i < Width * Height * Depth; i++)
            {
                //var idMetadata = BitConverter.ToUInt16(new[] { blocks[j], blocks[j + 1] }, 0);
                var idMetadata = ((blocks[j + 1] << 8) | blocks[j]);

                var id = (ushort)(idMetadata >> 4);
                var meta = (byte)(idMetadata & 0x000F); // & 15

                Blocks[i] = new Block(id, meta, blockLight[i], skyLight[i]);

                j = j + 2;
            }

            IsFilled = true;
        }

        public void BuildFromBlocks(BlockList list)
        {
            if (IsFilled)
                return;

            Blocks = list;

            IsFilled = true;
        }

        public Block GetBlock(Position sectionPos)
        {
            if (!IsFilled)
                throw new IndexOutOfRangeException("Section is empty");

            return Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z];
        }

        public void SetBlock(Position sectionPos, Block block)
        {
            if (!IsFilled)
                BuildEmpty();

            var oldBlock = Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z];

            // REWRITE: Light recalculating or what? Remove it cause of deferred lightning
            block.Light = oldBlock.Light;
            block.SkyLight = oldBlock.SkyLight;

            Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z] = block;
        }

        public byte GetBlockLighting(Position sectionPos)
        {
            if (!IsFilled)
                throw new IndexOutOfRangeException("Section is empty");

            return Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z].Light;
        }

        public void SetBlockLighting(Position sectionPos, byte data)
        {
            if (!IsFilled)
                BuildEmpty();

            var block = Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z];
            block.Light = data;
            Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z] = block;
        }

        public byte GetBlockSkylight(Position sectionPos)
        {
            if (!IsFilled)
                throw new IndexOutOfRangeException("Section is empty");

            return Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z].SkyLight;
        }

        public void SetBlockSkylight(Position sectionPos, byte data)
        {
            if (!IsFilled)
                BuildEmpty();

            var block = Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z];
            block.SkyLight = data;
            Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z] = block;
        }

        #region Helping Methods

        public static Position GetSectionPositionByIndex(int index)
        {
            return new Position(
                index % 16,
                index / (16 * 16),
                (index / 16) % 16);
        }

	    public int GetIndexByPosition(Position pos)
	    {
		    return pos.X + ((pos.Y * 16) + pos.Z) * 16;
	    }

        public Position GetGlobalPositionByIndex(int index)
        {
            var sectionPos = GetSectionPositionByIndex(index);

            return new Position(
                Width * ChunkPosition.X + sectionPos.X,
                Height * ChunkPosition.Y + sectionPos.Y,
                Depth * ChunkPosition.Z + sectionPos.Z);
        }

        public Position GetGlobalPositionByArrayIndex(int x, int y, int z)
        {
            return GetGlobalPositionByPosition(new Position(x, y, z));
        }

		public Position GetGlobalPositionByPosition(Position pos)
		{
			return new Position(
				Width * ChunkPosition.X + pos.X,
				Height * ChunkPosition.Y + pos.Y,
				Depth * ChunkPosition.Z + pos.Z);
		}

		private static byte[] ToBytePerBlock(byte[] halfByteData)
        {
            if (halfByteData.Length != Width * Height * Depth / 2)
                throw new ArgumentOutOfRangeException("halfByteData", "Length != Half Byte Metadata length");

			var newMeta = new byte[Width * Height * Depth];

			for (var i = 0; i < halfByteData.Length; i++)
            {
                var data = halfByteData[i];
                var block1 = (byte) (data & 0x0F);
				var block2 = (byte) (data >> 4);
            
				newMeta[(i * 2)] = block1;
                newMeta[(i * 2) + 1] = block2;
            }

            return newMeta;
        }

        private static byte[] ToHalfBytePerBlock(byte[] byteData)
        {
            var newMeta = new byte[Width * Height * Depth / 2];

            if (byteData.Length != Width * Height * Depth)
                throw new ArgumentOutOfRangeException("byteData", "Length != Full Byte Metadata length");

            for (var i = 0; i < byteData.Length; i++)
            {
                var block1 = newMeta[i];
                var block2 = newMeta[i + 1];

                var halfByteData = (byte) ((block1 & 0x0F) | (block2 >> 4));
                newMeta[i] = halfByteData;
            }

            return newMeta;
        }

        #endregion

        public static bool operator ==(Section a, Section b)
        {
            if (!ReferenceEquals(a, b))
                return false;

            return a.Blocks == b.Blocks && a.IsFilled == b.IsFilled && a.ChunkPosition == b.ChunkPosition;
        }

        public static bool operator !=(Section a, Section b)
        {
            return !(a == b);
        }

        // You need to be a really freak to use it
        public bool Equals(Section section)
        {
            return Blocks.Equals(section.Blocks) && ChunkPosition.Equals(section.ChunkPosition);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Section) obj);
        }

        public override int GetHashCode()
        {
            return Blocks.GetHashCode() ^ ChunkPosition.GetHashCode();
        }
    }
}
