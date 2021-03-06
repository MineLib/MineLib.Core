﻿using System;

using Aragas.Core.IO;

using MineLib.Core.Data;

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

            Extend<Position>(ReadPosition, WritePosition);
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
