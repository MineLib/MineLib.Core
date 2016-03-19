using System;

using MineLib.Core.Exceptions;
using MineLib.Core.Loader;
using MineLib.Core.Protocols;

namespace MineLib.Core
{
    /// <summary>
    /// Standard NetworkHandler, ready for use.
    /// </summary>
    public sealed class ModularNetworkHandler : NetworkHandler
    {
        public ModularNetworkHandler(MineLibClient client, ProtocolAssembly protocol, bool debugPackets = false)
            : base(client, protocol, debugPackets)
        {
            var protocolType = ProtocolAssemblyLoader.GetProtocolType(protocol.FileName);
            if (protocolType == null)
                throw new NetworkHandlerException($"Protocol loading error: {protocol.FileName} was not found or corrupted.");

            Protocol = (Protocol) Activator.CreateInstance(protocolType, Client, debugPackets);

            if (UseLogin)
                if (!Protocol.Login(Client.ClientLogin, Client.ClientPassword).Result)
                    throw new ProtocolException("Login Failed");
        }
        
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}