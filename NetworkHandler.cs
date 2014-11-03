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
        public ConnectionState ConnectionState { get { return _protocol.State; } }

        public bool SavePackets { get { return _protocol.SavePackets; } }

        public bool Connected { get { return _protocol.Connected; } }

        #endregion

        private IMinecraftClient _minecraft;
        private IProtocol _protocol;


        /// <summary>
        /// Start NetworkHandler.
        /// </summary>
        public INetworkHandler Create(IMinecraftClient client, bool debugPackets = true)
        {
            _minecraft = client;

            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            _protocol = ModuleLoader.CreateModule<IProtocol>(string.Format(path + "\\{0}.dll", NetworkMode));
            if (_protocol == null)
                throw new NetworkHandlerException( string.Format("Module loading error: {0}.dll was not found or corrupted.", NetworkMode));
            
            _protocol.Create(_minecraft, debugPackets);

            return this;
        }

        public IAsyncResult BeginConnect(string ip, short port, AsyncCallback asyncCallback, object state)
        {
            return _protocol.BeginConnect(ip, port, asyncCallback, state);
        }

        public void EndConnect(IAsyncResult asyncResult)
        {
            _protocol.EndConnect(asyncResult);
        }

        public IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, object state)
        {
            return _protocol.BeginDisconnect(asyncCallback, state);
        }

        public void EndDisconnect(IAsyncResult asyncResult)
        {
            _protocol.EndDisconnect(asyncResult);
        }


        public void Connect()
        {
            _protocol.Connect();
        }

        public void Disconnect()
        {
            _protocol.Disconnect();

            Dispose();
        }


        /// <summary>
        /// Dispose NetworkHandler.
        /// </summary>
        public void Dispose()
        {
            if (_protocol != null)
                _protocol.Dispose();
        }
    }
}