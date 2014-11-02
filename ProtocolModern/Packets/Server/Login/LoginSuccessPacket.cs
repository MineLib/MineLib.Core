using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server.Login
{
    public struct LoginSuccessPacket : IPacket
    {
        public string UUID;
        public string Username;

        public byte ID { get { return 0x02; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            UUID = reader.ReadString();
            Username = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(UUID);
            stream.WriteString(Username);
            stream.Purge();

            return this;
        }
    }
}