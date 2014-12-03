using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using MineLib.Network.Data;
using MineLib.Network.Data.Anvil;
using ProtocolClassic.IO;

namespace ProtocolClassic.Data
{
    public class Level
    {
        public const int Length = 0;
        public const int Width = 0;
        public const int Height = 0;

        public static Chunk ReadFromArray(byte[] chunkData)
        {
            List<Block> blocks = new List<Block>();

            byte[] data = Decompress(chunkData);

            using (var reader = new ClassicDataReader(data))
            {
                var length = reader.ReadIntBigEndian();

                for (int i = 0; i < length; i++)
                {
                    var block = reader.ReadByte();
                    if (block == 0x00)
                        break;

                    blocks.Add(new Block(block, 0, 15, 15));
                }

                //List<Block> blocks1 = new List<Block>();
                //foreach (var block1 in blocks)
                //{
                //    if(!blocks1.Contains(block1))
                //        blocks1.Add(block1);
                //}
            }

            return new Chunk(Coordinates2D.Zero);
        }

        public int Index(int x, int y, int z)
        {
            return (z * Length + y) * Width + x;
        }

        public int Index(Position coords)
        {
            return (coords.Z * Length + coords.Y) * Width + coords.X;
        }

        static byte[] Decompress(byte[] data)
        {
            using (var stream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
            {
                const int size = 1024;
                byte[] buffer = new byte[size];
                using (var memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);

                        if (count > 0)
                            memory.Write(buffer, 0, count);
                    }
                    while (count > 0);

                    return memory.ToArray();
                }
            }
        }
    }
}