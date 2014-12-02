using System;
using System.Runtime.Serialization;

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
        //void EndConnect(IAsyncResult asyncResult);
        IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, object state);
        void EndDisconnect(IAsyncResult asyncResult);

        IAsyncResult DoAsyncSending(Type asyncSendingType, IAsyncSendingParameters parameters);

        void Connect(string ip, ushort port);
        void Disconnect();
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