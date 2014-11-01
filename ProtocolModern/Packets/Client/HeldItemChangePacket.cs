using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Client
{
    public struct HeldItemChangePacket : IPacket
    {
        public short Slot;

        public byte ID { get { return 0x09; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Slot = reader.ReadShort();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteShort(Slot);
            stream.Purge();

            return this;
        }
    }
}