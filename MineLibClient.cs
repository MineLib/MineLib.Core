using System;

using MineLib.Core.Data.Anvil;
using MineLib.Core.Events;
using MineLib.Core.Interfaces;

namespace MineLib.Core
{
    // TODO: Clean this mess.
    public abstract class MineLibClient : IDisposable
    {
        public abstract string ClientBrand { get; }

        public string ClientUsername { get; set; }
        public string ClientLogin { get; }
        public string ClientPassword { get; }
        public bool UseLogin { get; }
        
        protected NetworkHandler NetworkHandler { get; set; }
        public string ServerHost => NetworkHandler.Host;
        public ushort ServerPort => NetworkHandler.Port;
        public bool Connected => NetworkHandler.Connected;
        public ConnectionState ConnectionState => NetworkHandler.ConnectionState;
        public IStatusClient CreateStatusClient => NetworkHandler.CreateStatusClient;

        protected ProtocolType Mode { get; }

        public World World { get; set; }


        protected MineLibClient(string login, string password, ProtocolType mode, bool nameVerification = false)
        {
            ClientLogin = login;
            ClientPassword = password;
            UseLogin = nameVerification;
            Mode = mode;
        }
        
        public abstract void Connect(IServerInfo serverInfo);
        public abstract void Disconnect();

        public abstract void RegisterReceiveEvent<TReceiveEvent>(Action<TReceiveEvent> func) where TReceiveEvent : ReceiveEvent;
        public abstract void DeregisterReceiveEvent<TReceiveEvent>(Action<TReceiveEvent> func) where TReceiveEvent : ReceiveEvent;
        public abstract void DoReceiveEvent<TReceiveEvent>(TReceiveEvent args) where TReceiveEvent : ReceiveEvent;

        public abstract void Dispose();
    }
}