using MineLib.Network;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client
{
    public struct ResourcePackStatusPacket : IPacket
    {
        public string Hash;
        public ResourcePackStatus Result;

        public byte ID { get { return 0x19; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Hash = reader.ReadString();
            Result = (ResourcePackStatus) reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Hash);
            stream.WriteVarInt((int) Result);
            stream.Purge();

            return this;
        }
    }
}
