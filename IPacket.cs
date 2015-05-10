using System;
using MineLib.Core.IO;

namespace MineLib.Core
{
    /// <summary>
    /// IPacket without known size. Google-Protobuf like handling.
    /// </summary>
    public interface IPacket
    {
        /// <summary>
        /// IPacket number.
        /// </summary>
        Byte ID { get; }

        /// <summary>
        /// Read packet from any stream.
        /// </summary>
        IPacket ReadPacket(IProtocolDataReader reader);

        /// <summary>
        /// Write packet to any stream.
        /// </summary>
        IPacket WritePacket(IProtocolStream stream);
    }
}
