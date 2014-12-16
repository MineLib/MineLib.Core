using System;
using System.Collections.Generic;
using System.Linq;
using MineLib.Network;
using MineLib.Network.Data.Structs;

namespace ProtocolMinetest
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

            State = ConnectionState.JoiningServer;

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
            switch (param.Mode)
            {
                case PlaverMovedMode.OnGround:
                    {
                        var data = (PlaverMovedDataOnGround) param.Data;
                        return null;
                    }

                case PlaverMovedMode.Vector3:
                    {
                        var data = (PlaverMovedDataVector3) param.Data;
                        return null;
                    }

                case PlaverMovedMode.YawPitch:
                    {
                        var data = (PlaverMovedDataYawPitch) param.Data;
                        return null;
                    }

                case PlaverMovedMode.All:
                    {
                        var data = (PlaverMovedDataAll) param.Data;
                        return null;
                    }

                default:
                    throw new Exception("PacketError");
            }
        }

        private IAsyncResult BeginPlayerSetRemoveBlock(IAsyncSendingParameters parameters)
        {
            var param = (BeginPlayerSetRemoveBlockParameters)parameters;
            switch (param.Mode)
            {
                case PlayerSetRemoveBlockMode.Place:
                {
                    var data = (PlayerSetRemoveBlockDataPlace) param.Data;
                    return null;
                }

                case PlayerSetRemoveBlockMode.Dig:
                {
                    var data = (PlayerSetRemoveBlockDataDig) param.Data;
                    return null;
                }

                case PlayerSetRemoveBlockMode.Remove:
                {
                    var data = (PlayerSetRemoveBlockDataRemove)param.Data;
                    return null;
                }

                default:
                    throw new Exception("PacketError");
            }
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
