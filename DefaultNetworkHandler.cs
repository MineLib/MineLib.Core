using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Aragas.Core.Wrappers;

using MineLib.Core.Interfaces;
using MineLib.Core.Loader;

namespace MineLib.Core
{
    /// <summary>
    /// Standard NetworkHandler, ready for use.
    /// </summary>
    public sealed class DefaultNetworkHandler : INetworkHandler
    {
        #region Properties

        public ProtocolType NetworkMode { get { return _minecraft.Mode; } }

        public ConnectionState ConnectionState { get { return _protocol.State; } }

        public bool Connected { get { return _protocol.Connected; } }

        public bool UseLogin { get { return _minecraft.UseLogin; } }

        public bool SavePackets { get { return _protocol.SavePackets; } }

        #endregion

        private IMinecraftClient _minecraft; // -- Readonly.
        private IProtocol _protocol;


        public List<ProtocolAssembly> GetModules()
        {
            var protocols = new List<ProtocolAssembly>();

            if (FileSystemWrapper.AssemblyFolder != null)
                foreach (var file in FileSystemWrapper.AssemblyFolder.GetFilesAsync().Result)
                    if (FitsMask(file.Name, "Protocol*.dll"))
                        protocols.Add(new ProtocolAssembly(file.Name));

#if DEBUG
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


        public INetworkHandler Initialize(IMinecraftClient client, ProtocolAssembly module, bool debugPackets = false)
        {
            _minecraft = client;

            _protocol = ProtocolAssemblyLoader.CreateProtocol(module.FileName);
            if (_protocol == null)
                throw new NetworkHandlerException(string.Format("Module loading error: {0} was not found or corrupted.", module.FileName), new InvalidCastException("IProtocol"));

            _protocol.Initialize(_minecraft, debugPackets);

            // TODO: Make async
            if (UseLogin)
                if(!_protocol.Login(_minecraft.ClientLogin, _minecraft.ClientPassword).Result)
                    throw new ProtocolException("Login Failed");

            return this;
        }


        public void Connect(string host, ushort port)
        {
            _protocol.Connect(host, port);
        }

        public void Disconnect()
        {
            _protocol.Disconnect();
        }

        public void DoSending(Type sendingType, SendingArgs args)
        {
            _protocol.DoSending(sendingType, args);
        }


        public Task ConnectAsync(string ip, ushort port)
        {
            return _protocol.ConnectAsync(ip, port);
        }

        public bool DisconnectAsync()
        {
            return _protocol.DisconnectAsync();
        }


        public void Dispose()
        {
            if (_protocol != null)
                _protocol.Dispose();
        }
    }
}