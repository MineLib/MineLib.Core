using System;
using System.Threading.Tasks;

namespace MineLib.Core.Wrappers
{
    public interface INetworkTCPAsync
    {
        Task ConnectAsync(String ip, UInt16 port);

        Boolean DisconnectAsync();

        Task SendAsync(Byte[] bytes, Int32 offset, Int32 count);

        Task<Int32> ReceiveAsync(Byte[] bytes, Int32 offset, Int32 count);
    }

    public interface INetworkTCP : INetworkTCPAsync, IDisposable
    {
        Boolean Connected { get; }


        void Connect(String ip, UInt16 port);
        void Disconnect();


        void Send(Byte[] bytes, Int32 offset, Int32 count);
        Int32 Receive(Byte[] buffer, Int32 offset, Int32 count);

        INetworkTCP NewInstance();
    }

    public static class NetworkTCPWrapper
    {
        private static INetworkTCP _instance;
        public static INetworkTCP Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static INetworkTCP NewNetworkTcp() { return Instance.NewInstance(); }
    }
}
