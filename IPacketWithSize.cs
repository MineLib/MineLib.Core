using System;
using MineLib.Core.IO;

namespace MineLib.Core
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
