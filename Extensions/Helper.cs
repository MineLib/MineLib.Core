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
            var intArray = new bool[15];

            for (var i = 0; i < 15; i++)
                intArray[i] = ((value & (1 << i)) > 0);
            
            return intArray;
        }

        public static ushort ConvertToUShort(this Chunk chunk)
        {
            foreach (var section in chunk.Sections)
            {
                if (section.IsFilled)
                    ;
            }
            return 0;
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
