using System;
using MineLib.Network.IO;

namespace MineLib.Network
{
    public interface IPacket
    {
        Byte ID { get; }
        IPacket ReadPacket(IProtocolDataReader reader);
        IPacket WritePacket(IProtocolStream stream);
    }
}
