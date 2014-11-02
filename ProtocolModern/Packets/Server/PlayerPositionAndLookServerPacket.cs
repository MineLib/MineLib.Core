using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public struct PlayerPositionAndLookPacket : IPacket
    {
        public Vector3 Vector3;
        public float Yaw, Pitch;
        public PlayerPositionAndLookFlags Flags;

        public byte ID { get { return 0x08; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Vector3 = Vector3.FromReaderDouble(reader);
            Yaw = reader.ReadFloat();
            Pitch = reader.ReadFloat();
            Flags = (PlayerPositionAndLookFlags) reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            Vector3.ToStreamDouble(stream);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteSByte((sbyte) Flags);
            stream.Purge();

            return this;
        }
    }
}