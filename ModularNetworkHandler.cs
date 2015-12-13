using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Aragas.Core.Wrappers;

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


        public override List<ProtocolAssembly> GetModules()
        {
            var protocols = new List<ProtocolAssembly>();

            if (FileSystemWrapper.AssemblyFolder != null)
                foreach (var file in FileSystemWrapper.AssemblyFolder.GetFilesAsync().Result)
                    if (FitsMask(file.Name, "Protocol*.dll"))
                        protocols.Add(new ProtocolAssembly(file.Name));

#if DEBUG //|| !DEBUG
            if (protocols.Count == 0)
                protocols.Add(new ProtocolAssembly("ProtocolModern.Portable"));
#endif

            return protocols;
        }
        private static bool FitsMask(string sFileName, string sFileMask)
        {
            var mask = new Regex(sFileMask.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));
            return mask.IsMatch(sFileName);
        }
        
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}