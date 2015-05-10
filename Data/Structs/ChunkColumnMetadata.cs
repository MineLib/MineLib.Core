using System.Collections.Generic;

using MineLib.Core.IO;

namespace MineLib.Core.Data.Structs
{
    public struct ChunkColumnMetadata
    {
        public Coordinates2D Coordinates;
        public ushort PrimaryBitMap;

        // -- Debugging
        public bool[] PrimaryBitMapConverted { get { return Converter.ConvertFromUShort(PrimaryBitMap); } }
        // -- Debugging
    }

    public class ChunkColumnMetadataList
    {
        private readonly List<ChunkColumnMetadata> _entries;

        public ChunkColumnMetadataList()
        {
            _entries = new List<ChunkColumnMetadata>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public ChunkColumnMetadata this[int index]
        {
            get { return _entries[index]; }
            set
            {
                if (_entries.Count - 1 < index)
                    _entries.Add(value);
                else
                    _entries[index] = value;
            }
        }

        public IEnumerable<ChunkColumnMetadata> GetMetadata()
        {
            return _entries.ToArray();
        }

        public static ChunkColumnMetadataList FromReader(IProtocolDataReader reader)
        {
            var value = new ChunkColumnMetadataList();

            var count = reader.ReadVarInt();
            for (int i = 0; i < count; i++)
                value[i] = new ChunkColumnMetadata
                {
                    Coordinates = new Coordinates2D(reader.ReadInt(), reader.ReadInt()),
                    PrimaryBitMap = reader.ReadUShort()
                };
            
            return value;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteVarInt(Count);

            foreach (var entry in _entries)
            {
                stream.WriteInt(entry.Coordinates.X);
                stream.WriteInt(entry.Coordinates.Z);
                stream.WriteUShort(entry.PrimaryBitMap);
            }
        }
    }
}
