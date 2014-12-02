using System;

namespace MineLib.Network
{
    // TODO: Clean this shit.
    public interface IMinecraftClient : IDisposable
    {
        NetworkMode Mode { get; }
        ConnectionState ConnectionState { get; }

        string ClientLogin { get; }
        string ClientUsername { get; set; }
        string ClientPassword { get; }
        bool UseLogin { get; }

        string ClientBrand { get; set; }
        string ServerBrand { get; }

        string ServerHost { get; }
        ushort ServerPort { get; }

        bool Connected { get; }

        // -- Modern
        string AccessToken { get; set; }
        string SelectedProfile { get; set; }
        string ClientToken { get; set; }
        // -- Modern

        IMinecraftClient Create(string login, string password, NetworkMode mode, bool nameVerification = false, string serverSalt = null);

        IAsyncResult BeginConnect(string ip, ushort port, AsyncCallback asyncCallback, object state);
        //void EndConnect(IAsyncResult asyncResult);
        IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, object state);
        void EndDisconnect(IAsyncResult asyncResult);

        void Connect(string ip, ushort port);
        void Disconnect();

        void RegisterReceiveEvent(Type receiveEventType, Action<IAsyncReceive> method);
        void DoReceiveEvent(Type receiveEventType, IAsyncReceive data);

        //IAsyncResult DoAsyncSending(Type asyncSendingType, IAsyncSendingParameters parameters);
    }
}