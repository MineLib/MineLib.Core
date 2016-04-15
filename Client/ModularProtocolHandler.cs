using System;

using MineLib.Core.Exceptions;
using MineLib.Core.Loader;

namespace MineLib.Core.Client
{
    /// <summary>
    /// Default NetworkHandler, ready for use.
    /// </summary>
    public sealed class ModularProtocolHandler : ProtocolHandler
    {
        public ModularProtocolHandler(MineLibClient client, AssemblyInfo assemblyInfo, string login = "", string password = "") : base(client, assemblyInfo)
        {
            var protocolType = AssemblyParser.FindType<Protocol>(AssemblyInfo, "ProtocolModern_1.7.10");
            if (protocolType == null)
                throw new NetworkHandlerException($"Protocol loading error: {AssemblyInfo.FileName} was not found or corrupted.");

            try { Protocol = (Protocol) Activator.CreateInstance(protocolType, Client, ProtocolPurpose.Play); }
            catch (MissingMemberException) { throw new NetworkHandlerException("Protocol not supported."); }

            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
                if (!Protocol.Login(login, password).Result)
                    throw new NetworkHandlerException("Login Failed");
        }
    }
}