using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MineLib.Network
{
    public class ProtocolModule
    {
        public string FilePath { get; set; }

        public string FileName
        {
            get
            {
                List<string> splits = new List<string>(FilePath.Split('\\'));

                return splits[splits.Count - 1];
            }
        }

        public ProtocolModule(string path)
        {
            FilePath = path;
        }

        public override string ToString()
        {
            return FileName;
        }
    }

    public interface INetworkHandler : IDisposable 
    {
        #region Properties

        NetworkMode NetworkMode { get; }
        ConnectionState ConnectionState { get; }

        Boolean UseLogin { get; }

        Boolean SavePackets { get; }

        Boolean Connected { get; }

        #endregion

        List<ProtocolModule> GetModules();

        /// <summary>
        /// Constructor.
        /// </summary>
        INetworkHandler Initialize(ProtocolModule module, IMinecraftClient client, Boolean debugPackets = false);

        IAsyncResult BeginConnect(String ip, UInt16 port, AsyncCallback asyncCallback, Object state);
        //void EndConnect(IAsyncResult asyncResult);
        IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, Object state);
        void EndDisconnect(IAsyncResult asyncResult);

        IAsyncResult DoAsyncSending(Type asyncSendingType, IAsyncSendingParameters parameters);

        void Connect(String ip, UInt16 port);
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