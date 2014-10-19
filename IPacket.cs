﻿using MineLib.Network.IO;

namespace MineLib.Network
{
    public interface IPacket
    {
        byte ID { get; }
        void ReadPacket(PacketByteReader stream);
        void WritePacket(ref PacketStream stream);
    }
}