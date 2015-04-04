using System;

namespace MineLib.Network
{
    public static class Converter
    {
        public static byte[] ConvertUShort(ushort value)
        {
            var intArray = new byte[15];

            for (var i = 0; i < 15; i++)
                intArray[i] = (byte) ((value & (1 << i)) > 0 ? 1 : 0);
            
            return intArray;
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

    }
}
