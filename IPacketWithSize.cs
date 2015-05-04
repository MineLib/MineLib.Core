using System;

using MineLib.Network.IO;

namespace MineLib.Network
{
    /// <summary>
    /// Extension of IPacket with defenitioned packet size within it.
    /// </summary>
    public interface IPacketWithSize : IPacket
    {
        new IPacketWithSize ReadPacket(IProtocolDataReader reader);
        Int16 Size { get; }
    }
}
