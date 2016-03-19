using System;

using Aragas.Core.Data;
using Aragas.Core.IO;

using fNbt;

using MineLib.Core.Data;
using MineLib.Core.Data.Structs;

using Org.BouncyCastle.Math;

using static Aragas.Core.IO.PacketStream;
using static Aragas.Core.IO.PacketDataReader;

namespace MineLib.Core.Extensions
{
    public static class PacketExtensions
    {
        private static void Extend<T>(Func<PacketDataReader, int, T> readFunc, Action<PacketStream, T> writeAction)
        {
            ExtendRead(readFunc);
            ExtendWrite(writeAction);
        }

        public static void Init()
        {
            Aragas.Core.Extensions.PacketExtensions.Init();

            Extend<BigInteger>(ReadBigInteger, WriteBigInteger);
            Extend<Position>(ReadPosition, WritePosition);
            Extend<EntityMetadataList>(ReadEntityMetadata, WriteEntityMetadataList);
            Extend<ItemStack>(ReadItemStack, WriteItemStack);

            Extend<ItemStack[]>(ReadItemStackArray, WriteItemStackArray);
            Extend<EntityProperty[]>(ReadEntityPropertyArray, WriteEntityPropertyArray);
            Extend<Record[]>(ReadRecordArray, WriteRecordArray);
            Extend<ChunkColumnMetadata[]>(ReadChunkColumnMetadataArray, WriteChunkColumnMetadataArray);
        }

        private static void WriteBigInteger(PacketStream stream, BigInteger value)
        {
            stream.Write(value.ToByteArray());
        }
        private static BigInteger ReadBigInteger(PacketDataReader reader, int length = 0)
        {
            return new BigInteger(reader.Read<byte[]>(null, 4));
        }

        private static void WritePosition(PacketStream stream, Position value)
        {
            stream.Write(value.ToLong());
        }
        private static Position ReadPosition(PacketDataReader reader, int length = 0)
        {
            return MineLib.Core.Data.Position.FromLong(reader.Read<long>());
        }

        private static void WriteEntityMetadataList(PacketStream stream, EntityMetadataList value)
        {
            foreach (var entry in value._entries)
                entry.Value.ToStream(stream, entry.Key);

            stream.Write((byte)0x7F);
        }
        private static EntityMetadataList ReadEntityMetadata(PacketDataReader reader, int length = 0)
        {
            if (length == 0)
                length++;

            var array = new EntityMetadataList(length);

            while (true)
            {
                byte key = reader.Read<byte>();
                if (key == 127) break;

                var type = (byte)((key & 0xE0) >> 5);
                var index = (byte)(key & 0x1F);

                var entry = EntityMetadataList.EntryTypes[type]();
                entry.FromReader(reader);
                entry.Index = index;

                array[index] = entry;
            }
            return array;
        }

        private static void WriteItemStack(PacketStream stream, ItemStack value)
        {
            stream.Write(value.ID);
            if (value.Empty)
                return;

            stream.Write((byte)value.Count);
            stream.Write(value.Damage);
            if (value.Nbt == null)
            {
                stream.Write((short)-1);
                return;
            }

            var file = new NbtFile(value.Nbt);
            file.SaveToProtocolStream(stream, NbtCompression.GZip);
        }
        private static ItemStack ReadItemStack(PacketDataReader reader, int length = 0)
        {
            var itemStack = new ItemStack(reader.Read<short>());

            if (itemStack.Empty)
                return itemStack;

            itemStack.Count = reader.Read<byte>();
            itemStack.Damage = reader.Read<short>();

            var buffLength = reader.Read<VarInt>();
            if (buffLength == -1 || buffLength == 0)
                return itemStack;

            itemStack.Nbt = new NbtCompound();
            var buffer = reader.Read<byte[]>(null, buffLength);
            var nbt = new NbtFile();
            nbt.LoadFromBuffer(buffer, 0, buffLength, NbtCompression.GZip, null);
            itemStack.Nbt = nbt.RootTag;

            return itemStack;
        }

        private static void WriteItemStackArray(PacketStream stream, ItemStack[] value)
        {
            foreach (var itemStack in value)
            {
                //if (itemStack.ID == 1) // AIR
                //    return;

                stream.Write(itemStack.ID);
                stream.Write(itemStack.Damage);
                stream.Write((short)itemStack.Count);

                //if (itemStack.Empty)
                //    stream.WriteSByte(itemStack.Slot);

                if (itemStack.Nbt == null)
                {
                    stream.Write((short)-1);
                    return;
                }
            }
        }
        private static ItemStack[] ReadItemStackArray(PacketDataReader reader, int length = 0)
        {
            if(length == 0)
                length = reader.Read<short>();

            var array = new ItemStack[length];

            for (int i = 0; i < length; i++)
                array[i] = reader.Read<ItemStack>();

            return array;
        }

        private static void WriteEntityPropertyArray(PacketStream stream, EntityProperty[] value)
        {
            foreach (var entry in value)
            {
                stream.Write(entry.Key);
                stream.Write((double)entry.Value);

                stream.Write((short)entry.Modifiers.Length);
                foreach (var modifiers in entry.Modifiers)
                {
                    stream.Write(modifiers.UUID);
                    stream.Write((double)modifiers.Amount);
                    stream.Write(modifiers.Operation);
                }
            }
        }
        private static EntityProperty[] ReadEntityPropertyArray(PacketDataReader reader, int length = 0)
        {
            if (length == 0)
                length = reader.Read<int>();

            var array = new EntityProperty[length];
            for (int i = 0; i < length; i++)
            {
                var property = new EntityProperty();

                property.Key = reader.Read<string>();
                property.Value = (float)reader.Read<double>();
                var listLength = reader.Read<VarInt>();

                property.Modifiers = new Modifiers[listLength];
                for (var j = 0; j < listLength; j++)
                {
                    var item = new Modifiers
                    {
                        UUID = reader.Read<BigInteger>(),
                        Amount = (float)reader.Read<double>(),
                        Operation = reader.Read<sbyte>()
                    };

                    property.Modifiers[j] = item;
                }

                array[i] = property;
            }

            return array;
        }

        private static void WriteRecordArray(PacketStream stream, Record[] value)
        {
            foreach (var entry in value)
            {
            }
        }
        private static Record[] ReadRecordArray(PacketDataReader reader, int length = 0)
        {
            if (length == 0)
                length = reader.Read<int>();

            var array = new Record[length];
            for (int i = 0; i < length; i++)
            {
                var record = new Record();

                var coordinates = reader.Read<short>();
                var y = coordinates & 0xFF;
                var z = (coordinates >> 8) & 0xf;
                var x = (coordinates >> 12) & 0xf;

                var blockIDMeta = reader.Read<VarInt>();
                record.BlockIDMeta = Convert.ToUInt16(blockIDMeta);
                record.Coordinates = new Position(x, y, z);

                array[i] = record;
            }

            return array;
        }

        private static void WriteChunkColumnMetadataArray(PacketStream stream, ChunkColumnMetadata[] value)
        {
            foreach (var entry in value)
            {
                stream.Write(entry.Coordinates.X);
                stream.Write(entry.Coordinates.Z);
                stream.Write(entry.PrimaryBitMap);
            }
        }
        private static ChunkColumnMetadata[] ReadChunkColumnMetadataArray(PacketDataReader reader, int length = 0)
        {
            if (length == 0)
                length = reader.Read<VarInt>();

            var array = new ChunkColumnMetadata[length];
            for (int i = 0; i < length; i++)
                array[i] = new ChunkColumnMetadata
                {
                    Coordinates = new Coordinates2D(reader.Read<int>(), reader.Read<int>()),
                    PrimaryBitMap = reader.Read<ushort>()
                };

            return array;
        }
    }
}
