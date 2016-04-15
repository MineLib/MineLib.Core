using System;

using MineLib.Core.Data.Anvil;

namespace MineLib.Core.Extensions
{
    public static class Helper
    {
        /// <summary>
        /// Converting ushort value from a Chunk to a boolean value. Which Sections are empty or not.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool[] ConvertFromUShort(ushort value)
        {
            var array = new bool[15];

            for (var i = 0; i < 15; i++)
                array[i] = (value & (1 << i)) > 0;
            
            return array;
        }
        public static ushort ConvertToUShort(this Section[] sections)
        {
            ushort primaryBitMap = 0, mask = 1;

            for (var i = sections.Length - 1; i >= 0; i--)
            {
                if (sections[i].IsFilled)
                    primaryBitMap |= mask;
                
                mask <<= 1;
            }

            return primaryBitMap |= mask;
        }
        public static bool[] ConvertFromUShort(Section[] sections)
        {
            if (sections.Length > 16)
                throw new NotSupportedException();

            var array = new bool[sections.Length];

            for (var i = 0; i < 15; i++)
                if (sections[i].IsFilled)
                    array[i] = true;

            return array;
        }

        public static bool NearlyEquals(this double value1, double value2, double unimportantDifference = 0.0001)
        {
            return Math.Abs(value1 - value2) < unimportantDifference;
        }
        public static bool NearlyEquals(this double? value1, double? value2, double unimportantDifference = 0.0001)
        {
            if (value1 != value2)
            {
                if (value1 == null || value2 == null)
                    return false;

                return Math.Abs(value1.Value - value2.Value) < unimportantDifference;
            }

            return true;
        }

        public static bool NearlyEquals(this float value1, float value2, float unimportantDifference = 0.0001f)
        {
            return Math.Abs(value1 - value2) < unimportantDifference;
        }
        public static bool NearlyEquals(this float? value1, float? value2, float unimportantDifference = 0.0001f)
        {
            if (value1 != value2)
            {
                if (value1 == null || value2 == null)
                    return false;

                return Math.Abs(value1.Value - value2.Value) < unimportantDifference;
            }

            return true;
        }
    }
}
