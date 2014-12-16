using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using MineLib.Network.Data;
using MineLib.Network.Data.Anvil;
using ProtocolClassic.IO;

namespace ProtocolClassic.Data
{
    public static class Level
    {
        private static List<byte> ChunkData = new List<byte>();

        public static void ReadFromSteam(IEnumerable<byte> chunkData)
        {
            ChunkData.AddRange(chunkData);
        }

        public static ChunkList ReadFromArray(Position coordinates)
        {
            var width = coordinates.X;
            var height = coordinates.Y;
            var depth = coordinates.Z;

            //var blockers = new int[width * depth];
            //for (int i = 0; i < blockers.Length; i++)
            //    blockers[i] = height;
            //
            //CalcLightDepths(blockers, 0, 0, width, depth, width, height);

            var chunkCount = (width / Chunk.Width / 2) + (depth / Chunk.Depth / 2);
            var chunks = new List<Chunk>();

            using (var reader = new ClassicDataReader(Decompress(ChunkData.ToArray())))
            {
                ChunkData.Clear();

                var length = reader.ReadInt();                              // -- Block count.
                var blocksClassic = new Block[length];                      // -- Classic block format.
                for (int i = 0; i < length; i++)
                    blocksClassic[i] = new Block(reader.ReadByte());        // -- Read all blocks.

                var blocks = ClassicToAnvil(blocksClassic, width, depth, height);  // -- Anvil block format.

                // -- foreach Chunk.
                int yOffset = 0, xOffset = 0, zOffset = 0;
                for (int i = 0; i < chunkCount; i++)
                {
                    if(xOffset >= width || zOffset >= depth)
                        throw new Exception("Block count doesn't match Width and Depth info.");

                    chunks.Add(new Chunk(new Coordinates2D(0, 0))); // TODO: Implement coordinates

                    var blocksInEachSection = new Block[chunks[i].Sections.Length][, ,];

                    // -- foreach Section.
                    for (int j = 0; j < (height / Section.Height); j++)
                    {
                        blocksInEachSection[j] = new Block[Section.Width, Section.Height, Section.Depth];

                        // -- Section filling.
                        for (int secY = 0; secY < Section.Height; secY++)
                        {
                            for (int secX = 0; secX < Section.Width; secX++)
                            {
                                for (int secZ = 0; secZ < Section.Depth; secZ++)
                                {
                                    blocksInEachSection[j][secX, secY, secZ] = blocks[xOffset, yOffset, zOffset];
                                }
                            }

                            yOffset++;
                        }

                        chunks[i].Sections[j].BuildFromBlocks(blocksInEachSection[j]);
                    }

                    yOffset = 0;
                    xOffset += Section.Width;
                    zOffset += Section.Depth;
                }
            }

            return new ChunkList(chunks);
        }

        public static Block[,,] ClassicToAnvil(Block[] blocks, int width, int depth, int height)
        {
            var converted = new Block[width, height, depth];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        converted[x, y, z] = blocks[GetIndexClassic(x, y, z, width, depth)];

                    }
                }
            }

            return converted;
        }

        public static int GetIndexClassic(int x, int y, int z, int width, int depth)
        {
            return (y * depth + z) * width + x;
        }

        private static byte[] Decompress(byte[] data)
        {
            using (var stream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
            {
                const int size = 1024;
                var buffer = new byte[size];
                using (var memory = new MemoryStream())
                {
                    int count;
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