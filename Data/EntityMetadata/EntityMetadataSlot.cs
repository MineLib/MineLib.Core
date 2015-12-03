using Aragas.Core.Interfaces;

using MineLib.Core.Extensions;

namespace MineLib.Core.Data.EntityMetadata
{
    /// <summary>
    /// Slot Metadata
    /// </summary>
    public class EntityMetadataSlot : EntityMetadataEntry
    {
        protected override byte Identifier => 5;
        protected override string FriendlyName => "slot";

        public ItemStack Value;

        public static implicit operator EntityMetadataSlot(ItemStack value)
        {
            return new EntityMetadataSlot(value);
        }

        public EntityMetadataSlot()
        {
        }

        public EntityMetadataSlot(ItemStack value)
        {
            Value = value;
        }

        public override void FromReader(PacketDataReader reader)
        {
            Value = reader.Read<ItemStack>();
        }

        public override void ToStream(IPacketStream stream, byte index)
        {
            stream.Write(GetKey(index));
            stream.Write(Value);
        }
    }
}