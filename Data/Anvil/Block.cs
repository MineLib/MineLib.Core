using System;
using System.Runtime.InteropServices;

namespace MineLib.Network.Data.Anvil
{
	// -- Full  - 3 bytes.
	// -- Empty - 3 bytes.
	// -- Performace cost isn't too high. We are handling maximum 1kk, loose ~5 ms, but win 10mb.
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Block : IEquatable<Block>
	{
		private readonly ushort IDMeta;
		private byte SkyAndBlockLight;

		public ushort ID { get { return (ushort) (IDMeta >> 4); } }

		public byte Meta { get { return (byte) (IDMeta & 0x000F); } }

		public byte SkyLight
		{
			get { return (byte)(SkyAndBlockLight >> 4); }
			set { SkyAndBlockLight = (byte)(value << 4      | Light & 0x0F); }
		}
		public byte Light
		{
			get { return (byte)(SkyAndBlockLight & 0x0F); }
			set { SkyAndBlockLight = (byte)(SkyLight << 4   | value & 0x0F); }
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
			return String.Format("ID: {0}, Meta: {1}, Light: {2}, SkyLight: {3}", ID, Meta, Light, SkyLight);
		}

		public static bool operator ==(Block a, Block b)
		{
			return a.IDMeta == b.IDMeta && a.SkyAndBlockLight == b.SkyAndBlockLight;
		}

		public static bool operator !=(Block a, Block b)
		{
			return a.IDMeta != b.IDMeta && a.SkyAndBlockLight != b.SkyAndBlockLight;
		}

		public bool Equals(Block other)
		{
			return other.IDMeta.Equals(IDMeta);
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Block))
				return false;

			return Equals((Block) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = IDMeta.GetHashCode();
				return result;
			}
		}

        public bool IsAir { get { return ID == 0; } }

        public bool IsTransparent { get { return ID == 18 || ID == 161; } }

        public bool IsFluid { get { return ID == 18 || ID == 161; } }

        public static readonly int[] TransparentBlocks = 
        {
            6, 8, 9, 18, 20, 27, 28, 30,  31, 32, 37, 38, 39, 40, 
        };
	}
}