//#define FULLBLOCK

using System;
using System.Runtime.InteropServices;

namespace MineLib.Core.Data.Anvil
{
#if FULLBLOCK
    // -- Full  - 5 bytes.
    // -- Empty - 5 bytes.
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Block : IEquatable<Block>
	{
        public ushort ID { get; }

        public byte Meta { get; }

        public byte SkyLight { get; set; }
        public byte Light { get; set; }


		public Block(ushort id) : this()
		{
            ID = id;
		    Meta = 0;
			SkyLight = 0;
            Light = 0;
		}

        public Block(ushort id, byte meta) : this()
		{
            ID = id;
            Meta = meta;
            SkyLight = 0;
            Light = 0;
		}

        public Block(ushort id, byte meta, byte light) : this()
		{
            ID = id;
            Meta = meta;
            SkyLight = 0;
            Light = light;
		}

        public Block(ushort id, byte meta, byte light, byte skyLight) : this()
		{
            ID = id;
            Meta = meta;
            SkyLight = skyLight;
            Light = light;
		}

		public override string ToString() => $"ID: {ID}, Meta: {Meta}, Light: {Light}, SkyLight: {SkyLight}";

        public static bool operator ==(Block a, Block b) => a.ID == b.ID && a.Meta == b.Meta && a.Light == b.Light && a.SkyLight == b.SkyLight;
        public static bool operator !=(Block a, Block b) => !(a == b);

        public override bool Equals(object obj)
		{
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Block) obj);
		}
        public bool Equals(Block other) => other.ID.Equals(ID) && other.Meta.Equals(Meta);

        public override int GetHashCode()
		{
			return ID.GetHashCode() ^ Meta.GetHashCode();
		}

        public bool IsAir => ID == 0;

        public bool IsTransparent
        {
            get
            {
                foreach (var i in TransparentBlocks)
                    if (i == ID) return true;
                return false;
            }
        }

        public bool IsFluid
        {
            get
            {
                foreach (var i in FluidBlocks)
                    if (i == ID) return true;
                return false;
            }
        }

        public static readonly int[] FluidBlocks =
        {
            8, 9, 10, 11
        };

        public static readonly int[] TransparentBlocks =
        {
            6, 18, 20, 27, 28, 30, 31, 32, 37, 38, 39, 40, 161
        };
    }
#else
    // -- Full  - 3 bytes.
    // -- Empty - 3 bytes.
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Block : IEquatable<Block>
	{
        public static Block Empty => new Block(0);


		private readonly ushort IDMeta;
		private byte SkyAndBlockLight;

		public ushort ID => (ushort) (IDMeta >> 4);
        public byte Meta => (byte) (IDMeta & 0x000F);

        public byte SkyLight
		{
			get { return (byte)(SkyAndBlockLight >> 4); }
			set { SkyAndBlockLight = (byte) (value << 4      | Light & 0x0F); }
		}
		public byte Light
		{
			get { return (byte)(SkyAndBlockLight & 0x0F); }
			set { SkyAndBlockLight = (byte) (SkyLight << 4   | value & 0x0F); }
		}

		public Block(ushort id)
		{
			IDMeta = (ushort)(id << 4 & 0xFFF0              | 0 & 0x000F);
			SkyAndBlockLight = 0;
		}

		public Block(ushort id, byte meta)
		{
			IDMeta = (ushort)(id << 4 & 0xFFF0              | meta & 0x000F);
			SkyAndBlockLight = 0;
		}

		public Block(ushort id, byte meta, byte light)
		{
			IDMeta = (ushort)(id << 4 & 0xFFF0              | meta & 0x000F);
			SkyAndBlockLight = (byte)(0 << 4 & 0xF0         | light & 0x0F);
		}

		public Block(ushort id, byte meta, byte light, byte skyLight)
		{
			IDMeta = (ushort)(id << 4 & 0xFFF0              | meta & 0x000F);
			SkyAndBlockLight = (byte)(skyLight << 4 & 0xF0  | light & 0x0F);
		}

		public override string ToString() => $"ID: {ID}, Meta: {Meta}, Light: {Light}, SkyLight: {SkyLight}";

        public static bool operator ==(Block a, Block b) => a.IDMeta == b.IDMeta && a.SkyAndBlockLight == b.SkyAndBlockLight;
        public static bool operator !=(Block a, Block b) => !(a == b);

        public override bool Equals(object obj)
		{
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Block) obj);
		}
        public bool Equals(Block other) => other.IDMeta.Equals(IDMeta);

        public override int GetHashCode() => IDMeta.GetHashCode();

        public bool IsAir => ID == 0;

        public bool IsTransparent
        {
            get
            {
                foreach (var i in TransparentBlocks)
                    if (i == ID) return true;
                return false;
            }
        }

        public bool IsFluid
        {
            get
            {
                foreach (var i in FluidBlocks)
                    if (i == ID) return true;
                return false;
            }
        }

        public static readonly int[] FluidBlocks = 
        {
            8, 9, 10, 11
        };

        public static readonly int[] TransparentBlocks = 
        {
            6, 18, 20, 27, 28, 30, 31, 32, 37, 38, 39, 40, 161
        };
	}
#endif
    
    public class BlockList
    {
        public int XSize { get; }
        public int YSize { get; }
        public int ZSize { get; }

        private Block[] Blocks { get; }


        public BlockList(int size)
        {
            Blocks = new Block[size];
        }
        public BlockList(int xSize, int ySize, int zSize)
        {
            XSize = xSize;
            YSize = ySize;
            ZSize = zSize;

            Blocks = new Block[XSize * YSize * ZSize];
        }


        public Block this[Position pos]
        {
            get { return Blocks[pos.X + ((pos.Y * YSize) + pos.Z) * XSize]; }
            set { Blocks[pos.X + ((pos.Y * YSize) + pos.Z) * XSize] = value; }
        }
        public Block this[int x, int y, int z]
        {
            get { return Blocks[x + ((y * YSize) + z) * XSize]; }
            set { Blocks[x + ((y * YSize) + z) * XSize] = value; }
        }
        public Block this[int index]
        {
            get { return Blocks[index]; }
            set { Blocks[index] = value; }
        }
    }
}