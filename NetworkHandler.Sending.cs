using System;
using MineLib.Network.Data.Structs;

namespace MineLib.Network
{
    public sealed partial class NetworkHandler
    {
        public IAsyncResult BeginConnectToServer(AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginConnectToServer(asyncCallback, state);
        }

        public IAsyncResult BeginKeepAlive(Int32 value, AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginKeepAlive(value, asyncCallback, state);
        }

        public IAsyncResult BeginSendClientInfo(AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginSendClientInfo(asyncCallback, state);
        }

        public IAsyncResult BeginRespawn(AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginRespawn(asyncCallback, state);
        }

        public IAsyncResult BeginPlayerMoved(PlaverMovedData data, AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginPlayerMoved(data, asyncCallback, state);
        }

        public IAsyncResult BeginPlayerSetRemoveBlock(PlayerSetRemoveBlockData data, AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginPlayerSetRemoveBlock(data, asyncCallback, state);
        }

        public IAsyncResult BeginSendMessage(String message, AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginSendMessage(message, asyncCallback, state);
        }

        public IAsyncResult BeginPlayerHeldItem(Int16 slot, AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginPlayerHeldItem(slot, asyncCallback, state);
        }
    }
}
