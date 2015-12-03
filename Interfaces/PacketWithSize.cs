using System;

using Aragas.Core.Packets;

namespace MineLib.Core.Interfaces
{
    /// <summary>
    /// Extension of IPacket with defenitioned packet size within it.
    /// </summary>
    public abstract class ProtobufPacketWithSize : ProtobufPacket
    {
        Int16 Size { get; }
    }
}
