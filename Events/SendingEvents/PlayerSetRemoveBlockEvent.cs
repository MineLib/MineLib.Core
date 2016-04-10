using MineLib.Core.Data.Structs;

namespace MineLib.Core.Events.SendingEvents
{
    public class PlayerSetRemoveBlockEvent : SendingEvent
    {
        public PlayerSetRemoveBlockMode Mode { get; private set; }
        public IPlayerSetRemoveBlockData Data { get; private set; }

        public PlayerSetRemoveBlockEvent(IPlayerSetRemoveBlockData data)
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

        public PlayerSetRemoveBlockEvent(PlayerSetRemoveBlockMode mode, IPlayerSetRemoveBlockData data)
        {
            Mode = mode;

            Data = data;
        }
    }
}