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


        public ChoseModuleEventArgs(List<ProtocolAssembly> modules)
        {
            Modules = modules;
        }


        public void SetModule(ProtocolAssembly module)
        {
            ChosedModule = module;
        }
    }

    // TODO: Clean this mess.
    public abstract class MineLibClient : IDisposable
    {
        public abstract event EventHandler<ChoseModuleEventArgs> ChoseModule;

        public ProtocolType Mode { get; }

        public String ClientUsername { get; set; }
        public String ClientLogin { get; }
        public String ClientPassword { get; }
        public Boolean UseLogin { get; }

        public abstract String ClientBrand { get; }
        public String ServerBrand { get; protected set; }

        public abstract String ServerHost { get; }
        public abstract UInt16 ServerPort { get; }

        public abstract ConnectionState ConnectionState { get; }
        public abstract Boolean Connected { get; }

        protected NetworkHandler NetworkHandler { get; set; }


        protected MineLibClient(String login, String password, ProtocolType mode, Boolean nameVerification = false)
        {
            ClientLogin = login;
            ClientPassword = password;
            UseLogin = nameVerification;
            Mode = mode;
        }


        public abstract void Connect(String ip, UInt16 port);
        public abstract void Disconnect();

        public abstract void RegisterReceiveEvent(Type receiveType, Func<ReceiveEvent, Task> func);
        public abstract void DeregisterReceiveEvent(Type receiveType, Func<ReceiveEvent, Task> func);
        public abstract void DoReceiveEvent(Type receiveType, ReceiveEvent args);

        public abstract void Dispose();
    }
}