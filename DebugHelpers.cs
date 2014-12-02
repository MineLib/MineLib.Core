namespace MineLib.Network
{
    public static class Converter
    {
        public static int[] ConvertUShort(ushort value)
        {
            var intArray = new int[15];
            for (var i = 0; i < 15; i++)
            {
                if ((value & (1 << i)) > 0)
                    intArray[i] = 1;
                else
                    intArray[i] = 0;
            }
            return intArray;
        }
    }
}
