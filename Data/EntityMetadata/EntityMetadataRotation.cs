using Aragas.Core.Interfaces;

namespace MineLib.Core.Data.EntityMetadata
{
    /// <summary>
    /// Rotation Metadata
    /// </summary>
    public class EntityMetadataRotation : EntityMetadataEntry
    {
        public override byte Identifier { get { return 7; } }
        public override string FriendlyName { get { return "rotation"; } }

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

        public override void FromReader(IPacketDataReader reader)
        {
            Rotation = Rotation.FromReaderFloat(reader);
        }

        public override void ToStream(IPacketStream stream, byte index)
        {
            stream.Write(GetKey(index));
            Rotation.ToStreamFloat(stream);
        }
    }
}
