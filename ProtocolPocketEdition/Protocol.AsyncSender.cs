using System;
using MineLib.Network;
using MineLib.Network.Data;

namespace ProtocolPocketEdition
{
    public partial class Protocol
    {
        public IAsyncResult BeginConnectToServer(AsyncCallback asyncCallback, object state)
        {
            ConnectionState = ConnectionState.JoiningServer;

            return null;
        }

        public IAsyncResult BeginKeepAlive(int value, AsyncCallback asyncCallback, object state)
        {
            return null;
        }

        public IAsyncResult BeginSendClientInfo(AsyncCallback asyncCallback, object state)
        {
            return null;
        }

        public IAsyncResult BeginRespawn(AsyncCallback asyncCallback, object state)
        {
            return null;
        }

        public IAsyncResult BeginPlayerMoved(PlaverMovedData data, AsyncCallback asyncCallback, object state)
        {
            return null;
        }

        public IAsyncResult BeginPlayerSetRemoveBlock(PlayerSetRemoveBlockData data, AsyncCallback asyncCallback, object state)
        {
            return null;
        }

        public IAsyncResult BeginSendMessage(string message, AsyncCallback asyncCallback, object state)
        {
            return null;
        }

        public IAsyncResult BeginPlayerHeldItem(short slot, AsyncCallback asyncCallback, object state)
        {
            return null;
        }
    }
}
