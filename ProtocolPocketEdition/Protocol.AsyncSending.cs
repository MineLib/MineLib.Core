using System;
using System.Collections.Generic;
using System.Linq;
using MineLib.Network;

namespace ProtocolPocketEdition
{
    public partial class Protocol
    {
        private Dictionary<Type, Func<IAsyncSendingParameters, IAsyncResult>> AsyncSendingHandlers { get; set; }

        public void RegisterAsyncSending(Type asyncSendingType, Func<IAsyncSendingParameters, IAsyncResult> method)
        {
            bool any = asyncSendingType.GetInterfaces().Any(p => p == typeof(IAsyncSending));
            if (!any)
                throw new InvalidOperationException("AsyncSending type must implement MineLib.Network.IAsyncSending");

            AsyncSendingHandlers[asyncSendingType] = method;
        }


        private void RegisterSupportedAsyncSendings()
        {
            RegisterAsyncSending(typeof(BeginConnectToServer), BeginConnectToServer);
            RegisterAsyncSending(typeof(BeginKeepAlive), BeginKeepAlive);
            RegisterAsyncSending(typeof(BeginSendClientInfo), BeginSendClientInfo);
            RegisterAsyncSending(typeof(BeginRespawn), BeginRespawn);
            RegisterAsyncSending(typeof(BeginPlayerMoved), BeginPlayerMoved);
            RegisterAsyncSending(typeof(BeginPlayerSetRemoveBlock), BeginPlayerSetRemoveBlock);
            RegisterAsyncSending(typeof(BeginSendMessage), BeginSendMessage);
            RegisterAsyncSending(typeof(BeginPlayerHeldItem), BeginPlayerHeldItem);
        }

        public IAsyncResult DoAsyncSending(Type asyncSendingType, IAsyncSendingParameters parameters)
        {
            bool any = asyncSendingType.GetInterfaces().Any(p => p == typeof(IAsyncSending));
            if (!any)
                throw new InvalidOperationException("AsyncSending type must implement MineLib.Network.IAsyncSending");

            if (!AsyncSendingHandlers.ContainsKey(asyncSendingType))
                return null;

            return AsyncSendingHandlers[asyncSendingType](parameters);
        }


        private IAsyncResult BeginConnectToServer(IAsyncSendingParameters parameters)
        {
            var param = (BeginConnectToServerParameters) parameters;

            return null;
        }

        private IAsyncResult BeginKeepAlive(IAsyncSendingParameters parameters)
        {
            var param = (BeginKeepAliveParameters) parameters;

            return null;
        }

        private IAsyncResult BeginSendClientInfo(IAsyncSendingParameters parameters)
        {
            var param = (BeginSendClientInfoParameters) parameters;

            return null;
        }

        private IAsyncResult BeginRespawn(IAsyncSendingParameters parameters)
        {
            var param = (BeginRespawnParameters) parameters;

            return null;
        }

        private IAsyncResult BeginPlayerMoved(IAsyncSendingParameters parameters)
        {
            var param = (BeginPlayerMovedParameters) parameters;
            var data = param.PlaverMovedData;

            return null;
        }

        private IAsyncResult BeginPlayerSetRemoveBlock(IAsyncSendingParameters parameters)
        {
            var param = (BeginPlayerSetRemoveBlockParameters)parameters;
            var data = param.PlayerSetRemoveBlockData;

            return null;
        }

        private IAsyncResult BeginSendMessage(IAsyncSendingParameters parameters)
        {
            var param = (BeginSendMessageParameters) parameters;

            return null;
        }

        private IAsyncResult BeginPlayerHeldItem(IAsyncSendingParameters parameters)
        {
            var param = (BeginPlayerHeldItemParameters) parameters;

            return null;
        }
    }
}
