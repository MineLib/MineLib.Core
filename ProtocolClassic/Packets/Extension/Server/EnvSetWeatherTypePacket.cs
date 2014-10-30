using MineLib.Network;
using MineLib.Network.IO;
using ProtocolClassic.Enum;

namespace ProtocolClassic.Packets.Extension.Server
{
    public struct EnvSetWeatherTypePacket : IPacketWithSize
    {
        public WeatherType WeatherType;

        public byte ID { get { return 0x1F; } }
        public short Size { get { return 2; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader stream)
        {
            WeatherType = (WeatherType) stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte((byte) WeatherType);
            stream.Purge();

            return this;
        }
    }
}
