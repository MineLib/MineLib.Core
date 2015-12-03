using System;

using Aragas.Core.Data;

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
}
