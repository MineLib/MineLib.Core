using System;
using System.Runtime.Serialization;
using MineLib.Network.Data.Structs;

namespace MineLib.Network
{
    public interface INetworkHandler : IDisposable
    {
        #region Properties

        NetworkMode NetworkMode { get; }
        ConnectionState ConnectionState { get; }

        bool UseLogin { get; }
        
        bool SavePackets { get; }

        bool Connected { get; }

        #endregion

        INetworkHandler Create(IMinecraftClient client, bool debugPackets = true);

        IAsyncResult BeginConnect(string ip, ushort port, AsyncCallback asyncCallback, object state);
        void EndConnect(IAsyncResult asyncResult);
        IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, object state);
        void EndDisconnect(IAsyncResult asyncResult);

        void Connect();
        void Disconnect();

        IAsyncResult BeginConnectToServer(AsyncCallback asyncCallback, object state);
        IAsyncResult BeginKeepAlive(int value, AsyncCallback asyncCallback, object state);
        IAsyncResult BeginSendClientInfo(AsyncCallback asyncCallback, object state);
        IAsyncResult BeginRespawn(AsyncCallback asyncCallback, object state);
        IAsyncResult BeginPlayerMoved(PlaverMovedData data, AsyncCallback asyncCallback, object state);
        IAsyncResult BeginPlayerSetRemoveBlock(PlayerSetRemoveBlockData data, AsyncCallback asyncCallback, object state);
        IAsyncResult BeginSendMessage(string message, AsyncCallback asyncCallback, object state);
        IAsyncResult BeginPlayerHeldItem(short slot, AsyncCallback asyncCallback, object state);
    }

    [Serializable]
    public class NetworkHandlerException : Exception
    {
        public NetworkHandlerException()
            : base() { }

        public NetworkHandlerException(string message)
            : base(message) { }

        public NetworkHandlerException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public NetworkHandlerException(string message, Exception innerException)
            : base(message, innerException) { }

        public NetworkHandlerException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected NetworkHandlerException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }
    }
}