using System;

using MineLib.Network.Data.Structs;

namespace MineLib.Network
{
    /// <summary>
    /// Interface for registering supported sending types.
    /// </summary>
    public interface IAsyncSending { }

    /// <summary>
    /// Basic interface for registering supported receive types.
    /// </summary>
    public interface IAsyncSendingArgs
    {
        AsyncCallback AsyncCallback { get; } 
        Object State { get; }
    }


    public struct BeginConnectToServer : IAsyncSending { }
    public struct BeginConnectToServerArgs : IAsyncSendingArgs 
    {
        public AsyncCallback AsyncCallback { get; private set; }
        public object State { get; private set; }

        public BeginConnectToServerArgs(AsyncCallback asyncCallback, object state) : this()
        {
            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public struct BeginKeepAlive : IAsyncSending { }
    public struct BeginKeepAliveArgs : IAsyncSendingArgs
    {
        public int KeepAlive { get; private set; }

        public AsyncCallback AsyncCallback { get; private set; }
        public object State { get; private set; }

        public BeginKeepAliveArgs(int value, AsyncCallback asyncCallback, object state) : this()
        {
            KeepAlive = value;

            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public struct BeginSendClientInfo : IAsyncSending { }
    public struct BeginSendClientInfoArgs : IAsyncSendingArgs
    {
        public AsyncCallback AsyncCallback { get; private set; }
        public object State { get; private set; }

        public BeginSendClientInfoArgs(AsyncCallback asyncCallback, object state) : this()
        {
            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public struct BeginRespawn : IAsyncSending { }
    public struct BeginRespawnArgs : IAsyncSendingArgs
    {
        public AsyncCallback AsyncCallback { get; private set; }
        public object State { get; private set; }

        public BeginRespawnArgs(AsyncCallback asyncCallback, object state) : this()
        {
            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public struct BeginPlayerMoved : IAsyncSending { }
    public struct BeginPlayerMovedArgs : IAsyncSendingArgs
    {
        public AsyncCallback AsyncCallback { get; private set; }
        public object State { get; private set; }

        public PlaverMovedMode Mode { get; private set; }
        public IPlaverMovedData Data { get; private set; }

        public BeginPlayerMovedArgs(IPlaverMovedData data, AsyncCallback asyncCallback, object state) : this()
        {
            {
                var type = data as PlaverMovedDataOnGround;
                if (type != null)
                    Mode = PlaverMovedMode.OnGround;
            }

            {
                var type = data as PlaverMovedDataVector3;
                if (type != null)
                    Mode = PlaverMovedMode.Vector3;
            }

            {
                var type = data as PlaverMovedDataYawPitch;
                if (type != null)
                    Mode = PlaverMovedMode.YawPitch;
            }

            {
                var type = data as PlaverMovedDataAll;
                if (type != null)
                    Mode = PlaverMovedMode.All;
            }

            Data = data;

            AsyncCallback = asyncCallback;
            State = state;
        }

        public BeginPlayerMovedArgs(PlaverMovedMode mode, IPlaverMovedData data, AsyncCallback asyncCallback, object state): this()
        {
            Mode = mode;

            Data = data;

            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public struct BeginPlayerSetRemoveBlock : IAsyncSending { }
    public struct BeginPlayerSetRemoveBlockArgs : IAsyncSendingArgs
    {
        public AsyncCallback AsyncCallback { get; private set; }
        public object State { get; private set; }

        public PlayerSetRemoveBlockMode Mode { get; private set; }
        public IPlayerSetRemoveBlockData Data { get; private set; }

        public BeginPlayerSetRemoveBlockArgs(IPlayerSetRemoveBlockData data, AsyncCallback asyncCallback, object state) : this()
        {
            {
                var type = data as PlayerSetRemoveBlockDataDig;
                if (type != null)
                    Mode = PlayerSetRemoveBlockMode.Dig;
            }

            {
                var type = data as PlayerSetRemoveBlockDataPlace;
                if (type != null)
                    Mode = PlayerSetRemoveBlockMode.Place;
            }

            {
                var type = data as PlayerSetRemoveBlockDataRemove;
                if (type != null)
                    Mode = PlayerSetRemoveBlockMode.Remove;
            }

            Data = data;

            AsyncCallback = asyncCallback;
            State = state;
        }

        public BeginPlayerSetRemoveBlockArgs(PlayerSetRemoveBlockMode mode, IPlayerSetRemoveBlockData data, AsyncCallback asyncCallback, object state) : this()
        {
            Mode = mode;

            Data = data;

            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public struct BeginSendMessage : IAsyncSending { }
    public struct BeginSendMessageArgs : IAsyncSendingArgs
    {
        public AsyncCallback AsyncCallback { get; private set; }
        public object State { get; private set; }

        public string Message { get; private set; }

        public BeginSendMessageArgs(string message, AsyncCallback asyncCallback, object state) : this()
        {
            Message = message;

            AsyncCallback = asyncCallback;
            State = state;
        }
    }

    public struct BeginPlayerHeldItem : IAsyncSending { }
    public struct BeginPlayerHeldItemArgs : IAsyncSendingArgs
    {
        public AsyncCallback AsyncCallback { get; private set; }
        public object State { get; private set; }

        public short Slot { get; private set; }

        public BeginPlayerHeldItemArgs(short slot, AsyncCallback asyncCallback, object state) : this()
        {
            Slot = slot;

            AsyncCallback = asyncCallback;
            State = state;
        }
    }
}
