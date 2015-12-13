using MineLib.Core.Data.Structs;

namespace MineLib.Core.Events.SendingEvents
{
    public class PlayerSetRemoveBlockEvent : SendingEvent {  }

    public class PlayerSetRemoveBlockEventArgs : SendingEventArgs
    {
        public PlayerSetRemoveBlockMode Mode { get; private set; }
        public IPlayerSetRemoveBlockData Data { get; private set; }

        public PlayerSetRemoveBlockEventArgs(IPlayerSetRemoveBlockData data)
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

        public PlayerSetRemoveBlockEventArgs(PlayerSetRemoveBlockMode mode, IPlayerSetRemoveBlockData data)
        {
            Mode = mode;

            Data = data;
        }
    }
}