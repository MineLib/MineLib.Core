using MineLib.Network.IO;

namespace MineLib.Network
{
    public interface IPacket
    {
        byte ID { get; }
        IPacket ReadPacket(IProtocolDataReader reader);
        IPacket WritePacket(IProtocolStream stream);
    }
}
