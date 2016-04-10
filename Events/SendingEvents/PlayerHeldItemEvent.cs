namespace MineLib.Core.Events.SendingEvents
{
    public class PlayerHeldItemEvent : SendingEvent
    {
        public short Slot { get; private set; }

        public PlayerHeldItemEvent(short slot)
        {
            Slot = slot;
        }
    }
}