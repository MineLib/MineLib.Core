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
        public readonly ushort IDMeta;
        public byte SkyAndBlockLight;

        public Block(ushort id)
        {
            IDMeta = (ushort) (id << 4 & 0xFFF0 | 0 & 0x000F);
            SkyAndBlockLight = 0;
        }

        public Block(ushort id, byte meta)
        {
            IDMeta = (ushort) (id << 4 & 0xFFF0 | meta & 0x000F);
            SkyAndBlockLight = 0;
        }

        public Block(ushort id, byte meta, byte light)
        {
            IDMeta = (ushort) (id << 4 & 0xFFF0 | meta & 0x000F);
            SkyAndBlockLight = (byte) (0 << 4 & 0xF0 | light & 0x0F);
        }

        public Block(ushort id, byte meta, byte light, byte skyLight)
        {
            IDMeta = (ushort) (id << 4 & 0xFFF0 | meta & 0x000F);
            SkyAndBlockLight = (byte) (skyLight << 4 & 0xF0 | light & 0x0F);
        }

        public override string ToString()
        {
            return String.Format("ID: {0}, Meta: {1}, Light: {2}, SkyLight: {3}", GetID(), GetMeta(), GetLight(), GetSkyLight());
        }

        public ushort GetID()
        {
            return (ushort) (IDMeta >> 4);
        }

        public byte GetMeta()
        {
            return (byte) (IDMeta & 0x000F);
        }

        public byte GetSkyLight()
        {
            return (byte) (SkyAndBlockLight >> 4);
        }

        public byte GetLight()
        {
            return (byte) (SkyAndBlockLight & 0xF);
        }

        public void SetSkyLight(byte skyLight)
        {
            var light = GetLight();
            SkyAndBlockLight = (byte) (skyLight << 4 & 0xF0 | light & 0x0F);
        }

        public void SetLight(byte light)
        {
            var skyLight = GetSkyLight();
            SkyAndBlockLight = (byte) (skyLight << 4 & 0xF0 | light & 0x0F);
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

            return Equals((Block)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = IDMeta.GetHashCode();
                return result;
            }
        }
    }
}