using MineLib.Network;
using MineLib.Network.IO;
using ProtocolClassic.Enum;

namespace ProtocolClassic.Packets.Extension.Server
{
    public struct SetBlockPermissionPacket : IPacketWithSize
    {
        public byte BlockType;
        public AllowPlacement AllowPlacement;
        public AllowDeletion AllowDeletion;

        public byte ID { get { return 0x1C; } }
        public short Size { get { return 4; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader stream)
        {
            BlockType = stream.ReadByte();
            AllowPlacement = (AllowPlacement) stream.ReadByte();
            AllowDeletion = (AllowDeletion) stream.ReadByte();;

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(BlockType);
            stream.WriteByte((byte) AllowPlacement);
            stream.WriteByte((byte) AllowDeletion);
            stream.Purge();

            return this;
        }
    }
}
