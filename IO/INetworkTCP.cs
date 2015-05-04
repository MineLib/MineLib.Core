using System;

namespace MineLib.Network.IO
{
    public interface INetworkTCP : IDisposable
    {
        Boolean Available { get; }
        Boolean Connected { get; }


        void Connect(String ip, UInt16 port);
        void Disconnect(Boolean reuse);

        IAsyncResult BeginConnect(String ip, UInt16 port, AsyncCallback callback, Object state);
        void EndConnect(IAsyncResult result);

        IAsyncResult BeginDisconnect(Boolean reuse, AsyncCallback callback, Object state);
        void EndDisconnect(IAsyncResult result);


        void Send(Byte[] bytes, Int32 offset, Int32 count);
        Int32 Receive(Byte[] buffer, Int32 offset, Int32 count);

        IAsyncResult BeginSend(Byte[] bytes, Int32 offset, Int32 count, AsyncCallback callback, Object state);
        void EndSend(IAsyncResult result);

        IAsyncResult BeginReceive(Byte[] bytes, Int32 offset, Int32 count, AsyncCallback callback, Object state);
        Int32 EndReceive(IAsyncResult result);
    }
}
