using System;
using System.Collections.Generic;

using Aragas.Core.Data;
using Aragas.Core.Interfaces;

namespace MineLib.Core.Data.Structs
{
    public struct Icon : IEquatable<Icon>
    {
        public byte Direction;
        public byte Type;
        public int X;
        public int Y;
        
        public static bool operator ==(Icon a, Icon b)
        {
            return a.Direction == b.Direction && a.Type == b.Type && a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Icon a, Icon b)
        {
            return a.Direction != b.Direction && a.Type != b.Type && a.X != b.X && a.Y != b.Y;
        }

        public bool Equals(Icon other)
        {
            return Direction.Equals(other.Direction) && Type.Equals(other.Type) && X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Icon) obj);
        }

        public override int GetHashCode()
        {
            return Direction.GetHashCode() ^ Type.GetHashCode() ^ X.GetHashCode() ^ Y.GetHashCode();
        }
    }

    public class IconList
    {
        private readonly List<Icon> _entries;

        public IconList()
        {
            _entries = new List<Icon>();
        }

        public VarInt Count
        {
            get { return _entries.Count; }
        }

        public Icon this[int index]
        {
            get { return _entries[index]; }
            set { _entries[index] = value; }
        }

        public static IconList FromReader(IPacketDataReader reader)
        {
            var value = new IconList();

            var count = reader.Read<VarInt>();
            for (int i = 0; i < count; i++)
            {
                var icon = new Icon();

                var comb = reader.Read<byte>();
                icon.Direction = (byte)(comb & 0xF0);
                icon.Type = (byte)(comb & 0x0F);

                icon.X = reader.Read<byte>();
                icon.Y = reader.Read<byte>();

                value[i] = icon;
            }

            return value;
        }

        public void ToStream(IPacketStream stream)
        {
            stream.Write(Count);

            foreach (var entry in _entries)
            {
                stream.Write((byte) ((entry.Direction << 4) | entry.Type));
                stream.Write((byte) entry.X);
                stream.Write((byte) entry.Y);
            }
        }
    }

}
