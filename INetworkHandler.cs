using System;
using System.Collections.Generic;
using System.Net.Sockets;
using MineLib.Network.Events;
using MineLib.Network.IO;

namespace MineLib.Network
{
    public interface INetworkHandler : IDisposable
    {
        event PacketHandler OnPacketHandled;

        // -- Debugging
        List<IPacket> PacketsReceived { get; set;}
        List<IPacket> PacketsSended { get; set; }
        // -- Debugging

        IPacketSender PacketSender { get; }

        NetworkMode NetworkMode { get; }

        bool DebugPackets { get; set; }

        bool Connected { get; }
        bool Crashed { get; }


        void Connect(bool debugPackets = true);

        IAsyncResult BeginSendPacket(IPacket packet, AsyncCallback asyncCallback, object state);
        IAsyncResult BeginSend(IPacket packet, AsyncCallback asyncCallback, object state);
        void EndSend(IAsyncResult asyncResult);

        void RaisePacketHandled(int id, IPacket packet, ServerState? state);
    }
}