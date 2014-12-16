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
    }
}
