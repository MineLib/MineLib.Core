using Aragas.Core.Packets;

using MineLib.Core.Client;

namespace MineLib.Core
{
    public enum ModAPISide { Client, Server}
    public abstract class ModAPI
    {
        protected ModAPISide Side { get; }
        protected IModAPIContext Context { get; }


        protected ModAPI(ModAPISide side, IModAPIContext context) { Side = side; Context = context; }


        public abstract void OnConnect(ServerInfo serverInfo);

        public abstract void OnPacket(ProtobufPacket protobufPacket);
    }
}
