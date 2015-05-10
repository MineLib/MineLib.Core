using System;
using System.Threading.Tasks;
using MineLib.Core.IO;

namespace MineLib.Core
{
    // TODO: Clean this mess.
    public interface IMinecraftClientAsync
    {
        Task ConnectAsync(String ip, UInt16 port);
        Boolean DisconnectAsync();
    }
    public interface IMinecraftClient : IMinecraftClientAsync, IDisposable
    {
        ProtocolType Mode { get; }
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
        IMinecraftClient Initialize(String login, String password, ProtocolType mode, INetworkTCP tcp, Boolean nameVerification = false, String serverSalt = null);

        void Connect(String ip, UInt16 port);
        void Disconnect();

        void RegisterReceiveEvent(Type receiveEventType, Action<IReceiveAsync> method);
        void DoReceiveEvent(Type receiveEventType, IReceiveAsync data);

        //IAsyncResult DoAsyncSending(Type asyncSendingType, IAsyncSendingParameters parameters);
    }
}