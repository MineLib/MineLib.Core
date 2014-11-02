﻿using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolClassic.Packets.Extension.Server
{
    public struct RemoveSelectionPacket : IPacketWithSize
    {
        public byte SelectionID;

        public byte ID { get { return 0x1B; } }
        public short Size { get { return 2; } }

        public IPacketWithSize ReadPacket(IProtocolDataReader stream)
        {
            SelectionID = stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IProtocolDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(SelectionID);
            stream.Purge();

            return this;
        }
    }
}