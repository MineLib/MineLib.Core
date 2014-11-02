using MineLib.Network;
using MineLib.Network.IO;
using ProtocolClassic.Enum;

namespace ProtocolClassic.Packets.Extension.Server
{
    public struct HackControlPacket : IPacketWithSize
    {
        public Flying Flying;
        public NoClip NoClip;
        public Speeding Speeding;
        public SpawnControl SpawnControl;
        public ThirdPersonView ThirdPersonView;
        public short JumpHeight;

        public byte ID { get { return 0x20; } }
        public short Size { get { return 8; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            Flying = (Flying) stream.ReadByte();
            NoClip = (NoClip) stream.ReadByte();
            Speeding = (Speeding) stream.ReadByte();
            SpawnControl = (SpawnControl) stream.ReadByte();
            ThirdPersonView = (ThirdPersonView) stream.ReadByte();
            JumpHeight = stream.ReadShort();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte((byte) Flying);
            stream.WriteByte((byte) NoClip);
            stream.WriteByte((byte) Speeding);
            stream.WriteByte((byte) SpawnControl);
            stream.WriteByte((byte) ThirdPersonView);
            stream.WriteShort(JumpHeight);
            stream.Purge();

            return this;
        }
    }
}
