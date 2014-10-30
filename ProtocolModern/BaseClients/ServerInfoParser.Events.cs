using MineLib.Network.Events;

namespace ProtocolModern.BaseClients
{
    public sealed partial class ServerInfoParser
    {
        public event PacketsHandler FirePingPacket;
        public event PacketsHandler FireResponsePacket;
    }
}