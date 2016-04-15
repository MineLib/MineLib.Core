using Aragas.Core.Packets;

namespace MineLib.Core
{
    public interface IModAPIContext
    {
        void SendPacket(ProtobufPacket packet);
    }
}