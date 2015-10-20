using System;

using Aragas.Core.Interfaces;

namespace MineLib.Core.Interfaces
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
        IPacket ReadPacket(IPacketDataReader reader);

        /// <summary>
        /// Write packet to any stream.
        /// </summary>
        IPacket WritePacket(IPacketStream stream);
    }
}
