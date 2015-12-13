namespace MineLib.Core.Events.SendingEvents
{
    public class PlayerHeldItemEvent : SendingEvent { }

    public class PlayerHeldItemEventArgs : SendingEventArgs
    {
        public short Slot { get; private set; }

        public PlayerHeldItemEventArgs(short slot)
        {
            Slot = slot;
        }
    }
}