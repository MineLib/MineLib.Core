using System;
using System.Threading.Tasks;

namespace MineLib.Core.IO
{
    // TODO: use https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Battery for cross-platform plugin
    public interface INetworkTCPAsync
    {
        Task ConnectAsync(String ip, UInt16 port);

        Boolean DisconnectAsync(Boolean reuse);

        Task SendAsync(Byte[] bytes, Int32 offset, Int32 count);

        Task<Int32> ReceiveAsync(Byte[] bytes, Int32 offset, Int32 count);
    }

    public interface INetworkTCP : INetworkTCPAsync, IDisposable
    {
        Boolean Available { get; }
        Boolean Connected { get; }


        void Connect(String ip, UInt16 port);
        void Disconnect(Boolean reuse);


        void Send(Byte[] bytes, Int32 offset, Int32 count);
        Int32 Receive(Byte[] buffer, Int32 offset, Int32 count);
    }
}
