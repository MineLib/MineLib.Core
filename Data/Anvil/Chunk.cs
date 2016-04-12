using System;
using System.Linq;

using MineLib.Core.Extensions;

namespace MineLib.Core.Data.Anvil
{
    // -- Full  (12304 bytes Section) - 197138 bytes.
    // -- Empty (20    bytes Section) - 594    bytes.
    // -- Empty (empty       Section) - 26     bytes.
    public class Chunk : IEquatable<Chunk>
    {
        public const ushort Width = 16;
        public const ushort Height = 256;
        public const ushort Depth = 16;

        public const int OneByteData = Section.Width * Section.Depth * Section.Height;
        public const int HalfByteData = OneByteData / 2;
        public const int TwoByteData = OneByteData * 2;
        public const int BiomesLength = Width * Depth;

        public Coordinates2D Coordinates;
        public ushort PrimaryBitMap;
        public bool OverWorld;
        public bool GroundUp;

        public byte[] Biomes;

        public Section[] Sections;

        // -- Debugging
        public bool[] PrimaryBitMapConverted => SectionStatus(PrimaryBitMap);
        // -- Debugging
    
        public Chunk(Coordinates2D chunkCoordinates)
        {
            Coordinates = chunkCoordinates;
            Biomes = new byte[BiomesLength];

            Sections = new Section[16];
            for (var i = 0; i < Sections.Length; i++)
                Sections[i] = new Section(new Position(Coordinates.X, i, Coordinates.Z));     
        }

        public override string ToString() => $"Filled Sections: {GetFilledSectionsCount()}";

        public static int GetSectionCount(ushort bitMap)
        {
            var sectionCount = 0;

            for (var y = 0; y < 16; y++)
                if ((bitMap & (1 << y)) > 0) sectionCount++;
            
            return sectionCount;
        }

        public Block GetBlock(Position coordinates)
        {
            var destSection = GetSectionByY(coordinates.Y);
            return destSection.GetBlock(GetSectionCoordinates(coordinates));
        }
        public void SetBlock(Position worldCoordinates, Block block)
        {
            var destSection = GetSectionByY(worldCoordinates.Y);
            destSection.SetBlock(GetSectionCoordinates(worldCoordinates), block);
        }

        public void SetBlockMultiBlock(Position coordinates, Block block)
        {
            var destSection = GetSectionByY(coordinates.Y);
            destSection.SetBlock(new Position(coordinates.X, GetYinSection(coordinates.Y), coordinates.Z), block);
        }

        public byte GetBlockLight(Position worldCoordinates)
        {
            var destSection = GetSectionByY(worldCoordinates.Y);
            return destSection.GetBlockLighting(GetSectionCoordinates(worldCoordinates));
        }
        public void SetBlockLight(Position worldCoordinates, byte light)
        {
            var destSection = GetSectionByY(worldCoordinates.Y);
            destSection.SetBlockLighting(GetSectionCoordinates(worldCoordinates), light);
        }

        public byte GetBlockSkylight(Position worldCoordinates)
        {
            var destSection = GetSectionByY(worldCoordinates.Y);
            return destSection.GetBlockSkylight(GetSectionCoordinates(worldCoordinates));
        }
        public void SetBlockSkylight(Position worldCoordinates, byte light)
        {
            var destSection = GetSectionByY(worldCoordinates.Y);
            destSection.SetBlockSkylight(GetSectionCoordinates(worldCoordinates), light);
        }

        public byte GetBlockBiome(Position worldCoordinates)
        {
            var chunkCoordinates = GetChunkCoordinates(worldCoordinates);
            return Biomes[(chunkCoordinates.Z * 16) + chunkCoordinates.X];
        }
        public void SetBlockBiome(Position worldCoordinates, byte biome)
        {
            var chunkCoordinates = GetChunkCoordinates(worldCoordinates);
            Biomes[(chunkCoordinates.Z * 16) + chunkCoordinates.X] = biome;
        }

        public byte GetBlockBiome(Coordinates2D chunkCoordinates) => Biomes[(chunkCoordinates.Z * 16) + chunkCoordinates.X];
        public void SetBlockBiome(Coordinates2D chunkCoordinates, byte biome) { Biomes[(chunkCoordinates.Z * 16) + chunkCoordinates.X] = biome; }

        #region Helping Methods

        public Position GetBlockPosition(Section section, int index)
        {
            var sectionPosition = Section.GetSectionPositionByIndex(index);

            return new Position(
                Section.Width * Coordinates.X + sectionPosition.X,
                Section.Height * section.ChunkPosition.Y + sectionPosition.Y,
                Section.Depth * Coordinates.Z + sectionPosition.Z);
        }

        private Section GetSectionByY(int blockY) => Sections[blockY / Section.Height];

        public static Position GetSectionCoordinates(Position coordinates, Coordinates2D chunkCoordinates) => new Position(
                Math.Abs(coordinates.X - (chunkCoordinates.X*Section.Width)),
                coordinates.Y%Section.Height,
                coordinates.Z%chunkCoordinates.Z);

        private Position GetSectionCoordinates(Position coordinates)
        {
            var chunk = GetChunkCoordinates(coordinates);

            // -- https://github.com/Azzi777/Umbra-Voxel-Engine/blob/master/Umbra%20Voxel%20Engine/Implementations/ChunkManager.cs#L172
            if (chunk.X != Coordinates.X || chunk.Z != Coordinates.Z)
                throw new ArgumentOutOfRangeException(nameof(coordinates), "You stupid asshole!");

            return new Position(
                GetXinSection(coordinates.X),
                GetYinSection(coordinates.Y),
                GetZinSection(coordinates.Z));
        }

        private static Coordinates2D GetChunkCoordinates(Position worldCoordinates) => new Coordinates2D(
            worldCoordinates.X >> 4,
            worldCoordinates.Z >> 4);

        private int GetXinSection(int blockX) => Math.Abs(blockX - (Coordinates.X * Section.Width));
        private int GetYinSection(int blockY) => blockY % Section.Height;
        private int GetZinSection(int blockZ) => blockZ % Coordinates.Z;

        private int GetFilledSectionsCount() => Sections.Count(section => section.IsFilled);

        private static bool[] SectionStatus(ushort primaryBitMap) => Helper.ConvertFromUShort(primaryBitMap);

        private ushort SectionStatus() => this.ConvertToUShort();

        #endregion

        public static bool operator ==(Chunk a, Chunk b) => !ReferenceEquals(a, null) && !ReferenceEquals(b, null) && (a.Coordinates.Equals(b.Coordinates) && a.Sections.Equals(b.Sections) && a.Biomes.Equals(b.Biomes));
        public static bool operator !=(Chunk a, Chunk b) => ReferenceEquals(a, null) || ReferenceEquals(b, null) || (!a.Coordinates.Equals(b.Coordinates) && !a.Sections.Equals(b.Sections) && !a.Biomes.Equals(b.Biomes));

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Chunk) obj);
        }
        public bool Equals(Chunk chunk) => Coordinates.Equals(chunk.Coordinates) && Sections.Equals(chunk.Sections) && Biomes.Equals(chunk.Biomes);

        public override int GetHashCode() => Coordinates.GetHashCode() ^ Sections.GetHashCode() ^ Biomes.GetHashCode();
    }
}
