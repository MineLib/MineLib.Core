using System;
using MineLib.Network.Data.Structs;

namespace MineLib.Network
{
    public interface IAsyncSending { }

    public interface IAsyncSendingParameters
    {
        AsyncCallback AsyncCallback { get; set; } 
        object State { get; set; }
    }

    public class BeginConnectToServer : IAsyncSending { }
    public struct BeginConnectToServerParameters : IAsyncSendingParameters 
    {
        public AsyncCallback AsyncCallback { get; set; }
        public object State { get; set; }

        public BeginConnectToServerParameters(AsyncCallback asyncCallback, object state) : this()
        {
            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public class BeginKeepAlive : IAsyncSending { }
    public struct BeginKeepAliveParameters : IAsyncSendingParameters
    {
        public int KeepAlive { get; set; }

        public AsyncCallback AsyncCallback { get; set; }
        public object State { get; set; }

        public BeginKeepAliveParameters(int value, AsyncCallback asyncCallback, object state) : this()
        {
            KeepAlive = value;

            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public class BeginSendClientInfo : IAsyncSending { }
    public struct BeginSendClientInfoParameters : IAsyncSendingParameters
    {
        public AsyncCallback AsyncCallback { get; set; }
        public object State { get; set; }

        public BeginSendClientInfoParameters(AsyncCallback asyncCallback, object state) : this()
        {
            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public class BeginRespawn : IAsyncSending { }
    public struct BeginRespawnParameters : IAsyncSendingParameters
    {
        public AsyncCallback AsyncCallback { get; set; }
        public object State { get; set; }

        public BeginRespawnParameters(AsyncCallback asyncCallback, object state) : this()
        {
            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public class BeginPlayerMoved : IAsyncSending { }
    public struct BeginPlayerMovedParameters : IAsyncSendingParameters
    {
        public PlaverMovedData PlaverMovedData { get; set; }

        public AsyncCallback AsyncCallback { get; set; }
        public object State { get; set; }

        public BeginPlayerMovedParameters(PlaverMovedData data, AsyncCallback asyncCallback, object state) : this()
        {
            PlaverMovedData = data;

            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public class BeginPlayerSetRemoveBlock : IAsyncSending { }
    public struct BeginPlayerSetRemoveBlockParameters : IAsyncSendingParameters
    {
        public AsyncCallback AsyncCallback { get; set; }
        public object State { get; set; }

        public PlayerSetRemoveBlockData PlayerSetRemoveBlockData { get; set; }

        public BeginPlayerSetRemoveBlockParameters(PlayerSetRemoveBlockData data, AsyncCallback asyncCallback, object state) : this()
        {
            PlayerSetRemoveBlockData = data;

            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public class BeginSendMessage : IAsyncSending { }
    public struct BeginSendMessageParameters : IAsyncSendingParameters
    {
        public AsyncCallback AsyncCallback { get; set; }
        public object State { get; set; }

        public string Message { get; set; }

        public BeginSendMessageParameters(string message, AsyncCallback asyncCallback, object state) : this()
        {
            Message = message;

            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public class BeginPlayerHeldItem : IAsyncSending { }
    public struct BeginPlayerHeldItemParameters : IAsyncSendingParameters
    {
        public AsyncCallback AsyncCallback { get; set; }
        public object State { get; set; }

        public short Slot { get; set; }

        public BeginPlayerHeldItemParameters(short slot, AsyncCallback asyncCallback, object state) : this()
        {
            Slot = slot;

            AsyncCallback = asyncCallback;
            State = state;
        }
    }
}
