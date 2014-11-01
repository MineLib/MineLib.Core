using System;
using MineLib.Network.Data;

namespace MineLib.Network
{
    public interface IMinecraftClient : IDisposable
    {
        NetworkMode Mode { get; }
        ConnectionState ConnectionState { get; }

        string ClientLogin { get; }
        string ClientUsername { get; }
        string ClientPassword { set; }

        string ClientBrand { get; set; }
        string ServerBrand { get; }

        string ServerHost { get; }
        short ServerPort { get; }

        bool Connected { get; }

        // -- Modern
        string AccessToken { get; set; }
        string SelectedProfile { get; set; }
        // -- Modern

        IMinecraftClient Create(string login, string password, NetworkMode mode, bool nameVerification = false, string serverSalt = null);

        IAsyncResult BeginConnect(string ip, short port, AsyncCallback asyncCallback, object state);
        void EndConnect(IAsyncResult asyncResult);
        IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, object state);
        void EndDisconnect(IAsyncResult asyncResult);

        void Connect(string ip, short port);
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
}