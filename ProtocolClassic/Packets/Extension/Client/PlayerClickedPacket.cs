using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;
using ProtocolClassic.Enums;

namespace ProtocolClassic.Packets.Extension.Client
{
    public struct PlayerClickedPacket : IPacketWithSize
    {
        public Button Button;
        public Action Action;
        public short Yaw;
        public short Pitch;
        public byte TargetEntityID;
        public Position TargetBlockLocation;
        public TargetBlockFace TargetBlockFace;

        public byte ID { get { return 0x22; } }
        public short Size { get { return 12; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader reader)
        {
            Button = (Button) reader.ReadByte();
            Action = (Action) reader.ReadByte();
            Yaw = reader.ReadShort();
            Pitch = reader.ReadShort();
            TargetEntityID = reader.ReadByte();
            TargetBlockLocation = Position.FromReaderShort(reader);
            TargetBlockFace = (TargetBlockFace) reader.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte((byte) Button);
            stream.WriteByte((byte) Action);
            stream.WriteShort(Yaw);
            stream.WriteShort(Pitch);
            stream.WriteByte(TargetEntityID);
            TargetBlockLocation.ToStreamShort(stream);
            stream.WriteByte((byte) TargetBlockFace);
            stream.Purge();

            return this;
        }
    }
}
