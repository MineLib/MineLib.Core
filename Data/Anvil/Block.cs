//#define FULLBLOCK

using System;
using System.Runtime.InteropServices;

namespace MineLib.Core.Data.Anvil
{	
#if FULLBLOCK
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Block : IEquatable<Block>
	{
        public ushort ID { get; set; }

        public byte Meta { get; set; }

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

		public override string ToString()
		{
			return String.Format("ID: {0}, Meta: {1}, Light: {2}, SkyLight: {3}", ID, Meta, Light, SkyLight);
		}

		public static bool operator ==(Block a, Block b)
		{
            return a.ID == b.ID && a.Meta == b.Meta && a.Light == b.Light && a.SkyLight == b.SkyLight;
		}

		public static bool operator !=(Block a, Block b)
		{
			return !(a == b);
		}

		public bool Equals(Block other)
		{
            return other.ID.Equals(ID) && other.Meta.Equals(ID);
		}

		public override bool Equals(object obj)
		{
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Block) obj);
		}

		public override int GetHashCode()
		{
			return IDMeta.GetHashCode();
		}

        public bool IsAir { get { return ID == 0; } }

        public bool IsTransparent { get { return ID == 18 || ID == 161; } }

        public bool IsFluid { get { return ID == 18 || ID == 161; } }

        public static readonly int[] TransparentBlocks = 
        {
            6, 8, 9, 18, 20, 27, 28, 30,  31, 32, 37, 38, 39, 40, 
        };
	}
#else
    // -- Full  - 3 bytes.
    // -- Empty - 3 bytes.
    // -- Performace cost isn't too high. We are handling maximum 1kk, loose ~5 ms, but win 10mb.
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Block : IEquatable<Block>
	{
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

		public override string ToString()
		{
			return $"ID: {ID}, Meta: {Meta}, Light: {Light}, SkyLight: {SkyLight}";
		}

		public static bool operator ==(Block a, Block b) => a.IDMeta == b.IDMeta && a.SkyAndBlockLight == b.SkyAndBlockLight;
        public static bool operator !=(Block a, Block b) => a.IDMeta != b.IDMeta && a.SkyAndBlockLight != b.SkyAndBlockLight;

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
        public readonly int XSize;
        public readonly int YSize;
        public readonly int ZSize;

        private readonly Block[] _blocks;

        public BlockList(int size)
        {
            _blocks = new Block[size];
        }

        public BlockList(int xSize, int ySize, int zSize)
        {
            XSize = xSize;
            YSize = ySize;
            ZSize = zSize;

            _blocks = new Block[XSize * YSize * ZSize];
        }

        public Block this[Position pos]
        {
            get { return _blocks[pos.X + ((pos.Y * YSize) + pos.Z) * XSize]; }
            set { _blocks[pos.X + ((pos.Y * YSize) + pos.Z) * XSize] = value; }
        }

        public Block this[int x, int y, int z]
        {
            get { return _blocks[x + ((y * YSize) + z) * XSize]; }
            set { _blocks[x + ((y * YSize) + z) * XSize] = value; }
        }

        public Block this[int index]
        {
            get { return _blocks[index]; }
            set { _blocks[index] = value; }
        }
    }
}