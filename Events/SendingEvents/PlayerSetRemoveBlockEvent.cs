using MineLib.Core.Data.Structs;

namespace MineLib.Core.Events.SendingEvents
{
    public class PlayerSetRemoveBlockEvent : SendingEvent
    {
        public PlayerSetRemoveBlockMode Mode { get; }
        public IPlayerSetRemoveBlockData Data { get; }


        public PlayerSetRemoveBlockEvent(PlayerSetRemoveBlockMode mode, IPlayerSetRemoveBlockData data) { Mode = mode; Data = data; }
        public PlayerSetRemoveBlockEvent(IPlayerSetRemoveBlockData data)
        {
            if (data is PlayerSetRemoveBlockDataDig)
                Mode = PlayerSetRemoveBlockMode.Dig;
            else if (data is PlayerSetRemoveBlockDataPlace)
                Mode = PlayerSetRemoveBlockMode.Place;
            else if (data is PlayerSetRemoveBlockDataRemove)
                Mode = PlayerSetRemoveBlockMode.Remove;

            Data = data;
        }
    }
}