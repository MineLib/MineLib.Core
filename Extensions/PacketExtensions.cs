using System;

using Aragas.Core.IO;

using MineLib.Core.Data;

using Org.BouncyCastle.Math;

using static Aragas.Core.IO.PacketStream;
using static Aragas.Core.IO.PacketDataReader;

namespace MineLib.Core.Extensions
{
    public static class PacketExtensions
    {
        private static void Extend<T>(Func<PacketDataReader, int, T> readFunc, Action<PacketStream, T, bool> writeAction)
        {
            ExtendRead(readFunc);
            ExtendWrite(writeAction);
        }

        public static void Init()
        {
            Aragas.Core.Extensions.PacketExtensions.Init();

            Extend<NotSupportedType>(ReadNotSupportedType, WriteNotSupportedType);

            Extend<BigInteger>(ReadBigInteger, WriteBigInteger);
            Extend<Position>(ReadPosition, WritePosition);
        }

        public static void WriteNotSupportedType(PacketStream stream, NotSupportedType value, bool writeDefaultLength = true) { }
        private static NotSupportedType ReadNotSupportedType(PacketDataReader reader, int length = 0) { return null; }

        private static void WriteBigInteger(PacketStream stream, BigInteger value, bool writeDefaultLength = true)
        {
            stream.Write(value.ToByteArray());
        }
        private static BigInteger ReadBigInteger(PacketDataReader reader, int length = 0)
        {
            return new BigInteger(reader.Read<byte[]>(null, 4));
        }

        private static void WritePosition(PacketStream stream, Position value, bool writeDefaultLength = true)
        {
            stream.Write(value.ToLong());
        }
        private static Position ReadPosition(PacketDataReader reader, int length = 0)
        {
            return MineLib.Core.Data.Position.FromLong(reader.Read<long>());
        }
    }
}
