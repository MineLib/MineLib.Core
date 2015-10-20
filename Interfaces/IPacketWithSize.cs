using System;

using Aragas.Core.Interfaces;

namespace MineLib.Core.Interfaces
{
    /// <summary>
    /// Extension of IPacket with defenitioned packet size within it.
    /// </summary>
    public interface IPacketWithSize : IPacket
    {
        new IPacketWithSize ReadPacket(IPacketDataReader reader);
        Int16 Size { get; }
    }
}
