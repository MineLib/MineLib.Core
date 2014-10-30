using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct DisconnectPacket : IPacket
    {
        public string Reason;

        public byte ID { get { return 0x40; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Reason = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Reason);
            stream.Purge();

            return this;
        }
    }
}