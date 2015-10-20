using Aragas.Core.Interfaces;

using Org.BouncyCastle.Math;

namespace MineLib.Core.Extensions
{
    public static class PacketExtensions
    {
        public static void Write(this IPacketStream stream, BigInteger value)
        {
            stream.Write(value.ToByteArray());
        }
        public static BigInteger Read(this IPacketDataReader reader, BigInteger value = default(BigInteger))
        {
            return new BigInteger(reader.Read<byte[]>(null, 4));
        }
    }
}
