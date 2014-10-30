using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Client
{
    public struct EnchantItemPacket : IPacket
    {
        public byte WindowId;
        public byte Enchantment;

        public byte ID { get { return 0x11; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            WindowId = reader.ReadByte();
            Enchantment = reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(WindowId);
            stream.WriteVarInt(Enchantment);
            stream.Purge();

            return this;
        }
    }
}