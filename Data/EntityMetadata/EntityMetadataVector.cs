using Aragas.Core.Interfaces;

namespace MineLib.Core.Data.EntityMetadata
{
    /// <summary>
    /// Vector(Position) Metadata
    /// </summary>
    public class EntityMetadataVector : EntityMetadataEntry
    {
        protected override byte Identifier => 6;
        protected override string FriendlyName => "vector";

        public Position Coordinates;

        public EntityMetadataVector()
        {
        }

        public EntityMetadataVector(int x, int y, int z)
        {
            Coordinates = new Position(x, y, z);
        }

        public EntityMetadataVector(Position position)
        {
            Coordinates = position;
        }

        public override void FromReader(PacketDataReader reader)
        {
            Coordinates = new Position(
                reader.Read<int>(),
                reader.Read<int>(),
                reader.Read<int>());
        }

        public override void ToStream(IPacketStream stream, byte index)
        {
            stream.Write(GetKey(index));
            stream.Write(Coordinates.X);
            stream.Write(Coordinates.Y);
            stream.Write(Coordinates.Z);
        }
    }
}
