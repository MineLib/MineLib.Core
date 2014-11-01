using System;
using System.IO;
using System.Reflection;
using MineLib.Network.Module;

namespace MineLib.Network
{
    public sealed partial class NetworkHandler : INetworkHandler
    {
        #region Properties

        public NetworkMode NetworkMode { get { return _minecraft.Mode; } }
        public ConnectionState ConnectionState { get { return _protocol.ConnectionState; } }

        public bool SavePackets { get { return _protocol.SavePackets; } }

        public bool Connected { get { return _protocol.Connected; } }

        #endregion


        private IProtocol _protocol;

        private bool _customModule;


        /// <summary>
        /// Start NetworkHandler.
        /// </summary>
        public INetworkHandler Create(IMinecraftClient client, bool debugPackets = true)
        {
            _minecraft = client;

            if (File.Exists(string.Format(Environment.CurrentDirectory + "\\{0}.dll", NetworkMode)) && NetworkMode == NetworkMode.CustomModule)
                _customModule = true;
            else if(NetworkMode == NetworkMode.CustomModule)
                throw new NetworkHandlerException(string.Format("Custom module loading error: {0}.dll was not found.", NetworkMode));

            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            _protocol = ModuleLoader.CreateModule<IProtocol>(string.Format(path + "\\{0}.dll", NetworkMode));
            if(_protocol == null)
                throw new NetworkHandlerException(string.Format("Native module loading error: {0}.dll was not found or corrupted.", NetworkMode));

            _protocol.Create(_minecraft, debugPackets);
        
            return this;
        }

        public IAsyncResult BeginConnect(string ip, short port, AsyncCallback asyncCallback, object state)
        {
            if(_customModule)
                throw new NetworkHandlerException("Custom module error: Use non-async methods in a custom library");

            return _protocol.BeginConnect(ip, port, asyncCallback, state);
        }

        public void EndConnect(IAsyncResult asyncResult)
        {
            if (_customModule)
                throw new NetworkHandlerException("Custom module error: Use non-async methods in a custom library");

            _protocol.EndConnect(asyncResult);
        }

        public IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, object state)
        {
            if (_customModule)
                throw new NetworkHandlerException("Custom module error: Use non-async methods in a custom library");

            return _protocol.BeginDisconnect(asyncCallback, state);
        }

        public void EndDisconnect(IAsyncResult asyncResult)
        {
            if (_customModule)
                throw new NetworkHandlerException("Custom module error: Use non-async methods in a custom library");

            _protocol.EndDisconnect(asyncResult);
        }


        public void Connect()
        {
            if (_customModule)
                ModuleConnect(_minecraft.ServerHost, _minecraft.ServerPort);
            else
                _protocol.Connect();
        }

        public void Disconnect()
        {
            if (_customModule)
                ModuleDisconnect();
            else
                _protocol.Disconnect();

            Dispose();
        }


        /// <summary>
        /// Dispose NetworkHandler.
        /// </summary>
        public void Dispose()
        {
            if (_customModule)
                ModuleDispose();

            if (_protocol != null)
                _protocol.Dispose();
        }
    }
}