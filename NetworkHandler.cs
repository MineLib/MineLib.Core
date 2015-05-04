using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using MineLib.Network.IO;
using MineLib.Network.Module;

namespace MineLib.Network
{
    /// <summary>
    /// Standard NetworkHandler, ready for use.
    /// </summary>
    public sealed class NetworkHandler : INetworkHandler
    {
        #region Properties

        public ProtocolType NetworkMode { get { return _minecraft.Mode; } }

        public ConnectionState ConnectionState { get { return _protocol.State; } }

        public bool Connected { get { return _protocol.Connected; } }

        public bool UseLogin { get { return _minecraft.UseLogin; } }

        public bool SavePackets { get { return _protocol.SavePackets; } }

        #endregion

        public event LoadAssembly LoadAssembly { add { ModuleLoader.LoadAssembly += value; } remove { ModuleLoader.LoadAssembly -= value; } }

        private event Storage _getStorage;
        public event Storage GetStorage { add { _getStorage += value; ModuleLoader.GetStorage += value; } remove { _getStorage -= value; ModuleLoader.GetStorage -= value; } }

        private IMinecraftClient _minecraft; // -- Readonly.
        private IProtocol _protocol;


        public List<ProtocolModule> GetModules()
        {
            var protocols = new List<ProtocolModule>();

            if (_getStorage != null)
                foreach (var file in _getStorage(this).GetFilesAsync().Result)
                    if(FitsMask(file.Name, "Protocol*.dll"))
                        protocols.Add(new ProtocolModule(file.Name));

            protocols.Add(new ProtocolModule("ProtocolModern.Portable"));
            return protocols;
        }

        private static bool FitsMask(string sFileName, string sFileMask)
        {
            var mask = new Regex(sFileMask.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));
            return mask.IsMatch(sFileName);
        }


        public INetworkHandler Initialize(ProtocolModule module, IMinecraftClient client, INetworkTCP tcp, bool debugPackets = false)
        {
            _minecraft = client;

            _protocol = ModuleLoader.CreateModule<IProtocol>(module.FileName);
            if (_protocol == null)
                throw new NetworkHandlerException(string.Format("Module loading error: {0} was not found or corrupted.", module.FileName), new InvalidCastException("IProtocol"));

            _protocol.Initialize(_minecraft, tcp, debugPackets);

            // TODO: Make async
            if (UseLogin)
                _protocol.Login(_minecraft.ClientLogin, _minecraft.ClientPassword);

            return this;
        }


        public IAsyncResult BeginConnect(string ip, ushort port, AsyncCallback asyncCallback, object state)
        {
            return _protocol.BeginConnect(ip, port, asyncCallback, state);
        }

        public IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, object state)
        {
            return _protocol.BeginDisconnect(asyncCallback, state);
        }

        public void EndDisconnect(IAsyncResult asyncResult)
        {
            _protocol.EndDisconnect(asyncResult);
        }


        public IAsyncResult DoAsyncSending(Type asyncSendingType, IAsyncSendingArgs parameters)
        {
            return _protocol.DoAsyncSending(asyncSendingType, parameters);
        }


        public void Connect(string ip, ushort port)
        {
            _protocol.Connect(ip, port);
        }

        public void Disconnect()
        {
            _protocol.Disconnect();
        }


        public void Dispose()
        {
            if (_protocol != null)
                _protocol.Dispose();
        }
    }
}