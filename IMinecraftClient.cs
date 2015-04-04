using System;

namespace MineLib.Network
{
    // TODO: Clean this mess.
    public interface IMinecraftClient : IDisposable
    {
        NetworkMode Mode { get; }
        ConnectionState ConnectionState { get; }

        String ClientLogin { get; }
        String ClientUsername { get; set; }
        String ClientPassword { get; }
        Boolean UseLogin { get; }

        String ClientBrand { get; set; }
        String ServerBrand { get; }

        String ServerHost { get; }
        UInt16 ServerPort { get; }

        Boolean Connected { get; }

        // -- Modern
        String AccessToken { get; set; }
        String SelectedProfile { get; set; }
        String ClientToken { get; set; }
        // -- Modern

        /// <summary>
        /// Constructor.
        /// </summary>
        IMinecraftClient Initialize(String login, String password, NetworkMode mode, Boolean nameVerification = false, String serverSalt = null);

        IAsyncResult BeginConnect(String ip, UInt16 port, AsyncCallback asyncCallback, Object state);
        //void EndConnect(IAsyncResult asyncResult);
        IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, Object state);
        void EndDisconnect(IAsyncResult asyncResult);

        void Connect(String ip, UInt16 port);
        void Disconnect();

        void RegisterReceiveEvent(Type receiveEventType, Action<IAsyncReceive> method);
        void DoReceiveEvent(Type receiveEventType, IAsyncReceive data);

        //IAsyncResult DoAsyncSending(Type asyncSendingType, IAsyncSendingParameters parameters);
    }
}