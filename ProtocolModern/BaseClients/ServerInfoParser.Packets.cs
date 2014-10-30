using MineLib.Network;
using ProtocolModern.Enum;

namespace ProtocolModern.BaseClients
{
    public sealed partial class ServerInfoParser
    {
        private void RaisePacketHandled(int id, IPacket packet, ServerState? state)
        {
            if (state != ServerState.ModernStatus) 
                return;

            switch ((PacketsServer) id)
            {
                case PacketsServer.Ping:
                    if (FirePingPacket != null)
                        FirePingPacket(packet);
                    break;

                case PacketsServer.Response:
                    if (FireResponsePacket != null)
                        FireResponsePacket(packet);
                    break;
            }
        }
    }
}