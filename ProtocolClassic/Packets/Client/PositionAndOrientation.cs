using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Client
{
    public struct PositionAndOrientationPacket : IPacketWithSize
    {
        public byte PlayerID;
        public Vector3 Position;
        public byte Yaw;
        public byte Pitch;

        public byte ID { get { return 0x08; } }
        public short Size { get { return 10; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader stream)
        {
            PlayerID = stream.ReadByte();
            Position.X = stream.ReadShort();
            Position.Y = stream.ReadShort();
            Position.Z = stream.ReadShort();
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(PlayerID);
            stream.WriteShort((short)Position.X);
            stream.WriteShort((short)Position.Y);
            stream.WriteShort((short)Position.Z);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.Purge();

            return this;
        }
    }
}
