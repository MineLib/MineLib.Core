using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MineLib.Core.Events;
using MineLib.Core.Loader;

namespace MineLib.Core
{
    public class ChoseModuleEventArgs : EventArgs
    {
        public List<ProtocolAssembly> Modules { get; }
        public ProtocolAssembly ChosedModule { get; private set; }
        
        public ChoseModuleEventArgs(List<ProtocolAssembly> modules) { Modules = modules; }
        
        public void SetModule(ProtocolAssembly module) { ChosedModule = module; }
    }

    // TODO: Clean this mess.
    public abstract class MineLibClient : IDisposable
    {
        public ProtocolType Mode { get; }

        public string ClientUsername { get; set; }
        public string ClientLogin { get; }
        public string ClientPassword { get; }
        public bool UseLogin { get; }

        public abstract string ClientBrand { get; }
        public string ServerBrand { get; protected set; }

        public abstract string ServerHost { get; }
        public abstract ushort ServerPort { get; }

        public abstract ConnectionState ConnectionState { get; }
        public abstract bool Connected { get; }

        protected NetworkHandler NetworkHandler { get; set; }


        protected MineLibClient(string login, string password, ProtocolType mode, bool nameVerification = false)
        {
            ClientLogin = login;
            ClientPassword = password;
            UseLogin = nameVerification;
            Mode = mode;
        }
        
        public abstract void Connect(string ip, ushort port);
        public abstract void Disconnect();

        public abstract void RegisterReceiveEvent(Type receiveType, Func<ReceiveEvent, Task> func);
        public abstract void DeregisterReceiveEvent(Type receiveType, Func<ReceiveEvent, Task> func);
        public abstract void DoReceiveEvent(Type receiveType, ReceiveEvent args);

        public abstract void Dispose();
    }
}