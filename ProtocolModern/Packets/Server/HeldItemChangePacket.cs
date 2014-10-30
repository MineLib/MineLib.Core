using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct HeldItemChangePacket : IPacket
    {
        public sbyte Slot;

        public byte ID { get { return 0x09; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Slot = reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteSByte(Slot);
            stream.Purge();

            return this;
        }
    }
}