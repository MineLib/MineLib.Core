using System.Collections.Generic;
using MineLib.Network.IO;

namespace MineLib.Network.Data.Structs
{
    public struct StatisticsEntry
    {
        public string StatisticsName;
        public int Value;
    }

    public class StatisticsEntryList
    {
        private readonly List<StatisticsEntry> _entries;

        public StatisticsEntryList()
        {
            _entries = new List<StatisticsEntry>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public StatisticsEntry this[int index]
        {
            get { return _entries[index]; }
            set { _entries[index] = value; }
        }

        public static StatisticsEntryList FromReader(IProtocolDataReader reader)
        {
            var count = reader.ReadVarInt();

            var value = new StatisticsEntryList();
            for (int i = 0; i < count; i++)
                value[i] = new StatisticsEntry { StatisticsName = reader.ReadString(), Value = reader.ReadVarInt() };
            

            return value;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteVarInt(Count);

            foreach (var entry in _entries)
            {
                stream.WriteString(entry.StatisticsName);
                stream.WriteVarInt(entry.Value);
            }
        }
    }
}
