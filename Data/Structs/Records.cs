using System;
using System.Collections.Generic;

using Aragas.Core.Data;
using Aragas.Core.Interfaces;

namespace MineLib.Core.Data.Structs
{
    public struct Record : IEquatable<Record>
    {
        public ushort BlockIDMeta;
        public Position Coordinates;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Record) obj);
        }

        public bool Equals(Record other)
        {
            return BlockIDMeta == other.BlockIDMeta && Coordinates == other.Coordinates;
        }

        public override int GetHashCode()
        {
            return BlockIDMeta.GetHashCode() ^ Coordinates.GetHashCode();
        }
    }

    // TODO: Records bitmask
    public class RecordList
    {
        private readonly List<Record> _entries;

        public RecordList()
        {
            _entries = new List<Record>();
        }

        public VarInt Count
        {
            get { return _entries.Count; }
        }

        public Record this[int index]
        {
            get { return _entries[index]; }
            set
            {
                if (_entries.Count - 1 < index)
                    _entries.Add(value);
                else
                    _entries[index] = value;
            }
        }

        public static RecordList FromReader(IPacketDataReader reader)
        {
            var value = new RecordList();

            var count = reader.Read<VarInt>();

            for (int i = 0; i < count; i++)
            {
                var record = new Record();

                var coordinates = reader.Read<short>();
                var y = coordinates & 0xFF;
                var z = (coordinates >> 8) & 0xf; 
                var x = (coordinates >> 12) & 0xf;

                var blockIDMeta = reader.Read<VarInt>();
                record.BlockIDMeta = Convert.ToUInt16(blockIDMeta);
                record.Coordinates = new Position(x, y, z);

                value[i] = record;
            }

            return value;
        }

        public void ToStream(IPacketStream stream)
        {
            stream.Write(Count);

            foreach (var entry in _entries)
            {
            }
        }

        public IEnumerable<Record> GetRecords()
        {
            return _entries.ToArray();
        }
    }
}
