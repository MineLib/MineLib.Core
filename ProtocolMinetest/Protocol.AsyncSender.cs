using System;
using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.Data.Structs;
using ProtocolMinetest.Packets.Client;

namespace ProtocolMinetest
{
    public partial class Protocol
    {
        public IAsyncResult BeginConnectToServer(AsyncCallback asyncCallback, object state)
        {
            State = ConnectionState.JoiningServer;

            SendPacket(new ToServerInit
            {
            });

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
