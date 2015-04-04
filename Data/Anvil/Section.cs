using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MineLib.Network.Data.Anvil
{
    // -- Full  - 12304 bytes.
    // -- Empty - 20    bytes.
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Section : IEquatable<Section>
    {
        public const int Width = 16;
        public const int Height = 16;
        public const int Depth = 16;

        // 12
        public Position ChunkPosition;
        // 7
        public Block[,,] Blocks;
        // 1
        public bool IsFilled;


        public Section(Position position)
        {
            ChunkPosition = position;

            Blocks = new Block[0, 0, 0];
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

            Blocks = new Block[Width, Height, Depth];

            IsFilled = true;
        }

        public void BuildFromNibbleData(byte[] blocks, byte[] blockLights, byte[] blockSkyLights)
        {
            if(IsFilled) 
                return;

            Blocks = new Block[Width, Height, Depth];

            var blockLight = ToBytePerBlock(blockLights);
            var skyLight = ToBytePerBlock(blockSkyLights);

            for (int i = 0, j = 0; i < Width * Height * Depth; i++)
            {
                //var idMetadata = (ushort)(blocks[j] + blocks[j + 1]);
                var idMetadata = BitConverter.ToUInt16(new byte[2] { blocks[j], blocks[j + 1] }, 0);

                // TODO: Add auto Coordinate calculator

                var id = (ushort)(idMetadata >> 4);
                var meta = (byte)(idMetadata & 0x000F); // & 15

                //var id = (ushort)(idMetadata & 0xff);
                //var meta = (byte)(idMetadata >> 8); // & 15

                var sectionPos = GetSectionPositionByIndex(i);
		        Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z] = new Block(id, meta, blockLight[i], skyLight[i]);

				j++;
				j++;
			}

            IsFilled = true;
        }

        public void BuildFromBlocks(Block[, ,] blocks)
        {
            if (IsFilled)
                return;

            Blocks = blocks;

            IsFilled = true;
        }

        public Block GetBlock(Position sectionPos)
        {
            if (!IsFilled)
                throw new AccessViolationException("Section is empty");

            return Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z];
        }

        public void SetBlock(Position sectionPos, Block block)
        {
            if (!IsFilled)
                BuildEmpty();

            var oldBlock = Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z];

            // I don't think that these values will change
            block.Light = oldBlock.Light;
            block.SkyLight = oldBlock.SkyLight;

            Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z] = block;
        }

        public byte GetBlockLighting(Position sectionPos)
        {
            if (!IsFilled)
                throw new AccessViolationException("Section is empty");

            return Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z].Light;
        }

        public void SetBlockLighting(Position sectionPos, byte data)
        {
            if (!IsFilled)
                BuildEmpty();

            Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z].Light = data;
        }

        public byte GetBlockSkylight(Position sectionPos)
        {
            if (!IsFilled)
                throw new AccessViolationException("Section is empty");

            return Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z].SkyLight;
        }

        public void SetBlockSkylight(Position sectionPos, byte data)
        {
            if (!IsFilled)
                BuildEmpty();

            Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z].SkyLight = data;
        }

        #region Helping Methods

        public static Position GetSectionPositionByIndex(int index)
        {
            return new Position(
                index % 16,
                index / (16 * 16),
                (index / 16) % 16
            );
        }

	    public int GetIndexByPosition(Position pos)
	    {
		    return pos.X + ((pos.Y * 16) + pos.Z) * 16;
	    }

        public Position GetGlobalPositionByArrayIndex(Position pos)
        {
            return GetGlobalPositionByArrayIndex(pos.X, pos.Y, pos.Z);
        }

        public Position GetGlobalPositionByArrayIndex(int index1, int index2, int index3)
        {
	        return GetGlobalPositionByPosition(new Position(index1, index2, index3));
			//return GetGlobalPositionByIndex(index1 + Width * (index2 + Depth * index3));
        }

        public Position GetGlobalPositionByIndex(int index)
        {
            var sectionPos = GetSectionPositionByIndex(index);

            return new Position(
                Width * ChunkPosition.X + sectionPos.X,
                Height * ChunkPosition.Y + sectionPos.Y,
                Depth * ChunkPosition.Z + sectionPos.Z
            );
        }

		public Position GetGlobalPositionByPosition(Position pos)
		{
			return new Position(
				Width * ChunkPosition.X + pos.X,
				Height * ChunkPosition.Y + pos.Y,
				Depth * ChunkPosition.Z + pos.Z
			);
		}

		private static byte[] ToBytePerBlock(byte[] halfByteData)
        {
            if (halfByteData.Length != Width * Height * Depth / 2)
                throw new ArgumentOutOfRangeException("halfByteData", "Length != Half Byte Metadata length");

			var newMeta = new byte[Width * Height * Depth];

			for (var i = 0; i < halfByteData.Length; i++)
            {
                var data = halfByteData[i];
				//var block2 = (byte)((data >> 4) & 0xF);
				//var block1 = (byte)(data & 0xF);
				var block2 = (byte)(data >> 4);
				var block1 = (byte)(data & 0x0F);

				newMeta[(i * 2)] = block1;
                newMeta[(i * 2) + 1] = block2;
            }

            return newMeta;
        }

        private static byte[] ToHalfBytePerBlock(IList<byte> byteData)
        {
            var newMeta = new byte[Width * Height * Depth / 2];

            if (byteData.Count != Width * Height * Depth)
                throw new ArgumentOutOfRangeException("byteData", "Length != Full Byte Metadata length");

            for (var i = 0; i < byteData.Count; i++)
            {
                // TODO: Convert Full Byte Metadata to Half Byte
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
            if (obj.GetType() != typeof(Section))
                return false;

            return Equals((Section) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = Blocks.GetHashCode();
                result = (result * 397) ^ ChunkPosition.GetHashCode();
                return result;
            }
        }
    }
}
