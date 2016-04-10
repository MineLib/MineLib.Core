using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Aragas.Core.Wrappers;

using MineLib.Core.Exceptions;
using MineLib.Core.Loader;

namespace MineLib.Core
{
    /// <summary>
    /// Default NetworkHandler, ready for use.
    /// </summary>
    public sealed class ModularNetworkHandler : NetworkHandler
    {
        private static bool FitsMask(string sFileName, string sFileMask)
        {
            var mask = new Regex(sFileMask.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));
            return mask.IsMatch(sFileName);
        }
        public static IList<ProtocolAssembly> GetModules()
        {
            var files = FileSystemWrapper.AssemblyFolder.GetFilesAsync().Result;
            var assemblies = files.Where(file => FitsMask(file.Name, "Protocol*.dll"));
            return assemblies.Select(assembly => new ProtocolAssembly(assembly.Path)).ToList();
        }

        public ModularNetworkHandler(MineLibClient client, ProtocolAssembly protocol) : base(client, protocol)
        {
            var protocolType = ProtocolAssemblyLoader.GetProtocolType(ProtocolAssembly.FileName);
            if (protocolType == null)
                throw new NetworkHandlerException($"Protocol loading error: {ProtocolAssembly.FileName} was not found or corrupted.");

            try { Protocol = (Protocol) Activator.CreateInstance(protocolType, Client, ProtocolMode.Play); }
            catch (MissingMemberException) { throw new NetworkHandlerException("Protocol not supported."); }

            if (UseLogin && !Protocol.Login(Client.ClientLogin, Client.ClientPassword).Result)
                throw new NetworkHandlerException("Login Failed");
        }
    }
}