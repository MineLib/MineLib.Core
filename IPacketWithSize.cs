using System;
using MineLib.Network.IO;

namespace MineLib.Network
{
    public interface IPacketWithSize : IPacket
    {
        new IPacketWithSize ReadPacket(IProtocolDataReader reader);
        Int16 Size { get; }
    }
}
