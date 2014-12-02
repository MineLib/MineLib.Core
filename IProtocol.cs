using System;
using System.Collections.Generic;
using System.Threading;
using MineLib.Network.Module;

namespace MineLib.Network
{
    /// <summary>
    /// Any Minecraft protocol can be implemented by using this. Really, any.
    /// </summary>
    public interface IProtocol : IModule, IProtocolDebug, IProtocolAsyncConnection, IDisposable
    {
        ConnectionState State { get; set; }
        bool Connected { get; }

        IProtocol Create(IMinecraftClient client, bool debugPackets = true);

        void SendPacket(IPacket packet);

        void Connect(string ip, ushort port);
        void Disconnect();

        bool UseLogin { get; }
        bool Login(string login, string password);
        bool Logout();
    }

    public interface IProtocolDebug
    {
        List<IPacket> PacketsReceived { get; }
        List<IPacket> PacketsSended { get; }
        List<IPacket> LastPackets { get; }
        IPacket LastPacket { get; }
        bool SavePackets { get; }
    }

    public interface IProtocolAsyncConnection
    {
        IAsyncResult BeginSendPacketHandled(IPacket packet, AsyncCallback asyncCallback, object state);
        IAsyncResult BeginSendPacket(IPacket packet, AsyncCallback asyncCallback, object state);
        void EndSendPacket(IAsyncResult asyncResult);

        IAsyncResult BeginConnect(string ip, ushort port, AsyncCallback asyncCallback, object state);
        //void EndConnect(IAsyncResult asyncResult);

        IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, object state);
        void EndDisconnect(IAsyncResult asyncResult);

        void RegisterAsyncSending(Type asyncSendingType, Func<IAsyncSendingParameters, IAsyncResult> method);
        IAsyncResult DoAsyncSending(Type asyncSendingType, IAsyncSendingParameters parameters);
    }

    public class ProtocolAsyncResult : IAsyncResult
    {
        public bool IsCompleted { get; private set; }
        public WaitHandle AsyncWaitHandle { get; private set; }
        public object AsyncState { get; private set; }
        public bool CompletedSynchronously { get; private set; }
    }

    public class ProtocolAsyncReceiverEventArgs : EventArgs
    {
        public string Text { get; set; }

        public ProtocolAsyncReceiverEventArgs(string text)
        {
            Text = text;
        }
    }

    public class ProtocolException : Exception
    {
        public ProtocolException()
            : base() { }

        public ProtocolException(string message)
            : base(message) { }

        public ProtocolException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public ProtocolException(string message, Exception innerException)
            : base(message, innerException) { }

        public ProtocolException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}
