using System;
using MineLib.Network.Events;
using MineLib.Network.IO;

namespace MineLib.Network
{
    /// <summary>
    /// Any Minecraft protocol can be implemented by using this. Really, any.
    /// </summary>
    public interface IProtocol : IPluginBase, IDisposable
    {
        // -- Packet receiving.
        event PacketHandler PacketHandled;

        IPacketSender PacketSender { get; set; }

        bool Connected { get; }

        bool Crashed { get; }

        void Connect(IMinecraftClient client);

        //void Login(string login, string password);

        // -- Packet sending.
        IAsyncResult BeginSendPacket(IPacket packet, AsyncCallback asyncCallback, object state);
        IAsyncResult BeginSend(IPacket packet, AsyncCallback asyncCallback, object state);
        void EndSend(IAsyncResult asyncResult);
    }
}
