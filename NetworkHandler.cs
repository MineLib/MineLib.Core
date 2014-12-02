using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using MineLib.Network.Module;

namespace MineLib.Network
{
    public sealed class NetworkHandler : INetworkHandler
    {
        #region Properties

        public NetworkMode NetworkMode { get { return _minecraft.Mode; } }
        public ConnectionState ConnectionState { get { return _protocol.State; } }

        public bool Connected { get { return _protocol.Connected; } }

        public bool UseLogin { get { return _minecraft.UseLogin; }}

        public bool SavePackets { get { return _protocol.SavePackets; } }

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

            _protocol = ModuleLoader.CreateModule<IProtocol>(string.Format(CultureInfo.InvariantCulture, path + "\\{0}.dll", NetworkMode)).Create(_minecraft, debugPackets);
            if (_protocol == null)
                throw new NetworkHandlerException(string.Format("Module loading error: {0}.dll was not found or corrupted.", NetworkMode));

            // -- Make async
            if (UseLogin)
                _protocol.Login(_minecraft.ClientLogin, _minecraft.ClientPassword);

            return this;
        }

        /// <summary>
        /// EndConnect is called automatically by IProtocol.
        /// </summary>
        public IAsyncResult BeginConnect(string ip, ushort port, AsyncCallback asyncCallback, object state)
        {
            return _protocol.BeginConnect(ip, port, asyncCallback, state);
        }

        //private void EndConnect(IAsyncResult asyncResult)
        //{
        //    _protocol.EndConnect(asyncResult);
        //}

        public IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, object state)
        {
            return _protocol.BeginDisconnect(asyncCallback, state);
        }

        public void EndDisconnect(IAsyncResult asyncResult)
        {
            _protocol.EndDisconnect(asyncResult);
        }

        public IAsyncResult DoAsyncSending(Type asyncSendingType, IAsyncSendingParameters parameters)
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