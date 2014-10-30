using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Client.Status
{
    public struct RequestPacket : IPacket
    {
        public byte ID { get { return 0x00; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.Purge();

            return this;
        }
    }
}
