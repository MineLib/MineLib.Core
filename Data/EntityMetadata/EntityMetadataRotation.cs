using Aragas.Core.IO;

namespace MineLib.Core.Data.EntityMetadata
{
    /// <summary>
    /// Rotation Metadata
    /// </summary>
    public class EntityMetadataRotation : EntityMetadataEntry
    {
        protected override byte Identifier => 7;
        protected override string FriendlyName => "rotation";

        public Rotation Rotation;

        public EntityMetadataRotation()
        {
            Rotation = new Rotation(0,0,0);
        }

        public EntityMetadataRotation(float pitch, float yaw, float roll)
        {
            Rotation = new Rotation(pitch, yaw, roll);
        }

        public EntityMetadataRotation(Rotation rotation)
        {
            Rotation = rotation;
        }

        public override void FromReader(PacketDataReader reader)
        {
            Rotation = new Rotation(
                reader.Read<float>(),
                reader.Read<float>(),
                reader.Read<float>());
        }

        public override void ToStream(PacketStream stream, byte index)
        {
            stream.Write(GetKey(index));

            stream.Write(Rotation.Pitch);
            stream.Write(Rotation.Yaw);
            stream.Write(Rotation.Roll);
        }
    }
}
