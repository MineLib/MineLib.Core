using System;
using System.Collections.Generic;

using Aragas.Core.Data;
using Aragas.Core.Interfaces;

namespace MineLib.Core.Data.Structs
{
    public struct StatisticsEntry : IEquatable<StatisticsEntry>
    {
        public string StatisticsName;
        public VarInt Value;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            
            if (obj.GetType() != GetType())
                return false;

            return Equals((StatisticsEntry) obj);
        }

        public bool Equals(StatisticsEntry other)
        {
            return StatisticsName == other.StatisticsName && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return StatisticsName.GetHashCode() ^ Value.GetHashCode();
        }
    }

    public class StatisticsEntryList
    {
        private readonly List<StatisticsEntry> _entries;

        public StatisticsEntryList()
        {
            _entries = new List<StatisticsEntry>();
        }

        public VarInt Count
        {
            get { return _entries.Count; }
        }

        public StatisticsEntry this[int index]
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

        public static StatisticsEntryList FromReader(IPacketDataReader reader)
        {
            var count = reader.Read<VarInt>();

            var value = new StatisticsEntryList();
            for (int i = 0; i < count; i++)
                value[i] = new StatisticsEntry {StatisticsName = reader.Read<string>(), Value = reader.Read<VarInt>()};
            
            return value;
        }

        public void ToStream(IPacketStream stream)
        {
            stream.Write(Count);

            foreach (var entry in _entries)
            {
                stream.Write(entry.StatisticsName);
                stream.Write(entry.Value);
            }
        }
    }
}
