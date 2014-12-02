using System;
using MineLib.Network;
using ProtocolPocketEdition.Packets;

namespace ProtocolPocketEdition
{
    public partial class Protocol
    {
        private void OnPacketHandled(int id, IPacket packet, ConnectionState? state)
        {
            // -- Debugging
            Console.WriteLine("PocketEdition ID: 0x" + String.Format("{0:X}", id));
            Console.WriteLine(" ");
            // -- Debugging

            switch ((PacketsServer)id)
            {
            
            }
        }
    }
}