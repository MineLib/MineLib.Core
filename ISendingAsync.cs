using MineLib.Core.Data.Structs;

namespace MineLib.Core
{
    public interface ISendingAsync { }
    public interface ISendingAsyncArgs { }

    public struct ConnectToServerAsync : ISendingAsync { }
    public struct ConnectToServerAsyncArgs : ISendingAsyncArgs { }

    public struct KeepAliveAsync : ISendingAsync { }
    public struct KeepAliveAsyncArgs : ISendingAsyncArgs
    {
        public int KeepAlive { get; private set; }

        public KeepAliveAsyncArgs(int value) : this()
        {
            KeepAlive = value;
        }
    }

    public struct SendClientInfoAsync : ISendingAsync { }
    public struct SendClientInfoAsyncArgs : ISendingAsyncArgs { }

    public struct RespawnAsync : ISendingAsync { }
    public struct RespawnAsyncArgs : ISendingAsyncArgs { }

    public struct PlayerMovedAsync : ISendingAsync { }

    public struct PlayerMovedAsyncArgs : ISendingAsyncArgs
    {
        public PlaverMovedMode Mode { get; private set; }
        public IPlaverMovedData Data { get; private set; }

        public PlayerMovedAsyncArgs(IPlaverMovedData data) : this()
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
        }

        public PlayerMovedAsyncArgs(PlaverMovedMode mode, IPlaverMovedData data) : this()
        {
            Mode = mode;

            Data = data;
        }
    }

    public struct PlayerSetRemoveBlockAsync : ISendingAsync { }

    public struct PlayerSetRemoveBlockAsyncArgs : ISendingAsyncArgs
    {
        public PlayerSetRemoveBlockMode Mode { get; private set; }
        public IPlayerSetRemoveBlockData Data { get; private set; }

        public PlayerSetRemoveBlockAsyncArgs(IPlayerSetRemoveBlockData data) : this()
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
        }

        public PlayerSetRemoveBlockAsyncArgs(PlayerSetRemoveBlockMode mode, IPlayerSetRemoveBlockData data) : this()
        {
            Mode = mode;

            Data = data;
        }
    }

    public struct SendMessageAsync : ISendingAsync { }
    public struct SendMessageAsyncArgs : ISendingAsyncArgs
    {
        public string Message { get; private set; }

        public SendMessageAsyncArgs(string message) : this()
        {
            Message = message;
        }
    }

    public struct PlayerHeldItemAsync : ISendingAsync { }
    public struct PlayerHeldItemAsyncArgs : ISendingAsyncArgs
    {
        public short Slot { get; private set; }

        public PlayerHeldItemAsyncArgs(short slot) : this()
        {
            Slot = slot;
        }
    }
}
